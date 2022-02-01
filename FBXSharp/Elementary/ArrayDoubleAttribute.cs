using FBXSharp.Core;
using System;
using System.Diagnostics;

namespace FBXSharp.Elementary
{
	[DebuggerDisplay("Double : {this.Value.Length} Items")]
	public class ArrayDoubleAttribute : IGenericAttribute<double[]>
	{
		public static readonly IElementAttributeType PropertyType = IElementAttributeType.ArrayDouble;

		public static readonly int PropertyStride = 8;

		public static readonly int PropertyLength = -1;

		public IElementAttributeType Type => ArrayDoubleAttribute.PropertyType;

		public int Size => 12 + ArrayDoubleAttribute.PropertyStride * this.Length;

		public int Stride => ArrayDoubleAttribute.PropertyStride;

		public int Length { get; }

		public double[] Value { get; }

		public ArrayDoubleAttribute(double[] value)
		{
			this.Value = value ?? Array.Empty<double>();
			this.Length = this.Value.Length;
		}

		public object GetElementValue() => this.Value;
	}
}
