using FBXSharp.Core;
using FBXSharp.ValueTypes;
using System;
using System.Collections.Generic;

namespace FBXSharp.Objective
{
	public class Shape : FBXObject
	{
		private int[] m_indices;
		private Vector3[] m_vertices;
		private Vector3[] m_normals;

		public static readonly FBXObjectType FType = FBXObjectType.Shape;

		public static readonly FBXClassType FClass = FBXClassType.Geometry;

		public override FBXObjectType Type => Shape.FType;

		public override FBXClassType Class => Shape.FClass;

		public IReadOnlyList<int> Indices => this.m_indices;

		public IReadOnlyList<Vector3> Vertices => this.m_vertices;

		public IReadOnlyList<Vector3> Normals => this.m_normals;

		internal Shape(IElement element, IScene scene) : base(element, scene)
		{
			if (element is null)
			{
				return;
			}

			var indices = element.FindChild("Indexes");

			if (!(indices is null) && indices.Attributes.Length > 0 && indices.Attributes[0].Type == IElementAttributeType.ArrayInt32)
			{
				if (!ElementaryFactory.ToInt32Array(indices.Attributes[0], out this.m_indices))
				{
					this.m_indices = Array.Empty<int>();
				}
			}

			var vertices = element.FindChild("Vertices");

			if (!(vertices is null) && vertices.Attributes.Length > 0 && vertices.Attributes[0].Type == IElementAttributeType.ArrayDouble)
			{
				if (!ElementaryFactory.ToVector3Array(vertices.Attributes[0], out this.m_vertices))
				{
					this.m_vertices = Array.Empty<Vector3>();
				}
			}

			var normals = element.FindChild("Normals");

			if (!(normals is null) && normals.Attributes.Length > 0 && normals.Attributes[0].Type == IElementAttributeType.ArrayDouble)
			{
				if (!ElementaryFactory.ToVector3Array(normals.Attributes[0], out this.m_normals))
				{
					this.m_normals = Array.Empty<Vector3>();
				}
			}
		}

		public void SetupMorphTarget(int[] indices, Vector3[] vertices, Vector3[] normals)
		{
			if (indices is null || vertices is null || normals is null)
			{
				return;
			}

			if (indices.Length != vertices.Length || indices.Length != vertices.Length)
			{
				return;
			}

			this.m_indices = indices;
			this.m_vertices = vertices;
			this.m_normals = normals;
		}

		public override IElement AsElement(bool binary)
		{
			return new Element(this.Class.ToString(), null, this.BuildAttributes("Geometry", this.Type.ToString(), binary)); // #TODO
		}
	}
}
