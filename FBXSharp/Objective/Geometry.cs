using FBXSharp.Core;
using FBXSharp.ValueTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
			public Array Buffer { get; }
			public ChannelType Type { get; }
			public ComponentType Size { get; }

			public Channel(int layer, ChannelType type, ComponentType size, Array array)
			{
				this.Layer = layer;
				this.Type = type;
				this.Size = size;
				this.Buffer = array;
			}

			public override string ToString() => $"{this.Type} {this.Layer}";
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
			this.m_channels = new List<Channel>();
			this.m_readonly = new ReadOnlyCollection<Channel>(this.m_channels);
			this.m_subMeshes = Array.Empty<SubMesh>();
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

		public GeometryBuilder(IScene scene) : base(scene)
		{
			this.m_vertices = new List<Vector3>();
			this.m_polygons = new List<int[]>();
			this.m_channels = new List<Geometry.Channel>();
			this.m_subMeshes = new List<Geometry.SubMesh>();
		}

		public Geometry BuildGeometry()
		{
			var geometry = new Geometry(null, this.m_scene)
			{
				Name = this.m_name,
			};

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

			geometry.SetNewID(geometry.GetHashCode());

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
