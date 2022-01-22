using FBXSharp.Core;
using System;
using System.Diagnostics;

namespace FBXSharp.Elementary
{
	[DebuggerDisplay("Boolean : {this.Value.Length} Items")]
	public class ArrayBooleanAttribute : IGenericAttribute<bool[]>
	{
		public static readonly IElementAttributeType PropertyType = IElementAttributeType.ArrayBoolean;

		public static readonly int PropertyStride = 1;

		public static readonly int PropertyLength = -1;

		public IElementAttributeType Type => ArrayBooleanAttribute.PropertyType;

		public int Stride => ArrayBooleanAttribute.PropertyStride;

		public int Length { get; }

		public bool[] Value { get; }

		public ArrayBooleanAttribute(bool[] value)
		{
			this.Value = value ?? Array.Empty<bool>();
			this.Length = this.Value.Length;
		}

		public object GetElementValue() => this.Value;
	}
}
