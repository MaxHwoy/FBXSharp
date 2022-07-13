using FBXSharp.Core;
using System;

namespace FBXSharp.Objective
{
	public class AnimationLayer : FBXObject
	{
		public static readonly FBXObjectType FType = FBXObjectType.AnimationLayer;

		public static readonly FBXClassType FClass = FBXClassType.AnimationLayer;

		public override FBXObjectType Type => AnimationLayer.FType;

		public override FBXClassType Class => AnimationLayer.FClass;

		internal AnimationLayer(IElement element, IScene scene) : base(element, scene)
		{
			// #TODO
		}

		public override IElement AsElement(bool binary)
		{
			return new Element(this.Class.ToString(), null, this.BuildAttributes("AnimLayer", String.Empty, binary));
		}
	}
}
