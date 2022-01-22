using FBXSharp.Core;

namespace FBXSharp.Objective
{
	public class LimbNode : NullNode
	{
		public static new readonly FBXObjectType FType = FBXObjectType.LimbNode;

		public override FBXObjectType Type => LimbNode.FType;

		internal LimbNode(IElement element, IScene scene) : base(element, scene)
		{
		}
	}
}
