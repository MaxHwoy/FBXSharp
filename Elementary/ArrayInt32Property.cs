using FBXSharp.Core;
using System;
using System.Diagnostics;

namespace FBXSharp.Elementary
{
	[DebuggerDisplay("Int32 : {this.Value.Length} Items")]
	public class ArrayInt32Attribute : IGenericAttribute<int[]>
	{
		public static readonly IElementAttributeType PropertyType = IElementAttributeType.ArrayInt32;

		public static readonly int PropertyStride = 4;

		public static readonly int PropertyLength = -1;

		public IElementAttributeType Type => ArrayInt32Attribute.PropertyType;

		public int Stride => ArrayInt32Attribute.PropertyStride;

		public int Length { get; }

		public int[] Value { get; }

		public ArrayInt32Attribute(int[] value)
		{
			this.Value = value ?? Array.Empty<int>();
			this.Length = this.Value.Length;
		}

		public object GetElementValue() => this.Value;
	}
}
