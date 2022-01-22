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

		//virtual const AnimationCurveNode* getCurveNode(int index) const = 0;
		//virtual const AnimationCurveNode* getCurveNode(const Object& bone, const char* property) const = 0;
	}
}
