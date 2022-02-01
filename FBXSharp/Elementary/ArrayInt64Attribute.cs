using FBXSharp.Core;
using System;
using System.Diagnostics;

namespace FBXSharp.Elementary
{
	[DebuggerDisplay("Int64 : {this.Value.Length} Items")]
	public class ArrayInt64Attribute : IGenericAttribute<long[]>
	{
		public static readonly IElementAttributeType PropertyType = IElementAttributeType.ArrayInt64;

		public static readonly int PropertyStride = 8;

		public static readonly int PropertyLength = -1;

		public IElementAttributeType Type => ArrayInt64Attribute.PropertyType;

		public int Size => 12 + ArrayInt64Attribute.PropertyStride * this.Length;

		public int Stride => ArrayInt64Attribute.PropertyStride;

		public int Length { get; }

		public long[] Value { get; }

		public ArrayInt64Attribute(long[] value)
		{
			this.Value = value ?? Array.Empty<long>();
			this.Length = this.Value.Length;
		}

		public object GetElementValue() => this.Value;
	}
}
