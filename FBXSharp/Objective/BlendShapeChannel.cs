using FBXSharp.Core;

namespace FBXSharp.Objective
{
	public class BlendShapeChannel : FBXObject
	{
		public static readonly FBXObjectType FType = FBXObjectType.BlendShapeChannel;

		public static readonly FBXClassType FClass = FBXClassType.Deformer;

		public override FBXObjectType Type => BlendShapeChannel.FType;

		public override FBXClassType Class => BlendShapeChannel.FClass;

		internal BlendShapeChannel(IElement element, IScene scene) : base(element, scene)
		{
			// #TODO
		}

		public override IElement AsElement(bool binary)
		{
			return new Element(this.Class.ToString(), null, this.BuildAttributes("SubDeformer", this.Type.ToString(), binary)); // #TODO
		}
	}
}
