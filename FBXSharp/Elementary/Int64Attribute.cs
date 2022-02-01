using FBXSharp.Core;
using System.Diagnostics;

namespace FBXSharp.Elementary
{
	[DebuggerDisplay("{this.Value}L")]
	public class Int64Attribute : IGenericAttribute<long>
	{
		public static readonly IElementAttributeType PropertyType = IElementAttributeType.Int64;

		public static readonly int PropertyStride = 8;

		public static readonly int PropertyLength = 1;

		public IElementAttributeType Type => Int64Attribute.PropertyType;

		public int Size => Int64Attribute.PropertyStride;

		public int Stride => Int64Attribute.PropertyStride;

		public int Length => Int64Attribute.PropertyLength;

		public long Value { get; }

		public Int64Attribute(long value) => this.Value = value;

		public object GetElementValue() => this.Value;
	}
}
