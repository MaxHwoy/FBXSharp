using FBXSharp.Core;

namespace FBXSharp.Objective
{
	public class AnimationCurveNode : FBXObject
	{
		public static readonly FBXObjectType FType = FBXObjectType.AnimationCurveNode;

		public override FBXObjectType Type => AnimationCurveNode.FType;

		internal AnimationCurveNode(IElement element, IScene scene) : base(element, scene)
		{
			// #TODO
		}

		public override IElement AsElement() => throw new System.NotImplementedException();
	}
}
