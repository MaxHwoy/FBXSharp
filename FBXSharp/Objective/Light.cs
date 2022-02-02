using FBXSharp.Core;
using System;

namespace FBXSharp.Objective
{
	public class Light : Model
	{
		private LightAttribute m_attribute;

		public override bool SupportsAttribute => true;

		public override NodeAttribute Attribute
		{
			get => this.m_attribute;
			set => this.SetNodeAttribute(value);
		}

		internal Light(IElement element, IScene scene) : base(element, scene)
		{
		}

		private void SetNodeAttribute(NodeAttribute attribute)
		{
			if (attribute is LightAttribute light)
			{
				this.m_attribute = light;
			}
			else
			{
				throw new ArgumentException("Node Attribute passed should be of LightAttribute type");
			}
		}

		public override IElement AsElement(bool binary)
		{
			return this.MakeElement("Light", binary);
		}
	}

	public class LightAttribute : NodeAttribute
	{
		internal LightAttribute(IElement element, IScene scene) : base(element, scene)
		{
		}

		public override IElement AsElement(bool binary)
		{
			var elements = new IElement[3];

			elements[0] = Element.WithAttribute("TypeFlags", ElementaryFactory.GetElementAttribute("Light"));
			elements[1] = Element.WithAttribute("GeometryVersion", ElementaryFactory.GetElementAttribute(124));
			elements[2] = this.BuildProperties70();

			return new Element("NodeAttribute", elements, this.BuildAttributes("Light", binary));
		}
	}
}
