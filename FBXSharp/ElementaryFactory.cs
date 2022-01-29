using FBXSharp.Core;
using FBXSharp.Elementary;
using FBXSharp.ValueTypes;
using System;

namespace FBXSharp
{
	internal static class ElementaryFactory
	{
		private static S[] DeepGenericCopy<T, S>(T[] buffer)
		{
			var array = new S[buffer.Length];

			Array.Copy(buffer, array, array.Length);

			return array;
		}

		private static Vector2[] DeepVector2Copy<T>(T[] buffer)
		{
			var array = new Vector2[buffer.Length >> 1];

			for (int i = 0, k = 0; i < array.Length; ++i, k += 2)
			{
				array[i] = new Vector2
				(
					Convert.ToDouble(buffer[k + 0]),
					Convert.ToDouble(buffer[k + 1])
				);
			}

			return array;
		}

		private static Vector3[] DeepVector3Copy<T>(T[] buffer)
		{
			var array = new Vector3[buffer.Length / 3];

			for (int i = 0, k = 0; i < array.Length; ++i, k += 3)
			{
				array[i] = new Vector3
				(
					Convert.ToDouble(buffer[k + 0]),
					Convert.ToDouble(buffer[k + 1]),
					Convert.ToDouble(buffer[k + 2])
				);
			}

			return array;
		}

		private static Vector4[] DeepVector4Copy<T>(T[] buffer)
		{
			var array = new Vector4[buffer.Length >> 2];

			for (int i = 0, k = 0; i < array.Length; ++i, k += 4)
			{
				array[i] = new Vector4
				(
					Convert.ToDouble(buffer[k + 0]),
					Convert.ToDouble(buffer[k + 1]),
					Convert.ToDouble(buffer[k + 2]),
					Convert.ToDouble(buffer[k + 3])
				);
			}

			return array;
		}

		public static bool ToInt32(IElementAttribute attribute, out int value)
		{
			switch (attribute.Type)
			{
				case IElementAttributeType.Byte:
				case IElementAttributeType.Int16:
				case IElementAttributeType.Int32:
				case IElementAttributeType.Int64:
				case IElementAttributeType.Single:
				case IElementAttributeType.Double:
					value = Convert.ToInt32(attribute.GetElementValue());
					return true;

				case IElementAttributeType.ArrayBoolean:
				case IElementAttributeType.ArrayInt32:
				case IElementAttributeType.ArrayInt64:
				case IElementAttributeType.ArraySingle:
				case IElementAttributeType.ArrayDouble:
					var buffer = attribute.GetElementValue() as Array;
					var result = buffer?.Length > 0;
					value = result ? Convert.ToInt32(buffer.GetValue(0)) : 0;
					return result;

				default:
					value = 0;
					return false;
			}
		}

		public static bool ToDouble(IElementAttribute attribute, out double value)
		{
			switch (attribute.Type)
			{
				case IElementAttributeType.Byte:
				case IElementAttributeType.Int16:
				case IElementAttributeType.Int32:
				case IElementAttributeType.Int64:
				case IElementAttributeType.Single:
				case IElementAttributeType.Double:
					value = Convert.ToDouble(attribute.GetElementValue());
					return true;

				case IElementAttributeType.ArrayBoolean:
				case IElementAttributeType.ArrayInt32:
				case IElementAttributeType.ArrayInt64:
				case IElementAttributeType.ArraySingle:
				case IElementAttributeType.ArrayDouble:
					var buffer = attribute.GetElementValue() as Array;
					var result = buffer?.Length > 0;
					value = result ? Convert.ToDouble(buffer.GetValue(0)) : 0.0;
					return result;

				default:
					value = 0.0;
					return false;
			}
		}

		public static bool ToVector2(IElementAttribute attribute, out Vector2 vector)
		{
			Array array;

			switch (attribute.Type)
			{
				case IElementAttributeType.ArrayBoolean:
				case IElementAttributeType.ArrayInt32:
				case IElementAttributeType.ArrayInt64:
				case IElementAttributeType.ArraySingle:
				case IElementAttributeType.ArrayDouble:
					array = attribute.GetElementValue() as Array;
					break;

				default:
					vector = default;
					return false;
			}

			if (array.Length < 2)
			{
				vector = default;
				return false;
			}

			vector = new Vector2
			(
				Convert.ToDouble(array.GetValue(0)),
				Convert.ToDouble(array.GetValue(1))
			);

			return true;
		}

		public static bool ToVector3(IElementAttribute attribute, out Vector3 vector)
		{
			Array array;

			switch (attribute.Type)
			{
				case IElementAttributeType.ArrayBoolean:
				case IElementAttributeType.ArrayInt32:
				case IElementAttributeType.ArrayInt64:
				case IElementAttributeType.ArraySingle:
				case IElementAttributeType.ArrayDouble:
					array = attribute.GetElementValue() as Array;
					break;

				default:
					vector = default;
					return false;
			}

			if (array.Length < 3)
			{
				vector = default;
				return false;
			}

			vector = new Vector3
			(
				Convert.ToDouble(array.GetValue(0)),
				Convert.ToDouble(array.GetValue(1)),
				Convert.ToDouble(array.GetValue(2))
			);

			return true;
		}

		public static bool ToVector4(IElementAttribute attribute, out Vector4 vector)
		{
			Array array;

			switch (attribute.Type)
			{
				case IElementAttributeType.ArrayBoolean:
				case IElementAttributeType.ArrayInt32:
				case IElementAttributeType.ArrayInt64:
				case IElementAttributeType.ArraySingle:
				case IElementAttributeType.ArrayDouble:
					array = attribute.GetElementValue() as Array;
					break;

				default:
					vector = default;
					return false;
			}

			if (array.Length < 4)
			{
				vector = default;
				return false;
			}

			vector = new Vector4
			(
				Convert.ToDouble(array.GetValue(0)),
				Convert.ToDouble(array.GetValue(1)),
				Convert.ToDouble(array.GetValue(2)),
				Convert.ToDouble(array.GetValue(3))
			);

			return true;
		}

		public static bool ToInt32Array(IElementAttribute attribute, out int[] array)
		{
			switch (attribute.Type)
			{
				case IElementAttributeType.ArrayBoolean:
					{
						array = ElementaryFactory.DeepGenericCopy<bool, int>(attribute.GetElementValue() as bool[]);
						return true;
					}

				case IElementAttributeType.ArrayDouble:
					{
						array = ElementaryFactory.DeepGenericCopy<double, int>(attribute.GetElementValue() as double[]);
						return true;
					}

				case IElementAttributeType.ArrayInt32:
					{
						array = ElementaryFactory.DeepGenericCopy<int, int>(attribute.GetElementValue() as int[]);
						return true;
					}

				case IElementAttributeType.ArrayInt64:
					{
						array = ElementaryFactory.DeepGenericCopy<long, int>(attribute.GetElementValue() as long[]);
						return true;
					}

				case IElementAttributeType.ArraySingle:
					{
						array = ElementaryFactory.DeepGenericCopy<float, int>(attribute.GetElementValue() as float[]);
						return true;
					}

				default:
					{
						array = null;
						return false;
					}
			}
		}

		public static bool ToDoubleArray(IElementAttribute attribute, out double[] array)
		{
			switch (attribute.Type)
			{
				case IElementAttributeType.ArrayBoolean:
					{
						array = ElementaryFactory.DeepGenericCopy<bool, double>(attribute.GetElementValue() as bool[]);
						return true;
					}

				case IElementAttributeType.ArrayDouble:
					{
						array = ElementaryFactory.DeepGenericCopy<double, double>(attribute.GetElementValue() as double[]);
						return true;
					}

				case IElementAttributeType.ArrayInt32:
					{
						array = ElementaryFactory.DeepGenericCopy<int, double>(attribute.GetElementValue() as int[]);
						return true;
					}

				case IElementAttributeType.ArrayInt64:
					{
						array = ElementaryFactory.DeepGenericCopy<long, double>(attribute.GetElementValue() as long[]);
						return true;
					}

				case IElementAttributeType.ArraySingle:
					{
						array = ElementaryFactory.DeepGenericCopy<float, double>(attribute.GetElementValue() as float[]);
						return true;
					}

				default:
					{
						array = null;
						return false;
					}
			}
		}

		public static bool ToVector2Array(IElementAttribute attribute, out Vector2[] array)
		{
			switch (attribute.Type)
			{
				case IElementAttributeType.ArrayBoolean:
					{
						array = ElementaryFactory.DeepVector2Copy(attribute.GetElementValue() as bool[]);
						return true;
					}

				case IElementAttributeType.ArrayDouble:
					{
						array = ElementaryFactory.DeepVector2Copy(attribute.GetElementValue() as double[]);
						return true;
					}

				case IElementAttributeType.ArrayInt32:
					{
						array = ElementaryFactory.DeepVector2Copy(attribute.GetElementValue() as int[]);
						return true;
					}

				case IElementAttributeType.ArrayInt64:
					{
						array = ElementaryFactory.DeepVector2Copy(attribute.GetElementValue() as long[]);
						return true;
					}

				case IElementAttributeType.ArraySingle:
					{
						array = ElementaryFactory.DeepVector2Copy(attribute.GetElementValue() as float[]);
						return true;
					}

				default:
					{
						array = null;
						return false;
					}
			}
		}

		public static bool ToVector3Array(IElementAttribute attribute, out Vector3[] array)
		{
			switch (attribute.Type)
			{
				case IElementAttributeType.ArrayBoolean:
					{
						array = ElementaryFactory.DeepVector3Copy(attribute.GetElementValue() as bool[]);
						return true;
					}

				case IElementAttributeType.ArrayDouble:
					{
						array = ElementaryFactory.DeepVector3Copy(attribute.GetElementValue() as double[]);
						return true;
					}

				case IElementAttributeType.ArrayInt32:
					{
						array = ElementaryFactory.DeepVector3Copy(attribute.GetElementValue() as int[]);
						return true;
					}

				case IElementAttributeType.ArrayInt64:
					{
						array = ElementaryFactory.DeepVector3Copy(attribute.GetElementValue() as long[]);
						return true;
					}

				case IElementAttributeType.ArraySingle:
					{
						array = ElementaryFactory.DeepVector3Copy(attribute.GetElementValue() as float[]);
						return true;
					}

				default:
					{
						array = null;
						return false;
					}
			}
		}

		public static bool ToVector4Array(IElementAttribute attribute, out Vector4[] array)
		{
			switch (attribute.Type)
			{
				case IElementAttributeType.ArrayBoolean:
					{
						array = ElementaryFactory.DeepVector4Copy(attribute.GetElementValue() as bool[]);
						return true;
					}

				case IElementAttributeType.ArrayDouble:
					{
						array = ElementaryFactory.DeepVector4Copy(attribute.GetElementValue() as double[]);
						return true;
					}

				case IElementAttributeType.ArrayInt32:
					{
						array = ElementaryFactory.DeepVector4Copy(attribute.GetElementValue() as int[]);
						return true;
					}

				case IElementAttributeType.ArrayInt64:
					{
						array = ElementaryFactory.DeepVector4Copy(attribute.GetElementValue() as long[]);
						return true;
					}

				case IElementAttributeType.ArraySingle:
					{
						array = ElementaryFactory.DeepVector4Copy(attribute.GetElementValue() as float[]);
						return true;
					}

				default:
					{
						array = null;
						return false;
					}
			}
		}

		public static Vector4 ExtendVector(in Vector3 vector) => new Vector4(vector.X, vector.Y, vector.Z);

		public static Vector4 ExtendVector(in Vector3 vector, double w) => new Vector4(vector.X, vector.Y, vector.Z, w);

		public static IElementAttribute GetElementAttribute(object value)
		{
			var type = value.GetType();

			switch (Type.GetTypeCode(type))
			{
				case TypeCode.Boolean: return ElementaryFactory.GetElementAttribute((bool)value);
				case TypeCode.Char: return ElementaryFactory.GetElementAttribute((char)value);
				case TypeCode.SByte: return ElementaryFactory.GetElementAttribute((sbyte)value);
				case TypeCode.Byte: return ElementaryFactory.GetElementAttribute((byte)value);
				case TypeCode.Int16: return ElementaryFactory.GetElementAttribute((short)value);
				case TypeCode.UInt16: return ElementaryFactory.GetElementAttribute((ushort)value);
				case TypeCode.Int32: return ElementaryFactory.GetElementAttribute((int)value);
				case TypeCode.UInt32: return ElementaryFactory.GetElementAttribute((uint)value);
				case TypeCode.Int64: return ElementaryFactory.GetElementAttribute((long)value);
				case TypeCode.UInt64: return ElementaryFactory.GetElementAttribute((ulong)value);
				case TypeCode.Single: return ElementaryFactory.GetElementAttribute((float)value);
				case TypeCode.Double: return ElementaryFactory.GetElementAttribute((double)value);
				case TypeCode.String: return ElementaryFactory.GetElementAttribute(value.ToString());
			}

			if (type.IsArray)
			{
				switch (Type.GetTypeCode(type.GetElementType()))
				{
					case TypeCode.Boolean: return ElementaryFactory.GetElementAttribute(value as bool[]);
					case TypeCode.Char: return ElementaryFactory.GetElementAttribute(value as char[]);
					case TypeCode.SByte: return ElementaryFactory.GetElementAttribute(value as sbyte[]);
					case TypeCode.Byte: return ElementaryFactory.GetElementAttribute(value as byte[]);
					case TypeCode.Int16: return ElementaryFactory.GetElementAttribute(value as short[]);
					case TypeCode.UInt16: return ElementaryFactory.GetElementAttribute(value as ushort[]);
					case TypeCode.Int32: return ElementaryFactory.GetElementAttribute(value as int[]);
					case TypeCode.UInt32: return ElementaryFactory.GetElementAttribute(value as uint[]);
					case TypeCode.Int64: return ElementaryFactory.GetElementAttribute(value as long[]);
					case TypeCode.UInt64: return ElementaryFactory.GetElementAttribute(value as ulong[]);
					case TypeCode.Single: return ElementaryFactory.GetElementAttribute(value as float[]);
					case TypeCode.Double: return ElementaryFactory.GetElementAttribute(value as double[]);
				}
			}

			return ElementaryFactory.GetElementAttribute(value.ToString());
		}

		public static IElementAttribute GetElementAttribute(bool value)
		{
			return new Int32Attribute(value ? 1 : 0);
		}
		
		public static IElementAttribute GetElementAttribute(byte value)
		{
			return new ByteAttribute(value);
		}
		
		public static IElementAttribute GetElementAttribute(sbyte value)
		{
			return new ByteAttribute((byte)value);
		}
		
		public static IElementAttribute GetElementAttribute(char value)
		{
			return new Int16Attribute((short)value);
		}
		
		public static IElementAttribute GetElementAttribute(short value)
		{
			return new Int16Attribute(value);
		}
		
		public static IElementAttribute GetElementAttribute(ushort value)
		{
			return new Int16Attribute((short)value);
		}
		
		public static IElementAttribute GetElementAttribute(int value)
		{
			return new Int32Attribute(value);
		}
		
		public static IElementAttribute GetElementAttribute(uint value)
		{
			return new Int32Attribute((int)value);
		}
		
		public static IElementAttribute GetElementAttribute(long value)
		{
			return new Int64Attribute(value);
		}
		
		public static IElementAttribute GetElementAttribute(ulong value)
		{
			return new Int64Attribute((long)value);
		}

		public static IElementAttribute GetElementAttribute(float value)
		{
			return new SingleAttribute(value);
		}

		public static IElementAttribute GetElementAttribute(double value)
		{
			return new DoubleAttribute(value);
		}

		public static IElementAttribute GetElementAttribute(string value)
		{
			return new StringAttribute(value);
		}

		public static IElementAttribute GetElementAttribute(bool[] value)
		{
			return new ArrayBooleanAttribute(value);
		}

		public static IElementAttribute GetElementAttribute(byte[] value)
		{
			return new BinaryAttribute(value);
		}

		public static IElementAttribute GetElementAttribute(sbyte[] value)
		{
			return new BinaryAttribute(ElementaryFactory.DeepGenericCopy<sbyte, byte>(value));
		}

		public static IElementAttribute GetElementAttribute(char[] value)
		{
			return new ArrayInt32Attribute(ElementaryFactory.DeepGenericCopy<char, int>(value));
		}

		public static IElementAttribute GetElementAttribute(short[] value)
		{
			return new ArrayInt32Attribute(ElementaryFactory.DeepGenericCopy<short, int>(value));
		}

		public static IElementAttribute GetElementAttribute(ushort[] value)
		{
			return new ArrayInt32Attribute(ElementaryFactory.DeepGenericCopy<ushort, int>(value));
		}

		public static IElementAttribute GetElementAttribute(int[] value)
		{
			return new ArrayInt32Attribute(value);
		}

		public static IElementAttribute GetElementAttribute(uint[] value)
		{
			return new ArrayInt32Attribute(ElementaryFactory.DeepGenericCopy<uint, int>(value));
		}

		public static IElementAttribute GetElementAttribute(long[] value)
		{
			return new ArrayInt64Attribute(value);
		}

		public static IElementAttribute GetElementAttribute(ulong[] value)
		{
			return new ArrayInt64Attribute(ElementaryFactory.DeepGenericCopy<ulong, long>(value));
		}

		public static IElementAttribute GetElementAttribute(float[] value)
		{
			return new ArraySingleAttribute(value);
		}

		public static IElementAttribute GetElementAttribute(double[] value)
		{
			return new ArrayDoubleAttribute(value);
		}
	}
}
