using FBXSharp.Core;
using System.Diagnostics;

namespace FBXSharp.Elementary
{
	[DebuggerDisplay("{this.Value}")]
	public class Int32Attribute : IGenericAttribute<int>
	{
		public static readonly IElementAttributeType PropertyType = IElementAttributeType.Int32;

		public static readonly int PropertyStride = 4;

		public static readonly int PropertyLength = 1;

		public IElementAttributeType Type => Int32Attribute.PropertyType;

		public int Size => Int32Attribute.PropertyStride;

		public int Stride => Int32Attribute.PropertyStride;

		public int Length => Int32Attribute.PropertyLength;

		public int Value { get; }

		public Int32Attribute(int value) => this.Value = value;

		public object GetElementValue() => this.Value;
	}
}
