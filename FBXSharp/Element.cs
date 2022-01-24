using FBXSharp.Core;
using System;
using System.Diagnostics;

namespace FBXSharp
{
	[DebuggerDisplay("{this.Name}")]
	public class Element : IElement
	{
		public string Name { get; }
		public IElement[] Children { get; }
		public IElementAttribute[] Attributes { get; }

		public Element(string name, IElement[] children, IElementAttribute[] attributes)
		{
			this.Name = name ?? String.Empty;
			this.Children = children ?? Array.Empty<IElement>();
			this.Attributes = attributes ?? Array.Empty<IElementAttribute>();
		}

		public IElement FindChild(string name) => Array.Find(this.Children, _ => _.Name == name);

		public static Element WithAttribute(string name, IElementAttribute attribute)
		{
			return new Element(name, null, new IElementAttribute[] { attribute });
		}
	}
}
