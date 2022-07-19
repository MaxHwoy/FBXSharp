using FBXSharp.Core;
using System;
using System.Collections.Generic;

namespace FBXSharp.Objective
{
	public class BlendShapeChannel : FBXObject
	{
		private readonly List<Shape> m_shapes;
		private double[] m_fullWeights;

		public static readonly FBXObjectType FType = FBXObjectType.BlendShapeChannel;

		public static readonly FBXClassType FClass = FBXClassType.Deformer;

		public override FBXObjectType Type => BlendShapeChannel.FType;

		public override FBXClassType Class => BlendShapeChannel.FClass;

		public double DeformPercent { get; set; }

		public double[] FullWeights
		{
			get => this.m_fullWeights;
			set => this.m_fullWeights = value ?? Array.Empty<double>();
		}

		public IReadOnlyList<Shape> Shapes => this.m_shapes;

		internal BlendShapeChannel(IElement element, IScene scene) : base(element, scene)
		{
			this.m_shapes = new List<Shape>();
			this.m_fullWeights = Array.Empty<double>();

			if (element is null)
			{
				return;
			}

			var percent = element.FindChild("DeformPercent");

			if (!(percent is null) && percent.Attributes.Length > 0 && percent.Attributes[0].Type == IElementAttributeType.Double)
			{
				this.DeformPercent = Convert.ToDouble(percent.Attributes[0].GetElementValue());
			}

			var weights = element.FindChild("FullWeights");

			if (!(weights is null) && weights.Attributes.Length > 0 && weights.Attributes[0].Type == IElementAttributeType.ArrayDouble)
			{
				_ = ElementaryFactory.ToDoubleArray(weights.Attributes[0], out this.m_fullWeights);
			}
		}

		public void AddShape(Shape shape)
		{
			if (shape is null)
			{
				return;
			}

			if (shape.Scene != this.Scene)
			{
				throw new Exception("Shape should share same scene with blend shape channel");
			}

			this.m_shapes.Add(shape);
		}
		public void RemoveShape(Shape shape)
		{
			if (shape is null || shape.Scene != this.Scene)
			{
				return;
			}

			_ = this.m_shapes.Remove(shape);
		}
		public void AddShapeAt(Shape shape, int index)
		{
			if (shape is null)
			{
				return;
			}

			if (shape.Scene != this.Scene)
			{
				throw new Exception("Shape should share same scene with blend shape channel");
			}

			if (index < 0 || index > this.m_shapes.Count)
			{
				throw new ArgumentOutOfRangeException("Index should be in range 0 to shape count inclusively");
			}

			this.m_shapes.Insert(index, shape);
		}
		public void RemoveMaterialAt(int index)
		{
			if (index < 0 || index >= this.m_shapes.Count)
			{
				throw new ArgumentOutOfRangeException("Index should be in 0 to shape count range");
			}

			this.m_shapes.RemoveAt(index);
		}

		public override Connection[] GetConnections()
		{
			if (this.m_shapes.Count == 0)
			{
				return Array.Empty<Connection>();
			}
			else
			{
				int thisHashKey = this.GetHashCode();
				var connections = new Connection[this.m_shapes.Count];

				for (int i = 0; i < connections.Length; ++i)
				{
					connections[i] = new Connection(Connection.ConnectionType.Object, this.m_shapes[i].GetHashCode(), thisHashKey);
				}

				return connections;
			}
		}

		public override void ResolveLink(FBXObject linker, IElementAttribute attribute)
		{
			if (linker.Class == FBXClassType.Geometry && linker.Type == FBXObjectType.Shape)
			{
				this.AddShape(linker as Shape);
			}
		}

		public override IElement AsElement(bool binary)
		{
			var elements = new IElement[3];

			elements[0] = Element.WithAttribute("Version", ElementaryFactory.GetElementAttribute(100));
			elements[1] = Element.WithAttribute("DeformPercent", ElementaryFactory.GetElementAttribute(this.DeformPercent));
			elements[2] = Element.WithAttribute("FullWeights", ElementaryFactory.GetElementAttribute(this.m_fullWeights));

			return new Element(this.Class.ToString(), elements, this.BuildAttributes("SubDeformer", this.Type.ToString(), binary)); // #TODO
		}
	}
}
