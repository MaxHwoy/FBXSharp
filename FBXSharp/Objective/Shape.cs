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
			this.m_indices = Array.Empty<int>();
			this.m_vertices = Array.Empty<Vector3>();
			this.m_normals = Array.Empty<Vector3>();

			if (element is null)
			{
				return;
			}

			var indices = element.FindChild("Indexes");

			if (!(indices is null) && indices.Attributes.Length > 0 && indices.Attributes[0].Type == IElementAttributeType.ArrayInt32)
			{
				_ = ElementaryFactory.ToInt32Array(indices.Attributes[0], out this.m_indices);
			}

			var vertexs = element.FindChild("Vertices");

			if (!(vertexs is null) && vertexs.Attributes.Length > 0 && vertexs.Attributes[0].Type == IElementAttributeType.ArrayDouble)
			{
				_ = ElementaryFactory.ToVector3Array(vertexs.Attributes[0], out this.m_vertices);
			}

			var normals = element.FindChild("Normals");

			if (!(normals is null) && normals.Attributes.Length > 0 && normals.Attributes[0].Type == IElementAttributeType.ArrayDouble)
			{
				_ = ElementaryFactory.ToVector3Array(normals.Attributes[0], out this.m_normals);
			}
		}

		public void SetupMorphTarget(int[] indices, Vector3[] vertices, Vector3[] normals)
		{
			if (indices is null || vertices is null || normals is null)
			{
				this.m_indices = Array.Empty<int>();
				this.m_normals = Array.Empty<Vector3>();
				this.m_vertices = Array.Empty<Vector3>();

				return;
			}

			if (indices.Length != vertices.Length || indices.Length != vertices.Length)
			{
				throw new Exception("Indices, vertices, and normals arrays should all have the same length");
			}

			this.m_indices = indices;
			this.m_vertices = vertices;
			this.m_normals = normals;
		}

		public override IElement AsElement(bool binary)
		{
			var elements = new IElement[5];

			var indexes = new int[this.m_indices.Length];
			var vertexs = new double[this.m_vertices.Length * 3];
			var normals = new double[this.m_normals.Length * 3];

			Array.Copy(this.m_indices, indexes, indexes.Length);

			for (int i = 0, k = 0; i < this.m_vertices.Length; ++i)
			{
				var vector = this.m_vertices[i];

				vertexs[k++] = vector.X;
				vertexs[k++] = vector.Y;
				vertexs[k++] = vector.Z;
			}

			for (int i = 0, k = 0; i < this.m_normals.Length; ++i)
			{
				var vector = this.m_normals[i];

				normals[k++] = vector.X;
				normals[k++] = vector.Y;
				normals[k++] = vector.Z;
			}

			elements[0] = this.BuildProperties70();
			elements[1] = Element.WithAttribute("Version", ElementaryFactory.GetElementAttribute(100));
			elements[2] = Element.WithAttribute("Indexes", ElementaryFactory.GetElementAttribute(indexes));
			elements[3] = Element.WithAttribute("Vertices", ElementaryFactory.GetElementAttribute(vertexs));
			elements[4] = Element.WithAttribute("Normals", ElementaryFactory.GetElementAttribute(normals));

			return new Element(this.Class.ToString(), elements, this.BuildAttributes("Geometry", this.Type.ToString(), binary));
		}
	}
}
