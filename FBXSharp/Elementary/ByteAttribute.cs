using FBXSharp.Core;
using System.Diagnostics;

namespace FBXSharp.Elementary
{
	[DebuggerDisplay("{this.Value}B")]
	public class ByteAttribute : IGenericAttribute<byte>
	{
		public static readonly IElementAttributeType PropertyType = IElementAttributeType.Byte;

		public static readonly int PropertyStride = 1;

		public static readonly int PropertyLength = 1;

		public IElementAttributeType Type => ByteAttribute.PropertyType;

		public int Size => ByteAttribute.PropertyStride;

		public int Stride => ByteAttribute.PropertyStride;

		public int Length => ByteAttribute.PropertyLength;

		public byte Value { get; }

		public ByteAttribute(byte value) => this.Value = value;

		public object GetElementValue() => this.Value;
	}
}
