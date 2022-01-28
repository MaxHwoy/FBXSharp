using FBXSharp.Core;

namespace FBXSharp.Objective
{
	public class Shape : FBXObject
	{
		public static readonly FBXObjectType FType = FBXObjectType.Shape;

		public override FBXObjectType Type => Shape.FType;

		internal Shape(IElement element, IScene scene) : base(element, scene)
		{
		}

		//virtual const Vec3* getVertices() const = 0;
		//virtual int getVertexCount() const = 0;
		//virtual const Vec3* getNormals() const = 0;

		public override IElement AsElement() => throw new System.NotImplementedException();
	}
}
