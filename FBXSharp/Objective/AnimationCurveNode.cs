using FBXSharp.Core;
using System;

namespace FBXSharp.Objective
{
	public class AnimationCurveNode : FBXObject
	{
		public static readonly FBXObjectType FType = FBXObjectType.AnimationCurveNode;

		public static readonly FBXClassType FClass = FBXClassType.AnimationCurveNode;

		public override FBXObjectType Type => AnimationCurveNode.FType;

		public override FBXClassType Class => AnimationCurveNode.FClass;

		internal AnimationCurveNode(IElement element, IScene scene) : base(element, scene)
		{
			// #TODO
		}

		public override IElement AsElement(bool binary)
		{
			return new Element(this.Class.ToString(), null, this.BuildAttributes("AnimCurveNode", String.Empty, binary)); // #TODO
		}
	}
}
