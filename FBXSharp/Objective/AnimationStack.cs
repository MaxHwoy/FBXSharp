using FBXSharp.Core;

namespace FBXSharp.Objective
{
	public class AnimationStack : FBXObject
	{
		public static readonly FBXObjectType FType = FBXObjectType.AnimationStack;

		public override FBXObjectType Type => AnimationStack.FType;

		internal AnimationStack(IElement element, IScene scene) : base(element, scene)
		{
		}

		public override IElement AsElement() => throw new System.NotImplementedException();
	}
}
