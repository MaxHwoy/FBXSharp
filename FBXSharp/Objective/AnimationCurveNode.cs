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

		//virtual const AnimationCurve* getCurve(int idx) const = 0;
		//virtual Vec3 getNodeLocalTransform(double time) const = 0;
		//virtual const Object* getBone() const = 0;
	}
}
