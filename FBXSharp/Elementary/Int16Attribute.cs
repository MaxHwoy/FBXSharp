using FBXSharp.Core;
using System.Diagnostics;

namespace FBXSharp.Elementary
{
	[DebuggerDisplay("{this.Value}S")]
	public class Int16Attribute : IGenericAttribute<short>
	{
		public static readonly IElementAttributeType PropertyType = IElementAttributeType.Int16;

		public static readonly int PropertyStride = 2;

		public static readonly int PropertyLength = 1;

		public IElementAttributeType Type => Int16Attribute.PropertyType;

		public int Stride => Int16Attribute.PropertyStride;

		public int Length => Int16Attribute.PropertyLength;

		public short Value { get; }

		public Int16Attribute(short value) => this.Value = value;

		public object GetElementValue() => this.Value;
	}
}
