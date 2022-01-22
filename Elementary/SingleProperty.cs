using FBXSharp.Core;
using System.Diagnostics;

namespace FBXSharp.Elementary
{
	[DebuggerDisplay("{this.Value}F")]
	public class SingleAttribute : IGenericAttribute<float>
	{
		public static readonly IElementAttributeType PropertyType = IElementAttributeType.Single;

		public static readonly int PropertyStride = 4;

		public static readonly int PropertyLength = 1;

		public IElementAttributeType Type => SingleAttribute.PropertyType;

		public int Stride => SingleAttribute.PropertyStride;

		public int Length => SingleAttribute.PropertyLength;

		public float Value { get; }

		public SingleAttribute(float value) => this.Value = value;

		public object GetElementValue() => this.Value;
	}
}
