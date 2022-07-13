using FBXSharp.Core;
using FBXSharp.ValueTypes;

namespace FBXSharp.Objective
{
	public class Cluster : FBXObject
	{
		public static readonly FBXObjectType FType = FBXObjectType.Cluster;

		public static readonly FBXClassType FClass = FBXClassType.Deformer;

		public override FBXObjectType Type => Cluster.FType;

		public override FBXClassType Class => Cluster.FClass;

		public Matrix4x4 Transform { get; set; }

		public Matrix4x4 TransformLink { get; set; }

		internal Cluster(IElement element, IScene scene) : base(element, scene)
		{
			if (element is null)
			{
				return;
			}

			var transformLink = element.FindChild("TransformLink");

			var transform = element.FindChild("Transform");

			// #TODO
		}

		//virtual const int* getIndices() const = 0;
		//virtual int getIndicesCount() const = 0;
		//virtual const double* getWeights() const = 0;
		//virtual int getWeightsCount() const = 0;
		//virtual Matrix getTransformMatrix() const = 0;
		//virtual Matrix getTransformLinkMatrix() const = 0;
		//virtual const Object* getLink() const = 0;

		public override IElement AsElement(bool binary)
		{
			return new Element(this.Class.ToString(), null, this.BuildAttributes("SubDeformer", this.Type.ToString(), binary)); // #TODO
		}
	}
}
