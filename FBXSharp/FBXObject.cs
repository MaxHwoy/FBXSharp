using FBXSharp.Core;
using FBXSharp.ValueTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FBXSharp
{
	public enum FBXObjectType : int
	{
		Root,
		Geometry,
		Shape,
		Material,
		Mesh,
		Texture,
		LimbNode,
		NullNode,
		NodeAttribute,
		Cluster,
		Skin,
		BlendShape,
		BlendShapeChannel,
		AnimationStack,
		AnimationLayer,
		AnimationCurve,
		AnimationCurveNode,
		Pose,
		Deformer,
		Model,
		Video,
		GlobalSettings,
	}

	public abstract class FBXObject
	{
		private readonly List<IElementProperty> m_properties;
		private readonly ReadOnlyCollection<IElementProperty> m_readProps;
		private long m_id;

		public abstract FBXObjectType Type { get; }
		public IScene Scene { get; }

		public long ID => this.m_id;
		public string Name { get; set; }

		public ReadOnlyCollection<IElementProperty> Properties => this.m_readProps;

		internal FBXObject(IElement element, IScene scene)
		{
			this.m_id = 0;
			this.Scene = scene;
			this.m_properties = new List<IElementProperty>();
			this.m_readProps = new ReadOnlyCollection<IElementProperty>(this.m_properties);

			if (element is null)
			{
				return;
			}

			this.FromElement(element);
		}

		internal void SetNewID(long id) => this.m_id = id;

		protected void FromElement(IElement element)
		{
			if (element.Attributes.Length > 0)
			{
				if (element.Attributes[0].Type == IElementAttributeType.Int64)
				{
					this.m_id = (long)element.Attributes[0].GetElementValue();
				}

				if (element.Attributes.Length > 1)
				{
					if (element.Attributes[1].Type == IElementAttributeType.String)
					{
						this.Name = element.Attributes[1].GetElementValue().ToString();
					}
				}
			}

			var properties = element.FindChild("Properties70");

			if (properties is null)
			{
				return;
			}

			for (int i = 0; i < properties.Children.Length; ++i)
			{
				var property = properties.Children[i];

				if (property.Name == "P" && property.Attributes.Length != 0)
				{
					this.AddProperty(PropertyFactory.AsElementProperty(property));
				}
			}
		}

		protected Enumeration InternalGetEnumeration(string name)
		{
			var attribute = this.GetProperty(name);

			if (attribute is null || attribute.Type != IElementPropertyType.Enum)
			{
				return null;
			}

			return attribute.GetPropertyValue() as Enumeration;
		}

		protected void InternalSetEnumeration(string name, Enumeration value, string primary, string secondary)
		{
			var attribute = this.GetProperty(name);

			if (attribute is null)
			{
				if (!(value is null))
				{
					this.AddProperty(new FBXProperty<Enumeration>(primary, secondary, name, IElementPropertyFlags.Imported, value));
				}
			}
			else if (attribute.Type == IElementPropertyType.Enum)
			{
				if (value is null)
				{
					this.RemoveProperty(attribute);
				}
				else
				{
					attribute.SetPropertyValue(value);
				}
			}
		}

		protected ColorRGB? InternalGetColor(string name)
		{
			var attribute = this.GetProperty(name);

			if (attribute is null)
			{
				return null;
			}

			if (attribute.Type == IElementPropertyType.Double3)
			{
				return new ColorRGB((Vector3)attribute.GetPropertyValue());
			}

			if (attribute.Type == IElementPropertyType.Double4)
			{
				return new ColorRGB((Vector4)attribute.GetPropertyValue());
			}

			return null;
		}

		protected void InternalSetColor(string name, ColorRGB? value, string primary, string secondary, IElementPropertyFlags flags)
		{
			var attribute = this.GetProperty(name);

			if (attribute is null)
			{
				if (value.HasValue)
				{
					this.AddProperty(new FBXProperty<Vector3>(primary, secondary, name, flags | IElementPropertyFlags.Imported, value.Value));
				}
			}
			else if (attribute.Type == IElementPropertyType.Double3)
			{
				if (value.HasValue)
				{
					attribute.SetPropertyValue(value);
				}
				else
				{
					this.RemoveProperty(attribute);
				}
			}
		}

		protected T? InternalGetPrimitive<T>(string name, IElementPropertyType type) where T : struct
		{
			var attribute = this.GetProperty(name);

			if (attribute is null || attribute.Type != type)
			{
				return null;
			}

			return (T)attribute.GetPropertyValue();
		}

		protected void InternalSetPrimitive<T>(string name, IElementPropertyType type, T? value, string primary, string secondary, IElementPropertyFlags flags = IElementPropertyFlags.None) where T : struct
		{
			var attribute = this.GetProperty(name);

			if (attribute is null)
			{
				if (value.HasValue)
				{
					this.AddProperty(new FBXProperty<T>(primary, secondary, name, flags | IElementPropertyFlags.Imported, value.Value));
				}
			}
			else if (attribute.Type == type)
			{
				if (value.HasValue)
				{
					attribute.SetPropertyValue(value);
				}
				else
				{
					this.RemoveProperty(attribute);
				}
			}
		}

		public IElementProperty GetProperty(string name)
		{
			return this.m_properties.Find(_ => _.Name == name);
		}

		public void AddProperty(IElementProperty attribute)
		{
			if (attribute is null)
			{
				return;
			}

			if (this.m_properties.Find(_ => _.Name == attribute.Name) is null)
			{
				this.m_properties.Add(attribute);
			}
		}
		
		public void RemoveProperty(IElementProperty attribute)
		{
			_ = this.m_properties.Remove(attribute);
		}

		public void RemoveProperty(int index)
		{
			if (index >= 0 && index < this.m_properties.Count)
			{
				this.m_properties.RemoveAt(index);
			}
		}

		public override string ToString() => $"{(String.IsNullOrEmpty(this.Name) ? this.m_id.ToString() : this.Name)} : {this.GetType().Name}";

		/*
		public FObject ResolveObjectLink(int index)
		{

		}
		public FObject ResolveObjectLink(FObjectType type, string property, int idx)
		{

		}
		public FObject ResolveObjectLinkReverse(FObjectType type)
		{

		}
		public FObject GetParent()
		{

		}

		public RotationOrder GetRotationOrder()
		{

		}
		public Vector3 GetRotationOffset()
		{

		}
		public Vector3 GetRotationPivot()
		{

		}
		public Vector3 GetPostRotation()
		{

		}
		public Vector3 GetScalingOffset()
		{

		}
		public Vector3 GetScalingPivot()
		{

		}
		public Vector3 GetPreRotation()
		{

		}
		public Vector3 GetLocalTranslation()
		{

		}
		public Vector3 GetLocalRotation()
		{

		}
		public Vector3 GetLocalScaling()
		{

		}
		public Matrix4x4 GetGlobalTransform()
		{

		}
		public Matrix4x4 GetLocalTransform()
		{

		}
		public Matrix4x4 EvalluateLocal(Vector3 translation, Vector3 rotation)
		{

		}
		public Matrix4x4 EvalluateLocal(Vector3 translation, Vector3 rotation, Vector3 scale)
		{

		}

		public T ResolveObjectLink<T>(int index) where T : FObject
		{
			// some generic shenanigans?
		}
		*/
	}

	public abstract class BuilderBase
	{
		protected readonly IScene m_scene;
		protected readonly List<FBXPropertyBase> m_properties;
		protected string m_name;

		public BuilderBase(IScene scene)
		{
			this.m_scene = scene;
			this.m_properties = new List<FBXPropertyBase>();
		}

		protected void SetObjectName(string name)
		{
			this.m_name = name;
		}

		protected void SetFBXProperty<T>(string name, T value, bool isUser)
		{
			var flag = isUser
				? IElementPropertyFlags.Animatable | IElementPropertyFlags.Animated | IElementPropertyFlags.UserDefined
				: IElementPropertyFlags.None;

			this.SetFBXProperty(name, value, flag);
		}
		protected void SetFBXProperty<T>(string name, T value, IElementPropertyFlags flags)
		{
			var type = Type.GetTypeCode(typeof(T));
			var flag = flags | IElementPropertyFlags.Imported;

			string primary = null;

			switch (type)
			{
				case TypeCode.Boolean: primary = "bool"; break;
				case TypeCode.Char: primary = "Short"; break;
				case TypeCode.SByte: primary = "Byte"; break;
				case TypeCode.Byte: primary = "UByte"; break;
				case TypeCode.Int16: primary = "Short"; break;
				case TypeCode.UInt16: primary = "UShort"; break;
				case TypeCode.Int32: primary = "int"; break;
				case TypeCode.UInt32: primary = "UInteger"; break;
				case TypeCode.Int64: primary = "LongLong"; break;
				case TypeCode.UInt64: primary = "ULongLong"; break;
				case TypeCode.Single: primary = "float"; break;
				case TypeCode.Double: primary = "double"; break;
				case TypeCode.DateTime: primary = "DateTime"; break;
				case TypeCode.String: primary = "KString"; break;
			}

			if (primary is null)
			{
				if (typeof(T) == typeof(Half))
				{
					primary = "HalfFloat";
				}
				else if (typeof(T) == typeof(Vector2))
				{
					primary = "Vector2D";
				}
				else if (typeof(T) == typeof(Vector3))
				{
					primary = "Vector3D";
				}
				else if (typeof(T) == typeof(Vector4))
				{
					primary = "Vector4D";
				}
				else if (typeof(T) == typeof(Matrix4x4))
				{
					primary = "matrix4x4";
				}
				else if (typeof(T) == typeof(Enumeration))
				{
					primary = "enum";
				}
				else if (typeof(T) == typeof(TimeBase))
				{
					primary = "KTime";
				}
				else if (typeof(T) == typeof(Reference))
				{
					primary = "Reference";
				}
				else if (typeof(T) == typeof(BinaryBlob))
				{
					primary = "Blob";
				}
				else if (typeof(T) == typeof(Distance))
				{
					primary = "Distance";
				}
				else
				{
					primary = "object";
				}
			}

			this.m_properties.Add(new FBXProperty<T>(primary, String.Empty, name, flag, value));
		}
		protected void SetFBXProperty<T>(FBXProperty<T> property)
		{
			property.Flags |= IElementPropertyFlags.Imported;
			this.m_properties.Add(property);
		}
	}
}
