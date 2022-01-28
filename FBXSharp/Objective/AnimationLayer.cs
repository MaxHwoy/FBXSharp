using FBXSharp.Core;

namespace FBXSharp.Objective
{
	public class AnimationLayer : FBXObject
	{
		public static readonly FBXObjectType FType = FBXObjectType.AnimationLayer;

		public override FBXObjectType Type => AnimationLayer.FType;

		internal AnimationLayer(IElement element, IScene scene) : base(element, scene)
		{
		}

		public override IElement AsElement() => throw new System.NotImplementedException();
	}
}
