using FBXSharp.Core;

namespace FBXSharp.Objective
{
	public class Pose : FBXObject
	{
		public static readonly FBXObjectType FType = FBXObjectType.Pose;

		public override FBXObjectType Type => Pose.FType;

		internal Pose(IElement element, IScene scene) : base(element, scene)
		{
			var poseNode = element.FindChild("PoseNode");

			if (poseNode is null)
			{
				return;
			}

			var node = poseNode.FindChild("Node");
			var matrix = poseNode.FindChild("Matrix");

			// #TODO
		}

		//virtual Matrix getMatrix() const = 0;
		//virtual const Object* getNode() const = 0;

		public override IElement AsElement() => throw new System.NotImplementedException();
	}
}
