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
}
