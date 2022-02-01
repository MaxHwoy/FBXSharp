using FBXSharp.Core;
using System;
using System.Diagnostics;

namespace FBXSharp.Elementary
{
	[DebuggerDisplay("{this.Value}")]
	public class StringAttribute : IGenericAttribute<string>
	{
		public static readonly IElementAttributeType PropertyType = IElementAttributeType.String;

		public static readonly int PropertyStride = -1;

		public static readonly int PropertyLength = 1;

		public IElementAttributeType Type => StringAttribute.PropertyType;

		public int Size => 4 + this.Stride;

		public int Stride { get; }

		public int Length => StringAttribute.PropertyLength;

		public string Value { get; }

		public StringAttribute(string value)
		{
			// 4 bytes char count + string length
			this.Value = value ?? String.Empty;
			this.Stride = this.Value.Length;
		}

		public object GetElementValue() => this.Value;
	}
}
