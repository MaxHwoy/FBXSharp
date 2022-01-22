using FBXSharp.Core;
using System.Diagnostics;

namespace FBXSharp.Elementary
{
	[DebuggerDisplay("{this.Value}D")]
	public class DoubleAttribute : IGenericAttribute<double>
	{
		public static readonly IElementAttributeType PropertyType = IElementAttributeType.Double;

		public static readonly int PropertyStride = 8;

		public static readonly int PropertyLength = 1;

		public IElementAttributeType Type => DoubleAttribute.PropertyType;

		public int Stride => DoubleAttribute.PropertyStride;

		public int Length => DoubleAttribute.PropertyLength;

		public double Value { get; }

		public DoubleAttribute(double value) => this.Value = value;

		public object GetElementValue() => this.Value;
	}
}
