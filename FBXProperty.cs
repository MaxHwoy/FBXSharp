using FBXSharp.Core;
using FBXSharp.ValueTypes;
using System;
using System.Collections.Generic;

namespace FBXSharp
{
	public abstract class FBXPropertyBase : IElementProperty
	{
		private static readonly Dictionary<Type, IElementPropertyType> ms_typeMapper = new Dictionary<Type, IElementPropertyType>()
		{
			{ typeof(sbyte), IElementPropertyType.SByte },
			{ typeof(byte), IElementPropertyType.Byte },
			{ typeof(short), IElementPropertyType.Short },
			{ typeof(ushort), IElementPropertyType.UShort },
			{ typeof(uint), IElementPropertyType.UInt },
			{ typeof(long), IElementPropertyType.Long },
			{ typeof(ulong), IElementPropertyType.ULong },
			{ typeof(Half), IElementPropertyType.Half },
			{ typeof(bool), IElementPropertyType.Bool },
			{ typeof(int), IElementPropertyType.Int },
			{ typeof(float), IElementPropertyType.Float },
			{ typeof(double), IElementPropertyType.Double },
			{ typeof(Vector2), IElementPropertyType.Double2 },
			{ typeof(Vector3), IElementPropertyType.Double3 },
			{ typeof(Vector4), IElementPropertyType.Double4 },
			{ typeof(Matrix4x4), IElementPropertyType.Double4x4 },
			{ typeof(Enumeration), IElementPropertyType.Enum },
			{ typeof(string), IElementPropertyType.String },
			{ typeof(TimeBase), IElementPropertyType.Time },
			{ typeof(Reference), IElementPropertyType.Reference },
			{ typeof(BinaryBlob), IElementPropertyType.Blob },
			{ typeof(Distance), IElementPropertyType.Distance },
			{ typeof(DateTime), IElementPropertyType.DateTime },
		};

		private string m_name;

		public IElementPropertyType Type { get; }

		public IElementPropertyFlags Flags { get; set; }
		
		public string Primary { get; }

		public string Secondary { get; }

		public bool SupportsMinMax { get; }

		public string Name
		{
			get => this.m_name;
			set => this.m_name = value ?? String.Empty;
		}

		public FBXPropertyBase(IElementPropertyType type, string primary, string secondary)
		{
			this.Type = type;
			this.Name = String.Empty;
			this.Primary = primary ?? String.Empty;
			this.Secondary = secondary ?? String.Empty;
			this.SupportsMinMax = FBXPropertyBase.TypeSupportsMinMax(type);
		}

		public abstract Type GetPropertyType();

		public abstract object GetPropertyValue();

		public abstract object GetPropertyMin();

		public abstract object GetPropertyMax();

		public abstract void SetPropertyValue(object value);

		public abstract void SetPropertyMin(object value);

		public abstract void SetPropertyMax(object value);

		public static IElementPropertyType MapType<T>()
		{
			return FBXPropertyBase.MapType(typeof(T));
		}

		public static IElementPropertyType MapType(Type type)
		{
			if (FBXPropertyBase.ms_typeMapper.TryGetValue(type, out var result))
			{
				return result;
			}
			else
			{
				return IElementPropertyType.Undefined;
			}
		}

		public static void RegisterType<T>(IElementPropertyType element)
		{
			FBXPropertyBase.ms_typeMapper[typeof(T)] = element;
		}

		public static void RegisterType(Type type, IElementPropertyType element)
		{
			FBXPropertyBase.ms_typeMapper[type] = element;
		}

		public static bool TypeSupportsMinMax(IElementPropertyType type)
		{
			switch (type)
			{
				case IElementPropertyType.Double4:
				case IElementPropertyType.Double4x4:
				case IElementPropertyType.String:
				case IElementPropertyType.Time:
				case IElementPropertyType.Reference:
				case IElementPropertyType.Blob:
				case IElementPropertyType.Distance:
				case IElementPropertyType.DateTime:
					return false;

				default:
					return true;
			}
		}
	}

	public class FBXProperty<T> : FBXPropertyBase, IGenericProperty<T>
	{
		private T m_min;
		private T m_max;

		public T Value { get; set; }

		public FBXProperty(string primary, string secondary) : base(FBXPropertyBase.MapType<T>(), primary, secondary)
		{
		}

		public FBXProperty(string primary, string secondary, string name) : this(primary, secondary)
		{
			this.Name = name;
		}

		public FBXProperty(string primary, string secondary, string name, IElementPropertyFlags flags) : this(primary, secondary)
		{
			this.Name = name;
			this.Flags = flags;
		}

		public FBXProperty(string primary, string secondary, string name, IElementPropertyFlags flags, T value) : this(primary, secondary)
		{
			this.Name = name;
			this.Flags = flags;
			this.Value = value;
		}

		public bool GetMinValue(out T min)
		{
			if (this.SupportsMinMax)
			{
				min = this.m_min;
				return true;
			}
			else
			{
				min = default;
				return false;
			}
		}

		public bool GetMaxValue(out T max)
		{
			if (this.SupportsMinMax)
			{
				max = this.m_max;
				return true;
			}
			else
			{
				max = default;
				return false;
			}
		}

		public void SetMinValue(in T min)
		{
			if (this.SupportsMinMax)
			{
				this.m_min = min;
			}
		}

		public void SetMaxValue(in T max)
		{
			if (this.SupportsMinMax)
			{
				this.m_max = max;
			}
		}

		public override Type GetPropertyType()
		{
			return typeof(T);
		}

		public override object GetPropertyValue()
		{
			return this.Value;
		}

		public override object GetPropertyMin()
		{
			return this.GetMinValue(out var min) ? (object)min : null;
		}

		public override object GetPropertyMax()
		{
			return this.GetMaxValue(out var max) ? (object)max : null;
		}

		public override void SetPropertyValue(object value)
		{
			if (value is T obj)
			{
				this.Value = obj;
			}
		}

		public override void SetPropertyMin(object value)
		{
			if (this.SupportsMinMax && value is T obj)
			{
				this.m_min = obj;
			}
		}

		public override void SetPropertyMax(object value)
		{
			if (this.SupportsMinMax && value is T obj)
			{
				this.m_max = obj;
			}
		}

		public override string ToString() => this.Name.ToString();
	}
}
