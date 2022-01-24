using FBXSharp.Core;
using System;
using System.Diagnostics;

namespace FBXSharp.Elementary
{
	[DebuggerDisplay("Raw : {this.Value.Length} bytes")]
	public class BinaryAttribute : IGenericAttribute<byte[]>
	{
		public static readonly IElementAttributeType PropertyType = IElementAttributeType.Binary;

		public static readonly int PropertyStride = 1;

		public static readonly int PropertyLength = -1;

		public IElementAttributeType Type => BinaryAttribute.PropertyType;

		public int Stride => BinaryAttribute.PropertyStride;

		public int Length { get; }

		public byte[] Value { get; }

		public BinaryAttribute(byte[] value)
		{
			this.Value = value ?? Array.Empty<byte>();
			this.Length = this.Value.Length;
		}

		public object GetElementValue() => this.Value;
	}
}
