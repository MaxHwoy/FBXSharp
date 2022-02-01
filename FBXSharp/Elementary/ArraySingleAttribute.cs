using FBXSharp.Core;
using System;
using System.Diagnostics;

namespace FBXSharp.Elementary
{
	[DebuggerDisplay("Single : {this.Value.Length} Items")]
	public class ArraySingleAttribute : IGenericAttribute<float[]>
	{
		public static readonly IElementAttributeType PropertyType = IElementAttributeType.ArraySingle;

		public static readonly int PropertyStride = 4;

		public static readonly int PropertyLength = -1;

		public IElementAttributeType Type => ArraySingleAttribute.PropertyType;

		public int Size => 12 + ArraySingleAttribute.PropertyStride * this.Length;

		public int Stride => ArraySingleAttribute.PropertyStride;

		public int Length { get; }

		public float[] Value { get; }

		public ArraySingleAttribute(float[] value)
		{
			this.Value = value ?? Array.Empty<float>();
			this.Length = this.Value.Length;
		}

		public object GetElementValue() => this.Value;
	}
}
