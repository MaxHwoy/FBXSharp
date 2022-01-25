using FBXSharp.Core;
using System;

namespace FBXSharp
{
	public class TemplateObject : FBXObject
	{
		public static readonly FBXObjectType FType = FBXObjectType.Template;

		public override FBXObjectType Type => TemplateObject.FType;

		public FBXObjectType OverridableType { get; }

		internal TemplateObject(FBXObjectType type, IElement element, IScene scene) : base(null, scene)
		{
			this.OverridableType = type;

			if (element is null)
			{
				return;
			}

			if (element.Attributes.Length > 0 && element.Attributes[0].Type == IElementAttributeType.String)
			{
				this.Name = element.Attributes[0].GetElementValue().ToString();
			}

			this.ParseProperties70(element);
		}

		public void MergeWith(TemplateObject template)
		{
			if (template is null)
			{
				throw new ArgumentNullException("Merger template passed cannot be null");
			}

			if (template.OverridableType != this.OverridableType)
			{
				throw new ArgumentException($"Cannot merge template of type {this.OverridableType} with template of type {template.OverridableType}");
			}

			foreach (var property in template.Properties)
			{
				this.AddProperty(property);
			}
		}
	}
}
