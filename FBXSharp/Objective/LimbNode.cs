using FBXSharp.Core;
using System;

namespace FBXSharp.Objective
{
	public class LimbNode : Model
	{
		public static readonly FBXObjectType FType = FBXObjectType.LimbNode;

		public override FBXObjectType Type => LimbNode.FType;

		public override bool SupportsAttribute => true;

		internal LimbNode(IElement element, IScene scene) : base(element, scene)
		{
		}

		public override IElement AsElement(bool binary)
		{
			return this.MakeElement("Model", binary);
		}
	}

	public class LimbNodeAttribute : NodeAttribute
	{
		private string m_typeFlags;

		public static readonly FBXObjectType FType = FBXObjectType.LimbNode;

		public override FBXObjectType Type => LimbNodeAttribute.FType;

		public string TypeFlags
		{
			get => this.m_typeFlags;
			set => this.m_typeFlags = value ?? String.Empty;
		}

		internal LimbNodeAttribute(IElement element, IScene scene) : base(element, scene)
		{
			if (element is null)
			{
				return;
			}

			var typeFlags = element.FindChild("TypeFlags");

			if (typeFlags is null || typeFlags.Attributes.Length == 0 || typeFlags.Attributes[0].Type != IElementAttributeType.String)
			{
				this.m_typeFlags = String.Empty;
			}
			else
			{
				this.m_typeFlags = typeFlags.Attributes[0].GetElementValue().ToString();
			}
		}

		public override IElement AsElement(bool binary)
		{
			bool hasAnyProperties = this.Properties.Count != 0;

			var elements = new IElement[1 + (hasAnyProperties ? 1 : 0)];

			elements[0] = Element.WithAttribute("TypeFlags", ElementaryFactory.GetElementAttribute(this.m_typeFlags));
			
			if (hasAnyProperties)
			{
				elements[1] = this.BuildProperties70();
			}

			return new Element(this.Class.ToString(), elements, this.BuildAttributes("NodeAttribute", this.Type.ToString(), binary));
		}
	}
}
