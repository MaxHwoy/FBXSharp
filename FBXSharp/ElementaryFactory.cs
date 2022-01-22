using FBXSharp.Core;
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
				case IElementAttributeType.Boolean:
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
				case IElementAttributeType.Boolean:
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
	}
}
