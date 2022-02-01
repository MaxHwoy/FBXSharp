using FBXSharp.Core;
using FBXSharp.ValueTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FBXSharp.Objective
{
	public class Geometry : FBXObject
	{
		public enum ChannelType : int
		{
			Normal,
			Tangent,
			Binormal,
			Color,
			TexCoord,
			Material,
		}

		public enum ComponentType : int
		{
			Int,
			Double,
			Double2,
			Double3,
			Double4,
		}

		public struct Channel
		{
			public int Layer { get; }
			public string Name { get; }
			public Array Buffer { get; }
			public ChannelType Type { get; }
			public ComponentType Size { get; }

			internal Channel(int layer, string name, ChannelType type, ComponentType size, Array array)
			{
				this.Layer = layer;
				this.Name = name ?? String.Empty;
				this.Type = type;
				this.Size = size;
				this.Buffer = array;
			}

			public override string ToString() => $"{this.Type} {this.Layer} | {this.Name}";
		}

		public struct SubMesh
		{
			public int PolygonStart { get; }
			public int PolygonCount { get; }
			public int MaterialIndex { get; }

			public SubMesh(int start, int count, int matIndex)
			{
				this.PolygonStart = start;
				this.PolygonCount = count;
				this.MaterialIndex = matIndex;
			}
		}

		private Vector3[] m_vertices;
		private int[][] m_indices;
		private SubMesh[] m_subMeshes;
		private readonly List<Channel> m_channels;
		private readonly ReadOnlyCollection<Channel> m_readonly;

		public static readonly FBXObjectType FType = FBXObjectType.Geometry;

		public override FBXObjectType Type => Geometry.FType;

		public Vector3[] Vertices => this.m_vertices;

		public int[][] Indices => this.m_indices;

		public SubMesh[] SubMeshes => this.m_subMeshes;

		public int IndexCount => this.InternalGetIndexCount();

		public ReadOnlyCollection<Channel> Channels => this.m_readonly;

		internal Geometry(IElement element, IScene scene) : base(element, scene)
		{
			this.m_vertices = Array.Empty<Vector3>();
			this.m_indices = Array.Empty<int[]>();
			this.m_subMeshes = Array.Empty<SubMesh>();
			this.m_channels = new List<Channel>();
			this.m_readonly = new ReadOnlyCollection<Channel>(this.m_channels);
		}

		private int InternalGetIndexCount()
		{
			int result = 0;

			for (int i = 0; i < this.m_indices.Length; ++i)
			{
				result += this.m_indices[i].Length;
			}

			return result;
		}

		internal void InternalSetVertices(Vector3[] vertices) => this.m_vertices = vertices;
		internal void InternalSetIndices(int[][] indices) => this.m_indices = indices;
		internal void InternalSetSubMeshes(SubMesh[] subMeshes) => this.m_subMeshes = subMeshes;
		internal void InternalSetChannel(in Channel channel) => this.m_channels.Add(channel);
		internal void InternalSortChannels() => this.m_channels.Sort(Geometry.ChannelSorter);

		private static int ChannelSorter(Channel a, Channel b)
		{
			var result = a.Type.CompareTo(b.Type);

			return result == 0 ? a.Layer.CompareTo(b.Layer) : result;
		}

		private static void MoveMaterials(IElement[] array, int start, int count)
		{
			var end = start + count - 1;

			for (int i = start; i < end; ++i)
			{
				var element = array[i];

				if (element.Name == "LayerElementMaterial")
				{
					while (i < end)
					{
						var temp = array[i + 1];
						array[i + 1] = array[i];
						array[i++] = temp;
					}

					return;
				}
			}
		}

		private int[] RecalculateEdges()
		{
			var mapper = new Dictionary<long, int>(this.IndexCount);

			for (int i = 0, k = 0; i < this.m_indices.Length; ++i)
			{
				var indexer = this.m_indices[i];

				for (int j = 0; j < indexer.Length; ++j)
				{
					long one = indexer[j];
					long two = indexer[(j + 1) % indexer.Length];

					var edge = one < two ? ((two << 0x20) | one) : ((one << 0x20) | two);

					if (!mapper.ContainsKey(edge))
					{
						mapper.Add(edge, k + j);
					}
				}

				k += indexer.Length;
			}

			var edges = new int[mapper.Count];

			mapper.Values.CopyTo(edges, 0);

			return edges;
		}

		private IElement ChannelToElement(in Channel channel)
		{
			var hasWs = channel.Size == ComponentType.Double4 ? 1 : 0;
			var array = default(double[]);
			var names = default((string, string, string));

			switch (channel.Type)
			{
				case ChannelType.Normal: names = ("LayerElementNormal", "Normals", "NormalsW"); break;
				case ChannelType.Tangent: names = ("LayerElementTangent", "Tangents", "TangentsW"); break;
				case ChannelType.Binormal: names = ("LayerElementBinormal", "Binormals", "BinormalsW"); break;
				case ChannelType.Color: names = ("LayerElementColor", "Colors", String.Empty); break;
				case ChannelType.TexCoord: names = ("LayerElementUV", "UV", String.Empty); break;
			}

			switch (channel.Size)
			{
				case ComponentType.Double: array = ElementaryFactory.DeepGenericCopy<double, double>(channel.Buffer as double[]); break;
				case ComponentType.Double2: array = ElementaryFactory.VtoDArray<Vector2, Vector2>(channel.Buffer as Vector2[]); break;
				case ComponentType.Double3: array = ElementaryFactory.VtoDArray<Vector3, Vector3>(channel.Buffer as Vector3[]); break;
				case ComponentType.Double4: array = ElementaryFactory.VtoDArray<Vector4, Vector3>(channel.Buffer as Vector4[]); break;
			}

			var elements = new IElement[5 + hasWs];

			elements[0] = Element.WithAttribute("Version", ElementaryFactory.GetElementAttribute(101));
			elements[1] = Element.WithAttribute("Name", ElementaryFactory.GetElementAttribute(channel.Name));
			elements[2] = Element.WithAttribute("MappingInformationType", ElementaryFactory.GetElementAttribute("ByPolygonVertex"));
			elements[3] = Element.WithAttribute("ReferenceInformationType", ElementaryFactory.GetElementAttribute("Direct"));
			elements[4] = Element.WithAttribute(names.Item2, ElementaryFactory.GetElementAttribute(array));

			if (channel.Size == ComponentType.Double4)
			{
				var buffer = channel.Buffer as Vector4[];
				var weight = new double[buffer.Length];

				for (int i = 0; i < weight.Length; ++i)
				{
					weight[i] = buffer[i].W;
				}

				elements[5] = Element.WithAttribute(names.Item3, ElementaryFactory.GetElementAttribute(weight));
			}

			var attributes = new IElementAttribute[]
			{
				ElementaryFactory.GetElementAttribute(channel.Layer),
			};

			return new Element(names.Item1, elements, attributes);
		}

		private IElement MateriaToElement()
		{
			int[] indices;
			string mapping;

			if (this.m_subMeshes.Length == 1)
			{
				mapping = "AllSame";
				indices = new int[] { this.m_subMeshes[0].MaterialIndex };
			}
			else
			{
				mapping = "ByPolygon";
				indices = new int[this.m_indices.Length];

				for (int i = 0; i < this.m_subMeshes.Length; ++i)
				{
					var subMesh = this.m_subMeshes[i];

					for (int k = 0; k < subMesh.PolygonCount; ++k)
					{
						indices[k + subMesh.PolygonStart] = subMesh.MaterialIndex;
					}
				}
			}

			var elements = new IElement[5];

			elements[0] = Element.WithAttribute("Version", ElementaryFactory.GetElementAttribute(101));
			elements[1] = Element.WithAttribute("Name", ElementaryFactory.GetElementAttribute(String.Empty));
			elements[2] = Element.WithAttribute("MappingInformationType", ElementaryFactory.GetElementAttribute(mapping));
			elements[3] = Element.WithAttribute("ReferenceInformationType", ElementaryFactory.GetElementAttribute("IndexToDirect"));
			elements[4] = Element.WithAttribute("Materials", ElementaryFactory.GetElementAttribute(indices));

			return new Element("LayerElementMaterial", elements, new IElementAttribute[]
			{
				ElementaryFactory.GetElementAttribute(0),
			});
		}

		public override IElement AsElement()
		{
			if (this.m_subMeshes.Length != 0)
			{
				this.m_channels.Add(new Channel(0, String.Empty, ChannelType.Material, ComponentType.Int, null));
			}

			var grouping = this.m_channels.GroupBy(_ => _.Layer).ToArray();
			var elements = new IElement[5 + this.m_channels.Count + grouping.Length];

			var vertexs = new double[this.m_vertices.Length * 3];
			var indices = new int[this.IndexCount];
			var edgearr = this.RecalculateEdges();

			for (int i = 0, k = 0; i < this.m_vertices.Length; ++i)
			{
				var vertex = this.m_vertices[i];

				vertexs[k++] = vertex.X;
				vertexs[k++] = vertex.Y;
				vertexs[k++] = vertex.Z;
			}

			for (int i = 0, k = 0; i < this.m_indices.Length; ++i)
			{
				var indexer = this.m_indices[i];
				var counter = indexer.Length - 1;
				
				if (counter < 0)
				{
					continue;
				}

				for (int j = 0; j < counter; ++j)
				{
					indices[k++] = indexer[j];
				}

				indices[k++] = ~indexer[counter];
			}

			elements[0] = this.BuildProperties70();
			elements[1] = Element.WithAttribute("Vertices", ElementaryFactory.GetElementAttribute(vertexs));
			elements[2] = Element.WithAttribute("PolygonVertexIndex", ElementaryFactory.GetElementAttribute(indices));
			elements[3] = Element.WithAttribute("Edges", ElementaryFactory.GetElementAttribute(edgearr));
			elements[4] = Element.WithAttribute("GeometryVersion", ElementaryFactory.GetElementAttribute(124));

			for (int l = 0, k = 5; l < grouping.Length; ++l)
			{
				var channels = grouping[l].ToArray();
				var elemento = new IElement[1 + channels.Length];

				elemento[0] = Element.WithAttribute("Version", ElementaryFactory.GetElementAttribute(100));

				for (int c = 0; c < channels.Length; ++c)
				{
					var channel = channels[c];
					var element = default(IElement);

					if (channel.Type == ChannelType.Material)
					{
						element = this.MateriaToElement();
					}
					else
					{
						element = this.ChannelToElement(channel);
					}

					elements[k++] = element;

					elemento[1 + c] = new Element("LayerElement", new IElement[]
					{
						Element.WithAttribute("Type", ElementaryFactory.GetElementAttribute(element.Name)),
						Element.WithAttribute("TypeIndex", ElementaryFactory.GetElementAttribute(channel.Layer)),
					}, null);
				}

				elements[5 + this.m_channels.Count + l] = new Element("Layer", elemento, new IElementAttribute[]
				{
					ElementaryFactory.GetElementAttribute(grouping[l].Key),
				});
			}

			if (this.m_subMeshes.Length != 0)
			{
				Geometry.MoveMaterials(elements, 5, this.m_channels.Count);
				this.m_channels.RemoveAt(this.m_channels.Count - 1);
			}

			return new Element("Geometry", elements, this.BuildAttributes("Mesh"));
		}
	}

	public class GeometryBuilder : BuilderBase
	{
		public enum PolyType
		{
			Tri = 3,
			Quad = 4,
		}

		private readonly List<Vector3> m_vertices;
		private readonly List<int[]> m_polygons;
		private readonly List<Geometry.Channel> m_channels;
		private readonly List<Geometry.SubMesh> m_subMeshes;

		public GeometryBuilder(Scene scene) : base(scene)
		{
			this.m_vertices = new List<Vector3>();
			this.m_polygons = new List<int[]>();
			this.m_channels = new List<Geometry.Channel>();
			this.m_subMeshes = new List<Geometry.SubMesh>();
		}

		public Geometry BuildGeometry()
		{
			var geometry = this.m_scene.CreateGeometry();

			geometry.Name = this.m_name;
			geometry.InternalSetVertices(this.m_vertices.ToArray());
			geometry.InternalSetIndices(this.m_polygons.ToArray());
			geometry.InternalSetSubMeshes(this.m_subMeshes.ToArray());

			foreach (var channel in this.m_channels)
			{
				geometry.InternalSetChannel(channel);
			}

			foreach (var property in this.m_properties)
			{
				geometry.AddProperty(property);
			}

			return geometry;
		}

		public GeometryBuilder WithName(string name)
		{
			this.SetObjectName(name);
			return this;
		}

		public GeometryBuilder WithFBXProperty<T>(string name, T value, bool isUser = false)
		{
			this.SetFBXProperty(name, value, isUser);
			return this;
		}
		public GeometryBuilder WithFBXProperty<T>(string name, T value, IElementPropertyFlags flags)
		{
			this.SetFBXProperty(name, value, flags);
			return this;
		}
		public GeometryBuilder WithFBXProperty<T>(FBXProperty<T> property)
		{
			this.SetFBXProperty(property);
			return this;
		}

		public GeometryBuilder WithVertex(in Vector3 vertex)
		{
			this.m_vertices.Add(vertex);
			return this;
		}
		public GeometryBuilder WithVertices(Vector3[] vertices)
		{
			this.m_vertices.AddRange(vertices ?? Array.Empty<Vector3>());
			return this;
		}

		public GeometryBuilder WithPolygon(int[] poly)
		{
			if (poly is null || poly.Length < 3)
			{
				throw new Exception("Poly was null or its index count is less than 3");
			}

			this.m_polygons.Add(poly);
			return this;
		}

		public GeometryBuilder WithIndices(int[] indices, PolyType type)
		{
			return this.WithIndices(indices, (int)type);
		}
		public GeometryBuilder WithIndices(int[] indices, int numPerPoly)
		{
			if (numPerPoly < 3)
			{
				throw new Exception($"Minimum number of indices per polygon should be 3");
			}

			var div = indices.Length / numPerPoly;
			var mod = indices.Length - numPerPoly * div;

			if (mod != 0)
			{
				throw new Exception($"Cannot equally divide index buffer given into {numPerPoly} polygons");
			}

			var polygons = new int[div][];

			for (int i = 0; i < div; ++i)
			{
				var poly = new int[numPerPoly];

				for (int k = 0; k < numPerPoly; ++k)
				{
					poly[k] = indices[i * numPerPoly + k];
				}

				polygons[i] = poly;
			}

			this.m_polygons.AddRange(polygons);
			return this;
		}

		public GeometryBuilder WithSubMesh(int polyStart, int polyCount, int matIndex = 0)
		{
			this.m_subMeshes.Add(new Geometry.SubMesh(polyStart, polyCount, matIndex));
			return this;
		}
		public GeometryBuilder WithSubMesh(in Geometry.SubMesh subMesh)
		{
			this.m_subMeshes.Add(subMesh);
			return this;
		}
	}
}
