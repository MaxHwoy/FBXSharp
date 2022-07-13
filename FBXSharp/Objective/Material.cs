using FBXSharp.Core;
using FBXSharp.ValueTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FBXSharp.Objective
{
	public class Material : FBXObject
	{
		public struct Channel
		{
			public string Name { get; }
			public Texture Texture { get; }

			public Channel(string name, Texture texture)
			{
				this.Name = name ?? String.Empty;
				this.Texture = texture;
			}

			public override string ToString() => this.Name;
		}

		private readonly List<Channel> m_channels;
		private readonly ReadOnlyCollection<Channel> m_readonly;
		private string m_shadingModel;

		public static readonly FBXObjectType FType = FBXObjectType.Material;

		public static readonly FBXClassType FClass = FBXClassType.Material;

		public override FBXObjectType Type => Material.FType;

		public override FBXClassType Class => Material.FClass;

		public ReadOnlyCollection<Channel> Channels => this.m_readonly;

		public string ShadingModel
		{
			get => this.m_shadingModel;
			set => this.m_shadingModel = value ?? String.Empty;
		}

		public bool MultiLayer { get; set; }

		public ColorRGB? DiffuseColor
		{
			get => this.InternalGetColor(nameof(this.DiffuseColor));
			set => this.InternalSetColor(nameof(this.DiffuseColor), value, "Color", String.Empty, IElementPropertyFlags.Animatable);
		}
		public ColorRGB? SpecularColor
		{
			get => this.InternalGetColor(nameof(this.SpecularColor));
			set => this.InternalSetColor(nameof(this.SpecularColor), value, "Color", String.Empty, IElementPropertyFlags.Animatable);
		}
		public ColorRGB? ReflectionColor
		{
			get => this.InternalGetColor(nameof(this.ReflectionColor));
			set => this.InternalSetColor(nameof(this.ReflectionColor), value, "Color", String.Empty, IElementPropertyFlags.Animatable);
		}
		public ColorRGB? AmbientColor
		{
			get => this.InternalGetColor(nameof(this.AmbientColor));
			set => this.InternalSetColor(nameof(this.AmbientColor), value, "Color", String.Empty, IElementPropertyFlags.Animatable);
		}
		public ColorRGB? EmissiveColor
		{
			get => this.InternalGetColor(nameof(this.EmissiveColor));
			set => this.InternalSetColor(nameof(this.EmissiveColor), value, "Color", String.Empty, IElementPropertyFlags.Animatable);
		}
		public ColorRGB? TransparentColor
		{
			get => this.InternalGetColor(nameof(this.TransparentColor));
			set => this.InternalSetColor(nameof(this.TransparentColor), value, "Color", String.Empty, IElementPropertyFlags.Animatable);
		}

		public double? DiffuseFactor
		{
			get => this.InternalGetPrimitive<double>(nameof(this.DiffuseFactor), IElementPropertyType.Double);
			set => this.InternalSetPrimitive<double>(nameof(this.DiffuseFactor), IElementPropertyType.Double, value, "Number", String.Empty, IElementPropertyFlags.Animatable);
		}
		public double? SpecularFactor
		{
			get => this.InternalGetPrimitive<double>(nameof(this.SpecularFactor), IElementPropertyType.Double);
			set => this.InternalSetPrimitive<double>(nameof(this.SpecularFactor), IElementPropertyType.Double, value, "Number", String.Empty, IElementPropertyFlags.Animatable);
		}
		public double? ReflectionFactor
		{
			get => this.InternalGetPrimitive<double>(nameof(this.ReflectionFactor), IElementPropertyType.Double);
			set => this.InternalSetPrimitive<double>(nameof(this.ReflectionFactor), IElementPropertyType.Double, value, "Number", String.Empty, IElementPropertyFlags.Animatable);
		}
		public double? Shininess
		{
			get => this.InternalGetPrimitive<double>(nameof(this.Shininess), IElementPropertyType.Double);
			set => this.InternalSetPrimitive<double>(nameof(this.Shininess), IElementPropertyType.Double, value, "Number", String.Empty, IElementPropertyFlags.Animatable);
		}
		public double? ShininessExponent
		{
			get => this.InternalGetPrimitive<double>(nameof(this.ShininessExponent), IElementPropertyType.Double);
			set => this.InternalSetPrimitive<double>(nameof(this.ShininessExponent), IElementPropertyType.Double, value, "Number", String.Empty, IElementPropertyFlags.Animatable);
		}
		public double? AmbientFactor
		{
			get => this.InternalGetPrimitive<double>(nameof(this.AmbientFactor), IElementPropertyType.Double);
			set => this.InternalSetPrimitive<double>(nameof(this.AmbientFactor), IElementPropertyType.Double, value, "Number", String.Empty, IElementPropertyFlags.Animatable);
		}
		public double? BumpFactor
		{
			get => this.InternalGetPrimitive<double>(nameof(this.BumpFactor), IElementPropertyType.Double);
			set => this.InternalSetPrimitive<double>(nameof(this.BumpFactor), IElementPropertyType.Double, value, "double", "Number", IElementPropertyFlags.Animatable);
		}
		public double? EmissiveFactor
		{
			get => this.InternalGetPrimitive<double>(nameof(this.EmissiveFactor), IElementPropertyType.Double);
			set => this.InternalSetPrimitive<double>(nameof(this.EmissiveFactor), IElementPropertyType.Double, value, "Number", String.Empty, IElementPropertyFlags.Animatable);
		}
		public double? TransparencyFactor
		{
			get => this.InternalGetPrimitive<double>(nameof(this.TransparencyFactor), IElementPropertyType.Double);
			set => this.InternalSetPrimitive<double>(nameof(this.TransparencyFactor), IElementPropertyType.Double, value, "Number", String.Empty, IElementPropertyFlags.Animatable);
		}

		internal Material(IElement element, IScene scene) : base(element, scene)
		{
			this.m_channels = new List<Channel>();
			this.m_readonly = new ReadOnlyCollection<Channel>(this.m_channels);
			this.m_shadingModel = "Phong";
			this.MultiLayer = false; // ??

			if (element is null)
			{
				return;
			}

			var shading = element.FindChild("ShadingModel");

			if (!(shading is null) && shading.Attributes.Length > 0 && shading.Attributes[0].Type == IElementAttributeType.String)
			{
				this.m_shadingModel = shading.Attributes[0].GetElementValue().ToString() ?? String.Empty;
			}

			var multi = element.FindChild("MultiLayer");

			if (!(multi is null) && multi.Attributes.Length > 0)
			{
				this.MultiLayer = Convert.ToBoolean(multi.Attributes[0].GetElementValue());
			}
		}

		internal void InternalSetChannel(in Channel channel) => this.m_channels.Add(channel);

		public void AddChannel(Channel channel)
		{
			this.AddChannelAt(channel, this.m_channels.Count);
		}
		public void RemoveChannel(Channel channel)
		{
			this.m_channels.Remove(channel);
		}
		public void AddChannelAt(Channel channel, int index)
		{
			if (String.IsNullOrEmpty(channel.Name))
			{
				throw new ArgumentNullException("Channel name cannot be null or empty");
			}

			if (channel.Texture is null)
			{
				throw new ArgumentNullException("Texture in the channel passed cannot be null");
			}

			if (channel.Texture.Scene != this.Scene)
			{
				throw new Exception("Texture in the channel passed should share same scene with material");
			}

			if (index < 0 || index > this.m_channels.Count)
			{
				throw new ArgumentOutOfRangeException("Index should be in range 0 to channel count inclusively");
			}

			if (this.m_channels.FindIndex(_ => _.Name == channel.Name) >= 0)
			{
				return;
			}

			this.m_channels.Insert(index, channel);
		}
		public void RemoveChannelAt(int index)
		{
			if (index < 0 || index >= this.m_channels.Count)
			{
				throw new ArgumentOutOfRangeException("Index should be in 0 to channel count range");
			}

			this.m_channels.RemoveAt(index);
		}

		public override Connection[] GetConnections()
		{
			if (this.m_channels.Count == 0)
			{
				return Array.Empty<Connection>();
			}

			var connections = new Connection[this.m_channels.Count];

			for (int i = 0; i < connections.Length; ++i)
			{
				connections[i] = new Connection
				(
					Connection.ConnectionType.Property,
					this.m_channels[i].Texture.GetHashCode(),
					this.GetHashCode(),
					ElementaryFactory.GetElementAttribute(this.m_channels[i].Name)
				);
			}

			return connections;
		}

		public override void ResolveLink(FBXObject linker, IElementAttribute attribute)
		{
			if (linker.Class == FBXClassType.Texture && linker.Type == FBXObjectType.Texture)
			{
				if (attribute is null || attribute.Type != IElementAttributeType.String)
				{
					return;
				}

				this.AddChannel(new Channel(attribute.GetElementValue().ToString(), linker as Texture));
			}
		}

		public override IElement AsElement(bool binary)
		{
			var elements = new IElement[4];

			elements[0] = Element.WithAttribute("Version", ElementaryFactory.GetElementAttribute(102));
			elements[1] = Element.WithAttribute("ShadingModel", ElementaryFactory.GetElementAttribute(this.m_shadingModel));
			elements[2] = Element.WithAttribute("MultiLayer", ElementaryFactory.GetElementAttribute(this.MultiLayer ? 1 : 0));
			elements[3] = this.BuildProperties70();

			return new Element(this.Class.ToString(), elements, this.BuildAttributes("Material", String.Empty, binary));
		}
	}

	public class MaterialBuilder : BuilderBase
	{
		public enum ColorType
		{
			DiffuseColor,
			SpecularColor,
			ReflectionColor,
			AmbientColor,
			EmissiveColor,
			TransparentColor,
		}

		public enum FactorType
		{
			DiffuseFactor,
			SpecularFactor,
			ReflectionFactor,
			Shininess,
			ShininessExponent,
			AmbientFactor,
			BumpFactor,
			EmissiveFactor,
			TransparencyFactor,
		}

		public enum ChannelType
		{
			DiffuseColor,
			NormalMap,
			HeightMap,
			OcclusionMap,
			ReflectionColor,
			AmbientColor,
			EmissiveColor,
			SpecularColor,
			TransparentColor,
		}

		private readonly List<Material.Channel> m_channels;
		private string m_shadingModel;
		private bool m_multiLayer;

		public MaterialBuilder(Scene scene) : base(scene)
		{
			this.m_channels = new List<Material.Channel>();
			this.m_shadingModel = "Phong";
		}

		public Material BuildMaterial()
		{
			var material = this.m_scene.CreateMaterial();

			material.Name = this.m_name;
			material.ShadingModel = this.m_shadingModel;
			material.MultiLayer = this.m_multiLayer;

			foreach (var channel in this.m_channels)
			{
				material.InternalSetChannel(channel);
			}

			foreach (var property in this.m_properties)
			{
				material.AddProperty(property);
			}

			return material;
		}

		public MaterialBuilder WithName(string name)
		{
			this.SetObjectName(name);
			return this;
		}

		public MaterialBuilder WithFBXProperty<T>(string name, T value, bool isUser = false)
		{
			this.SetFBXProperty(name, value, isUser);
			return this;
		}
		public MaterialBuilder WithFBXProperty<T>(string name, T value, IElementPropertyFlags flags)
		{
			this.SetFBXProperty(name, value, flags);
			return this;
		}
		public MaterialBuilder WithFBXProperty<T>(FBXProperty<T> property)
		{
			this.SetFBXProperty(property);
			return this;
		}

		public MaterialBuilder WithShadingModel(string shading)
		{
			this.m_shadingModel = shading ?? "Phong";
			return this;
		}
		public MaterialBuilder WithMultiLayer(bool multiLayer)
		{
			this.m_multiLayer = multiLayer;
			return this;
		}

		public MaterialBuilder WithChannel(ChannelType type, Texture texture)
		{
			return this.WithChannel(type.ToString(), texture);
		}
		public MaterialBuilder WithChannel(string name, Texture texture)
		{
			return this.WithChannel(new Material.Channel(name, texture));
		}
		public MaterialBuilder WithChannel(in Material.Channel channel)
		{
			if (channel.Texture is null)
			{
				throw new ArgumentNullException($"Channel {channel.Name} cannot have null texture");
			}

			if (channel.Texture.Scene != this.m_scene)
			{
				throw new Exception("Texture should share same scene with material");
			}

			this.m_channels.Add(channel);
			return this;
		}

		public MaterialBuilder WithColor(ColorType type, ColorRGB color)
		{
			return this.WithColor(type.ToString(), color);
		}
		public MaterialBuilder WithColor(string name, ColorRGB color)
		{
			var flag = IElementPropertyFlags.Imported | IElementPropertyFlags.Animatable;
			var prop = new FBXProperty<Vector3>("Color", String.Empty, name, flag, color);

			this.SetFBXProperty(prop);
			return this;
		}

		public MaterialBuilder WithFactor(FactorType type, double factor)
		{
			return this.WithFactor(type.ToString(), factor);
		}
		public MaterialBuilder WithFactor(string name, double factor)
		{
			var flag = IElementPropertyFlags.Imported | IElementPropertyFlags.Animatable;
			var p2nd = name.StartsWith("Bump") ? "Number" : String.Empty;
			var p1st = String.IsNullOrEmpty(p2nd) ? "Number" : "double";
			var prop = new FBXProperty<double>(p1st, p2nd, name, flag, factor);

			this.SetFBXProperty(prop);
			return this;
		}
	}
}
