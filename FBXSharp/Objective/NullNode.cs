using FBXSharp.Core;
using System;

namespace FBXSharp.Objective
{
	public class NullNode : Model
	{
		private NullAttribute m_attribute;

		public override bool SupportsAttribute => true;

		public override NodeAttribute Attribute
		{
			get => this.m_attribute;
			set => this.SetNodeAttribute(value);
		}

		internal NullNode(IElement element, IScene scene) : base(element, scene)
		{
		}

		private void SetNodeAttribute(NodeAttribute attribute)
		{
			if (attribute is NullAttribute @null)
			{
				this.m_attribute = @null;
			}
			else
			{
				throw new ArgumentException("Node Attribute passed should be of NullAttribute type");
			}
		}
	}

	public class NullAttribute : NodeAttribute
	{
		private string m_flags;

		public string Flags
		{
			get => this.m_flags;
			set => this.m_flags = String.IsNullOrWhiteSpace(value) ? "Null" : value;
		}

		internal NullAttribute(IElement element, IScene scene) : base(element, scene)
		{
			this.m_flags = "Null";

			var flags = element.FindChild("TypeFlags");

			if (!(flags is null) && flags.Attributes.Length > 0)
			{
				this.Flags = flags.Attributes[0].GetElementValue().ToString();
			}
		}
	}
}
