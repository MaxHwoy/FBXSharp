using FBXSharp.Core;
using System;

namespace FBXSharp.Objective
{
	public class BindPose : FBXObject
	{
		

		public static readonly FBXObjectType FType = FBXObjectType.BindPose;

		public static readonly FBXClassType FClass = FBXClassType.Pose;

		public override FBXObjectType Type => BindPose.FType;

		public override FBXClassType Class => BindPose.FClass;

		internal BindPose(IElement element, IScene scene) : base(element, scene)
		{
			var nbPoseNodes = element.FindChild("NbPoseNodes");

			if (nbPoseNodes is null || nbPoseNodes.Attributes.Length == 0)
			{
				return;
			}

			var numPoseNodes = Convert.ToInt32(nbPoseNodes.Attributes[0].GetElementValue());

			if (numPoseNodes != 0)
			{
				// initialize nodes here

				for (int i = 0, k = 0; i < element.Children.Length && k < numPoseNodes; ++i)
				{
					var child = element.Children[i];

					if (child.Name != "PoseNode")
					{
						continue;
					}

					var node = child.FindChild("Node");
					var matrix = child.FindChild("Matrix");

					if (node is null || matrix is null)
					{

					}
					else
					{

					}

					++k;
				}
			}
		}

		//virtual Matrix getMatrix() const = 0;
		//virtual const Object* getNode() const = 0;

		public override IElement AsElement(bool binary)
		{
			return new Element(this.Class.ToString(), null, this.BuildAttributes("Pose", this.Type.ToString(), binary)); // #TODO
		}
	}
}
