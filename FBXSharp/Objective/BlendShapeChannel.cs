using FBXSharp.Core;

namespace FBXSharp.Objective
{
	public class BlendShapeChannel : FBXObject
	{
		public static readonly FBXObjectType FType = FBXObjectType.BlendShapeChannel;

		public override FBXObjectType Type => BlendShapeChannel.FType;

		internal BlendShapeChannel(IElement element, IScene scene) : base(element, scene)
		{
		}

		public override IElement AsElement() => throw new System.NotImplementedException();
	}
}
