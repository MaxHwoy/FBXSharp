using FBXSharp.Core;

namespace FBXSharp.Objective
{
	public class Root : NullNode
	{
		public static new readonly FBXObjectType FType = FBXObjectType.Root;

		public override FBXObjectType Type => Root.FType;

		internal Root(IScene scene) : base(null, scene)
		{
			this.Name = "Root";
		}
	}
}
