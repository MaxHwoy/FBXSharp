using FBXSharp.Core;
using System.Diagnostics;

namespace FBXSharp.Elementary
{
	[DebuggerDisplay("{this.Value}")]
	public class BooleanAttribute : IGenericAttribute<bool>
	{
		public static readonly IElementAttributeType PropertyType = IElementAttributeType.Boolean;

		public static readonly int PropertyStride = 1;

		public static readonly int PropertyLength = 1;

		public IElementAttributeType Type => BooleanAttribute.PropertyType;

		public int Stride => BooleanAttribute.PropertyStride;

		public int Length => BooleanAttribute.PropertyLength;

		public bool Value { get; }

		public BooleanAttribute(bool value) => this.Value = value;

		public object GetElementValue() => this.Value;
	}
}
