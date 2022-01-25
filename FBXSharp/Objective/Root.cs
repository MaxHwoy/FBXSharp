using FBXSharp.Core;
using System;

namespace FBXSharp.Objective
{
	public class Root : Model
	{
		public static new readonly FBXObjectType FType = FBXObjectType.Root;

		public override FBXObjectType Type => Root.FType;

		public override bool SupportsAttribute => false;

		public override NodeAttribute Attribute
		{
			get => throw new NotSupportedException("Roots do not support node attributes");
			set => throw new NotSupportedException("Roots do not support node attributes");
		}

		internal Root(IScene scene) : base(null, scene)
		{
			this.Name = "RootNode";
		}
	}
}
