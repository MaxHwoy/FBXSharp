using FBXSharp.Core;
using System;

namespace FBXSharp.Objective
{
	public class LimbNode : Model
	{
		public override bool SupportsAttribute => false;

		public override NodeAttribute Attribute
		{
			get => throw new NotSupportedException("Limb Nodes do not support node attributes");
			set => throw new NotSupportedException("Limb Nodes do not support node attributes");
		}

		internal LimbNode(IElement element, IScene scene) : base(element, scene)
		{
		}

		public override IElement AsElement() => throw new NotImplementedException();
	}
}
