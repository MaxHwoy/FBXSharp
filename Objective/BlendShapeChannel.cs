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

		//virtual double getDeformPercent() const = 0;
		//virtual int getShapeCount() const = 0;
		//virtual const struct Shape* getShape(int idx) const = 0;
	}
}
