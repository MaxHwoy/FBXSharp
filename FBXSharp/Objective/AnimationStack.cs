using FBXSharp.Core;
using System;

namespace FBXSharp.Objective
{
	public class AnimationStack : FBXObject
	{
		public static readonly FBXObjectType FType = FBXObjectType.AnimationStack;

		public static readonly FBXClassType FClass = FBXClassType.AnimationStack;

		public override FBXObjectType Type => AnimationStack.FType;

		public override FBXClassType Class => AnimationStack.FClass;

		internal AnimationStack(IElement element, IScene scene) : base(element, scene)
		{
			// #TODO
		}

		public override IElement AsElement(bool binary)
		{
			return new Element(this.Class.ToString(), null, this.BuildAttributes("AnimStack", String.Empty, binary)); // #TODO
		}
	}
}
