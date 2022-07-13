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
		Clip,
		Texture,
		Material,
		Geometry,
		Shape,
		Model,
		Mesh,
		Null,
		LimbNode,
		Light,
		Camera,
		BindPose,
		Skin,
		Cluster,
		BlendShape,
		BlendShapeChannel,
		AnimationStack,
		AnimationLayer,
		AnimationCurve,
		AnimationCurveNode,
		GlobalSettings,
		Template,
	}

	public enum FBXClassType : int
	{
		Video,
		Texture,
		Material,
		Geometry,
		Model,
		NodeAttribute,
		Pose,
		Deformer,
		AnimationStack,
		AnimationLayer,
		AnimationCurve,
		AnimationCurveNode,
		GlobalSettings,
		Template,
		Count,
	}

	public abstract class FBXObject
	{
		private readonly List<IElementProperty> m_properties;
		private IScene m_scene;

		public abstract FBXObjectType Type { get; }

		public abstract FBXClassType Class { get; }

		public IScene Scene => this.m_scene;

		public string Name { get; set; }

		public IReadOnlyList<IElementProperty> Properties => this.m_properties;

		public FBXObject(IElement element, IScene scene)
		{
			this.m_scene = scene;
			this.m_properties = new List<IElementProperty>();

			if (element is null)
			{
				return;
			}

			this.FromElement(element);
		}

		protected void FromElement(IElement element)
		{
			if (element.Attributes.Length > 1)
			{
				if (element.Attributes[1].Type == IElementAttributeType.String)
				{
					this.Name = element.Attributes[1].GetElementValue().ToString().Substring("::").Substring("\x00\x01").Trim('\x00');
				}
			}

			this.ParseProperties70(element);
		}

		protected void ParseProperties70(IElement element)
		{
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

		protected IElementAttribute[] BuildAttributes(string className, string typeName, bool binary)
		{
			if (binary)
			{
				return new IElementAttribute[3]
				{
					ElementaryFactory.GetElementAttribute((long)this.GetHashCode()),
					ElementaryFactory.GetElementAttribute($"{this.Name}\x00\x01{className}"),
					ElementaryFactory.GetElementAttribute(typeName ?? String.Empty),
				};
			}
			else
			{
				return new IElementAttribute[3]
				{
					ElementaryFactory.GetElementAttribute((long)this.GetHashCode()),
					ElementaryFactory.GetElementAttribute($"{this.Name}::{this.Class}"),
					ElementaryFactory.GetElementAttribute(typeName ?? String.Empty),
				};
			}
		}

		protected IElement BuildProperties70()
		{
			var children = new IElement[this.m_properties.Count];

			for (int i = 0; i < children.Length; ++i)
			{
				children[i] = PropertyFactory.AsElement(this.m_properties[i]);
			}

			return new Element("Properties70", children, null);
		}

		protected bool InternalGetEnumType<T>(string name, out T value) where T : Enum
		{
			var enumeration = this.InternalGetEnumeration(name);

			if (enumeration is null)
			{
				value = default;
				return false;
			}
			else
			{
				value = (T)Enum.ToObject(typeof(T), enumeration.Value);
				return true;
			}
		}

		protected void InternalSetEnumType(string name, bool useValue, int value, string primary, string secondary)
		{
			if (useValue)
			{
				this.InternalSetEnumeration(name, new Enumeration(value), primary, secondary);
			}
			else
			{
				this.InternalSetEnumeration(name, null, primary, secondary);
			}
		}

		protected Enumeration InternalGetEnumeration(string name)
		{
			var attribute = this.GetProperty(name, true);

			if (attribute is null || attribute.Type != IElementPropertyType.Enum)
			{
				return null;
			}

			return attribute.GetPropertyValue() as Enumeration;
		}

		protected void InternalSetEnumeration(string name, Enumeration value, string primary, string secondary)
		{
			var attribute = this.GetProperty(name, false);

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
			var attribute = this.GetProperty(name, true);

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
			var attribute = this.GetProperty(name, false);

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
			var attribute = this.GetProperty(name, true);

			if (attribute is null || attribute.Type != type)
			{
				return null;
			}

			return (T)attribute.GetPropertyValue();
		}

		protected T InternalGetReference<T>(string name, IElementPropertyType type) where T : class
		{
			var attribute = this.GetProperty(name, true);

			if (attribute is null || attribute.Type != type)
			{
				return null;
			}

			return (T)attribute.GetPropertyValue();
		}

		protected void InternalSetPrimitive<T>(string name, IElementPropertyType type, T? value, string primary, string secondary, IElementPropertyFlags flags = IElementPropertyFlags.None) where T : struct
		{
			var attribute = this.GetProperty(name, false);

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

		protected void InternalSetReference<T>(string name, IElementPropertyType type, T value, string primary, string secondary, IElementPropertyFlags flags = IElementPropertyFlags.None) where T : class
		{
			var attribute = this.GetProperty(name, false);

			if (attribute is null)
			{
				if (!(value is null))
				{
					this.AddProperty(new FBXProperty<T>(primary, secondary, name, flags | IElementPropertyFlags.Imported, value));
				}
			}
			else if (attribute.Type == type)
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

		public IElementProperty GetProperty(string name, bool checkTemplate = true)
		{
			var property = this.m_properties.Find(_ => _.Name == name);

			if (checkTemplate && property is null && this.Type != FBXObjectType.Template)
			{
				property = this.Scene.GetTemplateObject(this.Class)?.GetProperty(name, false);
			}

			return property;
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

		public void RemoveProperty(string name)
		{
			this.RemoveProperty(this.m_properties.FindIndex(_ => _.Name == name));
		}

		public void RemoveAllProperties() => this.m_properties.Clear();

		public virtual Connection[] GetConnections() => Array.Empty<Connection>();

		public virtual void ResolveLink(FBXObject linker, IElementAttribute attribute)
		{
		}

		public virtual void Destroy()
		{
			if (!(this.m_scene is null))
			{
				var scene = this.m_scene;
				this.m_scene = null;
				scene.DestroyFBXObject(this);
			}
		}

		public abstract IElement AsElement(bool binary);

		public override string ToString() => $"{(String.IsNullOrEmpty(this.Name) ? this.GetHashCode().ToString() : this.Name)} : {this.GetType().Name}";
	}

	public abstract class BuilderBase
	{
		protected readonly Scene m_scene;
		protected readonly List<FBXPropertyBase> m_properties;
		protected string m_name;

		public BuilderBase(Scene scene)
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
