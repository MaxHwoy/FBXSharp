using FBXSharp.Core;
using FBXSharp.ValueTypes;
using System;
using System.Collections.Generic;

namespace FBXSharp
{
	internal static class PropertyFactory
	{
		public delegate IElementProperty AttributeCreator(IElement element);

		private static readonly Dictionary<string, IElementPropertyType> ms_typeMapper = new Dictionary<string, IElementPropertyType>()
		{
			{ "Byte", IElementPropertyType.SByte },
			{ "UByte", IElementPropertyType.Byte },
			{ "Short", IElementPropertyType.Short },
			{ "UShort", IElementPropertyType.UShort },
			{ "UInteger", IElementPropertyType.UInt },
			{ "LongLong", IElementPropertyType.Long },
			{ "ULongLong", IElementPropertyType.ULong },
			{ "HalfFloat", IElementPropertyType.Half },
			{ "Bool", IElementPropertyType.Bool },
			{ "bool", IElementPropertyType.Bool },
			{ "Integer", IElementPropertyType.Int },
			{ "int", IElementPropertyType.Int },
			{ "Float", IElementPropertyType.Float },
			{ "float", IElementPropertyType.Float },
			{ "Number", IElementPropertyType.Double },
			{ "double", IElementPropertyType.Double },
			{ "Vector2", IElementPropertyType.Double2 },
			{ "Vector2D", IElementPropertyType.Double2 },
			{ "Vector", IElementPropertyType.Double3 },
			{ "Vector3D", IElementPropertyType.Double3 },
			{ "Vector4", IElementPropertyType.Double4 },
			{ "Vector4D", IElementPropertyType.Double4 },
			{ "Matrix", IElementPropertyType.Double4x4 },
			{ "matrix4x4", IElementPropertyType.Double4x4 },
			{ "Enum", IElementPropertyType.Enum },
			{ "enum", IElementPropertyType.Enum },
			{ "stringlist", IElementPropertyType.Enum },
			{ "Time", IElementPropertyType.Time },
			{ "KTime", IElementPropertyType.Time },
			{ "TimeCode", IElementPropertyType.Double3 },
			{ "KTimeCode", IElementPropertyType.Double3 },
			{ "Reference", IElementPropertyType.Reference },
			{ "ReferenceProperty", IElementPropertyType.Reference },
			{ "object", IElementPropertyType.Undefined },
			{ "KString", IElementPropertyType.String },
			{ "charptr", IElementPropertyType.String },
			{ "Action", IElementPropertyType.Bool },
			{ "event", IElementPropertyType.Undefined },
			{ "Compound", IElementPropertyType.Undefined },
			{ "Blob", IElementPropertyType.Blob },
			{ "Distance", IElementPropertyType.Distance },
			{ "DateTime", IElementPropertyType.DateTime },
			{ "Color", IElementPropertyType.Double3 },
			{ "ColorRGB", IElementPropertyType.Double3 },
			{ "ColorAndAlpha", IElementPropertyType.Double4 },
			{ "ColorRGBA", IElementPropertyType.Double4 },
			{ "Real", IElementPropertyType.Double },
			{ "Translation", IElementPropertyType.Double3 },
			{ "Rotation", IElementPropertyType.Double3 },
			{ "Scaling", IElementPropertyType.Double3 },
			{ "Quaternion", IElementPropertyType.Double4 },
			{ "Lcl Translation", IElementPropertyType.Double3 },
			{ "Lcl Rotation", IElementPropertyType.Double3 },
			{ "Lcl Scaling", IElementPropertyType.Double3 },
			{ "Lcl Quaternion", IElementPropertyType.Double4 },
			{ "Matrix Transformation", IElementPropertyType.Double4x4 },
			{ "Matrix Translation", IElementPropertyType.Double4x4 },
			{ "Matrix Rotation", IElementPropertyType.Double4x4 },
			{ "Matrix Scaling", IElementPropertyType.Double4x4 },
			{ "Emissive", IElementPropertyType.Double3 },
			{ "EmissiveFactor", IElementPropertyType.Double },
			{ "Ambient", IElementPropertyType.Double3 },
			{ "AmbientFactor", IElementPropertyType.Double },
			{ "Diffuse", IElementPropertyType.Double3 },
			{ "DiffuseFactor", IElementPropertyType.Double },
			{ "NormalMap", IElementPropertyType.Double3 },
			{ "Bump", IElementPropertyType.Double },
			{ "Transparent", IElementPropertyType.Double3 },
			{ "TransparencyFactor", IElementPropertyType.Double },
			{ "Specular", IElementPropertyType.Double3 },
			{ "SpecularFactor", IElementPropertyType.Double },
			{ "Shininess", IElementPropertyType.Double },
			{ "Reflection", IElementPropertyType.Double3 },
			{ "ReflectionFactor", IElementPropertyType.Double },
			{ "Displacement", IElementPropertyType.Double3 },
			{ "VectorDisplacement", IElementPropertyType.Double3 },
			{ "Unknown Factor", IElementPropertyType.Double },
			{ "Unknown texture", IElementPropertyType.Double3 },
			{ "Url", IElementPropertyType.String },
			{ "XRefUrl", IElementPropertyType.String },
			{ "LayerElementUndefined", IElementPropertyType.Undefined },
			{ "LayerElementNormal", IElementPropertyType.Double4 },
			{ "LayerElementBinormal", IElementPropertyType.Double4 },
			{ "LayerElementTangent", IElementPropertyType.Double4 },
			{ "LayerElementMaterial", IElementPropertyType.Reference },
			{ "LayerElementTexture", IElementPropertyType.Reference },
			{ "LayerElementPolygonGroup", IElementPropertyType.Int },
			{ "LayerElementUV", IElementPropertyType.Double2 },
			{ "LayerElementVertexColor", IElementPropertyType.Double4 },
			{ "LayerElementSmoothing", IElementPropertyType.Int },
			{ "LayerElementCrease", IElementPropertyType.Double },
			{ "LayerElementHole", IElementPropertyType.Bool },
			{ "LayerElementUserData", IElementPropertyType.Reference },
			{ "LayerElementVisibility", IElementPropertyType.Bool },
			{ "Intensity", IElementPropertyType.Double },
			{ "Cone angle", IElementPropertyType.Double },
			{ "Fog", IElementPropertyType.Double },
			{ "Shape", IElementPropertyType.Double },
			{ "FieldOfView", IElementPropertyType.Double },
			{ "FieldOfViewX", IElementPropertyType.Double },
			{ "FieldOfViewY", IElementPropertyType.Double },
			{ "OpticalCenterX", IElementPropertyType.Double },
			{ "OpticalCenterY", IElementPropertyType.Double },
			{ "Roll", IElementPropertyType.Double },
			{ "Camera Index", IElementPropertyType.Int },
			{ "TimeWarp", IElementPropertyType.Double },
			{ "Visibility", IElementPropertyType.Double },
			{ "Visibility Inheritance", IElementPropertyType.Bool },
			{ "Translation UV", IElementPropertyType.Double3 },
			{ "Scaling UV", IElementPropertyType.Double3 },
			{ "TextureRotation", IElementPropertyType.Double3 },
			{ "HSB", IElementPropertyType.Double3 },
			{ "Orientation", IElementPropertyType.Double3 },
			{ "Look at", IElementPropertyType.Double3 },
			{ "Occlusion", IElementPropertyType.Double },
			{ "Weight", IElementPropertyType.Double },
			{ "IK Reach Translation", IElementPropertyType.Double },
			{ "IK Reach Rotation", IElementPropertyType.Double },
			{ "Presets", IElementPropertyType.Enum },
			{ "Statistics", IElementPropertyType.String },
			{ "Units", IElementPropertyType.String },
			{ "Warning", IElementPropertyType.String },
			{ "Web", IElementPropertyType.String },
			{ "TextLine", IElementPropertyType.String },
			{ "Alias", IElementPropertyType.Enum },
		};

		private static readonly Dictionary<IElementPropertyType, AttributeCreator> ms_activators = new Dictionary<IElementPropertyType, AttributeCreator>()
		{
			{ IElementPropertyType.Undefined, PropertyFactory.AsAttributeAny },
			{ IElementPropertyType.SByte, PropertyFactory.AsAttributeSByte },
			{ IElementPropertyType.Byte, PropertyFactory.AsAttributeByte },
			{ IElementPropertyType.Short, PropertyFactory.AsAttributeShort },
			{ IElementPropertyType.UShort, PropertyFactory.AsAttributeUShort },
			{ IElementPropertyType.UInt, PropertyFactory.AsAttributeUInt },
			{ IElementPropertyType.Long, PropertyFactory.AsAttributeLong },
			{ IElementPropertyType.ULong, PropertyFactory.AsAttributeULong },
			{ IElementPropertyType.Half, PropertyFactory.AsAttributeHalf },
			{ IElementPropertyType.Bool, PropertyFactory.AsAttributeBool },
			{ IElementPropertyType.Int, PropertyFactory.AsAttributeInt },
			{ IElementPropertyType.Float, PropertyFactory.AsAttributeFloat },
			{ IElementPropertyType.Double, PropertyFactory.AsAttributeDouble },
			{ IElementPropertyType.Double2, PropertyFactory.AsAttributeDouble2 },
			{ IElementPropertyType.Double3, PropertyFactory.AsAttributeDouble3 },
			{ IElementPropertyType.Double4, PropertyFactory.AsAttributeDouble4 },
			{ IElementPropertyType.Double4x4, PropertyFactory.AsAttributeDouble4x4 },
			{ IElementPropertyType.Enum, PropertyFactory.AsAttributeEnum },
			{ IElementPropertyType.String, PropertyFactory.AsAttributeString },
			{ IElementPropertyType.Time, PropertyFactory.AsAttributeTime },
			{ IElementPropertyType.Reference, PropertyFactory.AsAttributeReference },
			{ IElementPropertyType.Blob, PropertyFactory.AsAttributeBlob },
			{ IElementPropertyType.Distance, PropertyFactory.AsAttributeDistance },
			{ IElementPropertyType.DateTime, PropertyFactory.AsAttributeDateTime },
		};

		private static IElementPropertyFlags InternalParseFlags(IElement element)
		{
			var result = IElementPropertyFlags.Imported;
			var buffer = element.Attributes[3].GetElementValue().ToString();

			if (buffer.IndexOf('A') >= 0)
			{
				result |= IElementPropertyFlags.Animatable;
			}

			if (buffer.IndexOf('+') >= 0)
			{
				result |= IElementPropertyFlags.Animated;
			}

			if (buffer.IndexOf('U') >= 0)
			{
				result |= IElementPropertyFlags.UserDefined;
			}

			if (buffer.IndexOf('H') >= 0)
			{
				result |= IElementPropertyFlags.Hidden;
			}

			if (buffer.IndexOf('N') >= 0)
			{
				result |= IElementPropertyFlags.NotSavable;
			}

			var @lock = buffer.IndexOf('L');
			var muted = buffer.IndexOf('M');

			if (@lock >= 0)
			{
				var value = buffer[@lock + 1];

				if (value >= '1' && value <= '9')
				{
					result |= (IElementPropertyFlags)((value - '0') << 7);
				}
				else if (value < 'a' || value > 'e')
				{
					result |= (IElementPropertyFlags)(0b00001111 << 7);
				}
				else
				{
					result |= (IElementPropertyFlags)((value - 'W') << 7);
				}
			}

			if (muted >= 0)
			{
				var value = buffer[muted + 1];

				if (value >= '1' && value <= '9')
				{
					result |= (IElementPropertyFlags)((value - '0') << 11);
				}
				else if (value < 'a' || value > 'e')
				{
					result |= (IElementPropertyFlags)(0b00001111 << 11);
				}
				else
				{
					result |= (IElementPropertyFlags)((value - 'W') << 11);
				}
			}

			return result;
		}

		private static FBXProperty<T> InternalCreateProperty<T>(IElement element, T value)
		{
			return new FBXProperty<T>
			(
				element.Attributes[1].GetElementValue().ToString(),
				element.Attributes[2].GetElementValue().ToString(),
				element.Attributes[0].GetElementValue().ToString(),
				PropertyFactory.InternalParseFlags(element),
				value
			);
		}

		private static IElementProperty AsAttributeAny(IElement element)
		{
			if (element.Attributes.Length < 4)
			{
				return null;
			}

			var array = new object[element.Attributes.Length - 4];

			for (int i = 0, k = 4; i < array.Length; ++i, ++k)
			{
				array[i] = element.Attributes[k];
			}

			return PropertyFactory.InternalCreateProperty(element, array);
		}

		private static IElementProperty AsAttributeSByte(IElement element)
		{
			if (element.Attributes.Length < 5)
			{
				return null;
			}

			var property = PropertyFactory.InternalCreateProperty(element, Convert.ToSByte(element.Attributes[4].GetElementValue()));

			if ((property.Flags & IElementPropertyFlags.UserDefined) != 0)
			{
				if (element.Attributes.Length >= 6)
				{
					property.SetMinValue(Convert.ToSByte(element.Attributes[5].GetElementValue()));
				}

				if (element.Attributes.Length >= 7)
				{
					property.SetMaxValue(Convert.ToSByte(element.Attributes[6].GetElementValue()));
				}
			}

			return property;
		}

		private static IElementProperty AsAttributeByte(IElement element)
		{
			if (element.Attributes.Length < 5)
			{
				return null;
			}

			var property = PropertyFactory.InternalCreateProperty(element, Convert.ToByte(element.Attributes[4].GetElementValue()));

			if ((property.Flags & IElementPropertyFlags.UserDefined) != 0)
			{
				if (element.Attributes.Length >= 6)
				{
					property.SetMinValue(Convert.ToByte(element.Attributes[5].GetElementValue()));
				}

				if (element.Attributes.Length >= 7)
				{
					property.SetMaxValue(Convert.ToByte(element.Attributes[6].GetElementValue()));
				}
			}

			return property;
		}

		private static IElementProperty AsAttributeShort(IElement element)
		{
			if (element.Attributes.Length < 5)
			{
				return null;
			}

			var property = PropertyFactory.InternalCreateProperty(element, Convert.ToInt16(element.Attributes[4].GetElementValue()));

			if ((property.Flags & IElementPropertyFlags.UserDefined) != 0)
			{
				if (element.Attributes.Length >= 6)
				{
					property.SetMinValue(Convert.ToInt16(element.Attributes[5].GetElementValue()));
				}

				if (element.Attributes.Length >= 7)
				{
					property.SetMaxValue(Convert.ToInt16(element.Attributes[6].GetElementValue()));
				}
			}

			return property;
		}

		private static IElementProperty AsAttributeUShort(IElement element)
		{
			if (element.Attributes.Length < 5)
			{
				return null;
			}

			var property = PropertyFactory.InternalCreateProperty(element, Convert.ToUInt16(element.Attributes[4].GetElementValue()));

			if ((property.Flags & IElementPropertyFlags.UserDefined) != 0)
			{
				if (element.Attributes.Length >= 6)
				{
					property.SetMinValue(Convert.ToUInt16(element.Attributes[5].GetElementValue()));
				}

				if (element.Attributes.Length >= 7)
				{
					property.SetMaxValue(Convert.ToUInt16(element.Attributes[6].GetElementValue()));
				}
			}

			return property;
		}

		private static IElementProperty AsAttributeUInt(IElement element)
		{
			if (element.Attributes.Length < 5)
			{
				return null;
			}

			var property = PropertyFactory.InternalCreateProperty(element, Convert.ToUInt32(element.Attributes[4].GetElementValue()));

			if ((property.Flags & IElementPropertyFlags.UserDefined) != 0)
			{
				if (element.Attributes.Length >= 6)
				{
					property.SetMinValue(Convert.ToUInt32(element.Attributes[5].GetElementValue()));
				}

				if (element.Attributes.Length >= 7)
				{
					property.SetMaxValue(Convert.ToUInt32(element.Attributes[6].GetElementValue()));
				}
			}

			return property;
		}

		private static IElementProperty AsAttributeLong(IElement element)
		{
			if (element.Attributes.Length < 5)
			{
				return null;
			}

			var property = PropertyFactory.InternalCreateProperty(element, Convert.ToInt64(element.Attributes[4].GetElementValue()));

			if ((property.Flags & IElementPropertyFlags.UserDefined) != 0)
			{
				if (element.Attributes.Length >= 6)
				{
					property.SetMinValue(Convert.ToInt64(element.Attributes[5].GetElementValue()));
				}

				if (element.Attributes.Length >= 7)
				{
					property.SetMaxValue(Convert.ToInt64(element.Attributes[6].GetElementValue()));
				}
			}

			return property;
		}

		private static IElementProperty AsAttributeULong(IElement element)
		{
			if (element.Attributes.Length < 5)
			{
				return null;
			}

			var property = PropertyFactory.InternalCreateProperty(element, Convert.ToUInt64(element.Attributes[4].GetElementValue()));

			if ((property.Flags & IElementPropertyFlags.UserDefined) != 0)
			{
				if (element.Attributes.Length >= 6)
				{
					property.SetMinValue(Convert.ToUInt64(element.Attributes[5].GetElementValue()));
				}

				if (element.Attributes.Length >= 7)
				{
					property.SetMaxValue(Convert.ToUInt64(element.Attributes[6].GetElementValue()));
				}
			}

			return property;
		}

		private static IElementProperty AsAttributeHalf(IElement element)
		{
			if (element.Attributes.Length < 5)
			{
				return null;
			}

			var property = PropertyFactory.InternalCreateProperty(element, new Half(Convert.ToSingle(element.Attributes[4].GetElementValue())));

			if ((property.Flags & IElementPropertyFlags.UserDefined) != 0)
			{
				if (element.Attributes.Length >= 6)
				{
					property.SetMinValue(new Half((float)Convert.ToDouble(element.Attributes[5].GetElementValue())));
				}

				if (element.Attributes.Length >= 7)
				{
					property.SetMaxValue(new Half((float)Convert.ToDouble(element.Attributes[6].GetElementValue())));
				}
			}

			return property;
		}

		private static IElementProperty AsAttributeBool(IElement element)
		{
			if (element.Attributes.Length < 5)
			{
				return null;
			}

			var property = PropertyFactory.InternalCreateProperty(element, Convert.ToBoolean(element.Attributes[4].GetElementValue()));

			if ((property.Flags & IElementPropertyFlags.UserDefined) != 0)
			{
				if (element.Attributes.Length >= 6)
				{
					property.SetMinValue(Convert.ToDouble(element.Attributes[5].GetElementValue()) != 0.0);
				}

				if (element.Attributes.Length >= 7)
				{
					property.SetMaxValue(Convert.ToDouble(element.Attributes[6].GetElementValue()) != 0.0);
				}
			}

			return property;
		}

		private static IElementProperty AsAttributeInt(IElement element)
		{
			if (element.Attributes.Length < 5)
			{
				return null;
			}

			var property = PropertyFactory.InternalCreateProperty(element, Convert.ToInt32(element.Attributes[4].GetElementValue()));

			if ((property.Flags & IElementPropertyFlags.UserDefined) != 0)
			{
				if (element.Attributes.Length >= 6)
				{
					property.SetMinValue(Convert.ToInt32(element.Attributes[5].GetElementValue()));
				}

				if (element.Attributes.Length >= 7)
				{
					property.SetMaxValue(Convert.ToInt32(element.Attributes[6].GetElementValue()));
				}
			}

			return property;
		}

		private static IElementProperty AsAttributeFloat(IElement element)
		{
			if (element.Attributes.Length < 5)
			{
				return null;
			}

			var property = PropertyFactory.InternalCreateProperty(element, Convert.ToSingle(element.Attributes[4].GetElementValue()));

			if ((property.Flags & IElementPropertyFlags.UserDefined) != 0)
			{
				if (element.Attributes.Length >= 6)
				{
					property.SetMinValue((float)Convert.ToDouble(element.Attributes[5].GetElementValue()));
				}

				if (element.Attributes.Length >= 7)
				{
					property.SetMaxValue((float)Convert.ToDouble(element.Attributes[6].GetElementValue()));
				}
			}

			return property;
		}

		private static IElementProperty AsAttributeDouble(IElement element)
		{
			if (element.Attributes.Length < 5)
			{
				return null;
			}

			var property = PropertyFactory.InternalCreateProperty(element, Convert.ToDouble(element.Attributes[4].GetElementValue()));

			if ((property.Flags & IElementPropertyFlags.UserDefined) != 0)
			{
				if (element.Attributes.Length >= 6)
				{
					property.SetMinValue(Convert.ToDouble(element.Attributes[5].GetElementValue()));
				}

				if (element.Attributes.Length >= 7)
				{
					property.SetMaxValue(Convert.ToDouble(element.Attributes[6].GetElementValue()));
				}
			}

			return property;
		}

		private static IElementProperty AsAttributeDouble2(IElement element)
		{
			if (element.Attributes.Length < 6)
			{
				return null;
			}

			return PropertyFactory.InternalCreateProperty(element, new Vector2
			(
				Convert.ToDouble(element.Attributes[4].GetElementValue()),
				Convert.ToDouble(element.Attributes[5].GetElementValue())
			));
		}

		private static IElementProperty AsAttributeDouble3(IElement element)
		{
			if (element.Attributes.Length < 7)
			{
				return null;
			}

			return PropertyFactory.InternalCreateProperty(element, new Vector3
			(
				Convert.ToDouble(element.Attributes[4].GetElementValue()),
				Convert.ToDouble(element.Attributes[5].GetElementValue()),
				Convert.ToDouble(element.Attributes[6].GetElementValue())
			));
		}

		private static IElementProperty AsAttributeDouble4(IElement element)
		{
			if (element.Attributes.Length < 8)
			{
				return null;
			}

			return PropertyFactory.InternalCreateProperty(element, new Vector4
			(
				Convert.ToDouble(element.Attributes[4].GetElementValue()),
				Convert.ToDouble(element.Attributes[5].GetElementValue()),
				Convert.ToDouble(element.Attributes[6].GetElementValue()),
				Convert.ToDouble(element.Attributes[7].GetElementValue())
			));
		}

		private static IElementProperty AsAttributeDouble4x4(IElement element)
		{
			if (element.Attributes.Length < 20)
			{
				return null;
			}

			return PropertyFactory.InternalCreateProperty(element, new Matrix4x4
			(
				Convert.ToDouble(element.Attributes[4].GetElementValue()),
				Convert.ToDouble(element.Attributes[5].GetElementValue()),
				Convert.ToDouble(element.Attributes[6].GetElementValue()),
				Convert.ToDouble(element.Attributes[7].GetElementValue()),
				Convert.ToDouble(element.Attributes[8].GetElementValue()),
				Convert.ToDouble(element.Attributes[9].GetElementValue()),
				Convert.ToDouble(element.Attributes[10].GetElementValue()),
				Convert.ToDouble(element.Attributes[11].GetElementValue()),
				Convert.ToDouble(element.Attributes[12].GetElementValue()),
				Convert.ToDouble(element.Attributes[13].GetElementValue()),
				Convert.ToDouble(element.Attributes[14].GetElementValue()),
				Convert.ToDouble(element.Attributes[15].GetElementValue()),
				Convert.ToDouble(element.Attributes[16].GetElementValue()),
				Convert.ToDouble(element.Attributes[17].GetElementValue()),
				Convert.ToDouble(element.Attributes[18].GetElementValue()),
				Convert.ToDouble(element.Attributes[19].GetElementValue())
			));
		}

		private static IElementProperty AsAttributeEnum(IElement element)
		{
			if (element.Attributes.Length < 5)
			{
				return null;
			}

			var enumprop = new Enumeration(Convert.ToInt32(element.Attributes[4].GetElementValue()));
			var property = PropertyFactory.InternalCreateProperty(element, enumprop);

			if ((property.Flags & IElementPropertyFlags.UserDefined) != 0)
			{
				if (element.Attributes.Length >= 6)
				{
					enumprop.Flags = element.Attributes[5].GetElementValue().ToString().Split('~');
				}
			}

			return property;
		}

		private static IElementProperty AsAttributeString(IElement element)
		{
			if (element.Attributes.Length < 5)
			{
				return null;
			}

			return PropertyFactory.InternalCreateProperty(element, element.Attributes[4].GetElementValue().ToString());
		}

		private static IElementProperty AsAttributeTime(IElement element)
		{
			if (element.Attributes.Length < 5)
			{
				return null;
			}

			if (element.Attributes[4].Type == IElementAttributeType.Int64)
			{
				return PropertyFactory.InternalCreateProperty(element, new TimeBase(Convert.ToInt64(element.Attributes[4].GetElementValue())));
			}
			else
			{
				return PropertyFactory.InternalCreateProperty(element, new TimeBase(Convert.ToDouble(element.Attributes[4].GetElementValue())));
			}
		}

		private static IElementProperty AsAttributeReference(IElement element)
		{
			if (element.Attributes.Length < 4)
			{
				return null;
			}

			return PropertyFactory.InternalCreateProperty(element, new Reference());
		}

		private static IElementProperty AsAttributeBlob(IElement element)
		{
			if (element.Attributes.Length < 4)
			{
				return null;
			}

			var array = new object[element.Attributes.Length - 4];

			for (int i = 0, k = 4; i < array.Length; ++i, ++k)
			{
				array[i] = element.Attributes[k];
			}

			return PropertyFactory.InternalCreateProperty(element, new BinaryBlob(array));
		}

		private static IElementProperty AsAttributeDistance(IElement element)
		{
			if (element.Attributes.Length < 6)
			{
				return null;
			}

			return PropertyFactory.InternalCreateProperty(element, new Distance
			(
				Convert.ToSingle(element.Attributes[4].GetElementValue()),
				element.Attributes[5].GetElementValue().ToString()
			));
		}

		private static IElementProperty AsAttributeDateTime(IElement element)
		{
			if (element.Attributes.Length < 5)
			{
				return null;
			}

			var strinc = element.Attributes[4].GetElementValue().ToString();
			var splits = strinc.Split(new char[] { ' ', '/', ':', '.' }, StringSplitOptions.RemoveEmptyEntries);

			return PropertyFactory.InternalCreateProperty(element, new DateTime
			(
				splits.Length > 2 ? Int32.Parse(splits[2]) : 0,
				splits.Length > 1 ? Int32.Parse(splits[1]) : 0,
				splits.Length > 0 ? Int32.Parse(splits[0]) : 0,
				splits.Length > 3 ? Int32.Parse(splits[3]) : 0,
				splits.Length > 4 ? Int32.Parse(splits[4]) : 0,
				splits.Length > 5 ? Int32.Parse(splits[5]) : 0,
				splits.Length > 6 ? Int32.Parse(splits[6]) : 0
			));
		}

		public static IElementProperty AsElementProperty(IElement element)
		{
			if (element is null || element.Attributes.Length < 4)
			{
				return null;
			}

			if (PropertyFactory.ms_typeMapper.TryGetValue(element.Attributes[2].GetElementValue().ToString(), out var type))
			{
				return PropertyFactory.ms_activators[type].Invoke(element);
			}
			else
			{
				if (PropertyFactory.ms_typeMapper.TryGetValue(element.Attributes[1].GetElementValue().ToString(), out type))
				{
					return PropertyFactory.ms_activators[type].Invoke(element);
				}
				else
				{
					return PropertyFactory.ms_activators[IElementPropertyType.Undefined].Invoke(element);
				}
			}
		}
	}
}
