using FBXSharp.Core;

namespace FBXSharp.Objective
{
	public class BlendShape : FBXObject
	{
		public static readonly FBXObjectType FType = FBXObjectType.BlendShape;

		public override FBXObjectType Type => BlendShape.FType;

		internal BlendShape(IElement element, IScene scene) : base(element, scene)
		{
		}

		public override IElement AsElement() => throw new System.NotImplementedException();
	}
}
