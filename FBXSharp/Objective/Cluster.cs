using FBXSharp.Core;
using FBXSharp.ValueTypes;
using System;

namespace FBXSharp.Objective
{
	public class Cluster : FBXObject
	{
		private double[] m_weights;
		private int[] m_indices;
		private Model m_link;

		public static readonly FBXObjectType FType = FBXObjectType.Cluster;

		public static readonly FBXClassType FClass = FBXClassType.Deformer;

		public override FBXObjectType Type => Cluster.FType;

		public override FBXClassType Class => Cluster.FClass;

		public int[] Indices => this.m_indices;

		public double[] Weights => this.m_weights;

		public Matrix4x4 Transform { get; set; }

		public Matrix4x4 TransformLink { get; set; }

		public Matrix4x4 TransformAssociateModel { get; set; }

		public Model Link
		{
			get => this.m_link;
			set => this.InternalSetLink(value);
		}

		internal Cluster(IElement element, IScene scene) : base(element, scene)
		{
			this.m_indices = Array.Empty<int>();
			this.m_weights = Array.Empty<double>();
			this.Transform = Matrix4x4.Identity;
			this.TransformLink = Matrix4x4.Identity;
			this.TransformAssociateModel = Matrix4x4.Identity;

			if (element is null)
			{
				return;
			}

			var indexes = element.FindChild("Indexes");

			if (!(indexes is null) && indexes.Attributes.Length > 0 && indexes.Attributes[0].Type == IElementAttributeType.ArrayInt32)
			{
				_ = ElementaryFactory.ToInt32Array(indexes.Attributes[0], out this.m_indices);
			}

			var weights = element.FindChild("Weights");

			if (!(weights is null) && weights.Attributes.Length > 0 && weights.Attributes[0].Type == IElementAttributeType.ArrayDouble)
			{
				_ = ElementaryFactory.ToDoubleArray(weights.Attributes[0], out this.m_weights);
			}

			var transform = element.FindChild("Transform");

			if (!(transform is null) && transform.Attributes.Length > 0 && transform.Attributes[0].Type == IElementAttributeType.ArrayDouble)
			{
				if (ElementaryFactory.ToMatrix4x4(transform.Attributes[0], out var matrix))
				{
					this.Transform = matrix;
				}
			}

			var transformLink = element.FindChild("TransformLink");

			if (!(transformLink is null) && transformLink.Attributes.Length > 0 && transformLink.Attributes[0].Type == IElementAttributeType.ArrayDouble)
			{
				if (ElementaryFactory.ToMatrix4x4(transformLink.Attributes[0], out var matrix))
				{
					this.TransformLink = matrix;
				}
			}

			var transformAssociateModel = element.FindChild("TransformAssociateModel");

			if (!(transformAssociateModel is null) && transformAssociateModel.Attributes.Length > 0 && transformAssociateModel.Attributes[0].Type == IElementAttributeType.ArrayDouble)
			{
				if (ElementaryFactory.ToMatrix4x4(transformAssociateModel.Attributes[0], out var matrix))
				{
					this.TransformAssociateModel = matrix;
				}
			}
		}

		private void InternalSetLink(Model model)
		{
			if (model is null)
			{
				this.m_link = null;
				return;
			}

			if (model.Scene != this.Scene)
			{
				throw new Exception("Model should share same scene with cluster");
			}

			if (model.Type != FBXObjectType.Null && model.Type != FBXObjectType.LimbNode && model.Type != FBXObjectType.Mesh)
			{
				throw new Exception("Model linked should be either null node, limb node, or mesh node");
			}

			this.m_link = model;
		}

		public void SetupBlendWeights(int[] indices, double[] weights)
		{
			if (indices is null && weights is null)
			{
				this.m_indices = Array.Empty<int>();
				this.m_weights = Array.Empty<double>();

				return;
			}

			if (indices.Length != weights.Length)
			{
				throw new Exception("Indices and weights arrays should all have the same length");
			}

			this.m_indices = indices;
			this.m_weights = weights;
		}

		public override Connection[] GetConnections()
		{
			if (this.m_link is null)
			{
				return Array.Empty<Connection>();
			}
			else
			{
				return new Connection[]
				{
					new Connection(Connection.ConnectionType.Object, this.m_link.GetHashCode(), this.GetHashCode()),
				};
			}
		}

		public override void ResolveLink(FBXObject linker, IElementAttribute attribute)
		{
			if (linker.Class == FBXClassType.Model)
			{
				this.InternalSetLink(linker as Model);
			}
		}

		public override IElement AsElement(bool binary)
		{
			bool hasAnyProperties = this.Properties.Count != 0;

			var elements = new IElement[7 + (hasAnyProperties ? 1 : 0)];

			elements[0] = Element.WithAttribute("Version", ElementaryFactory.GetElementAttribute(100));
			elements[2] = Element.WithAttribute("Indexes", ElementaryFactory.GetElementAttribute(this.m_indices));
			elements[3] = Element.WithAttribute("Weights", ElementaryFactory.GetElementAttribute(this.m_weights));
			elements[4] = Element.WithAttribute("Transform", ElementaryFactory.GetElementAttribute(this.Transform));
			elements[5] = Element.WithAttribute("TransformLink", ElementaryFactory.GetElementAttribute(this.TransformLink));
			elements[6] = Element.WithAttribute("TransformAssociateModel", ElementaryFactory.GetElementAttribute(this.TransformAssociateModel));

			elements[1] = new Element("UserData", null, new IElementAttribute[]
			{
				ElementaryFactory.GetElementAttribute(String.Empty),
				ElementaryFactory.GetElementAttribute(String.Empty),
			});

			if (hasAnyProperties)
			{
				elements[7] = this.BuildProperties70();
			}

			return new Element(this.Class.ToString(), elements, this.BuildAttributes("SubDeformer", this.Type.ToString(), binary));
		}
	}
}
