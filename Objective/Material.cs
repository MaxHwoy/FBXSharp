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

		public override FBXObjectType Type => Material.FType;

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
			set => this.InternalSetColor(nameof(this.DiffuseColor), value, "Color", String.Empty);
		}
		public ColorRGB? SpecularColor
		{
			get => this.InternalGetColor(nameof(this.SpecularColor));
			set => this.InternalSetColor(nameof(this.SpecularColor), value, "Color", String.Empty);
		}
		public ColorRGB? ReflectionColor
		{
			get => this.InternalGetColor(nameof(this.ReflectionColor));
			set => this.InternalSetColor(nameof(this.ReflectionColor), value, "Color", String.Empty);
		}
		public ColorRGB? AmbientColor
		{
			get => this.InternalGetColor(nameof(this.AmbientColor));
			set => this.InternalSetColor(nameof(this.AmbientColor), value, "Color", String.Empty);
		}
		public ColorRGB? EmissiveColor
		{
			get => this.InternalGetColor(nameof(this.EmissiveColor));
			set => this.InternalSetColor(nameof(this.EmissiveColor), value, "Color", String.Empty);
		}
		public ColorRGB? TransparentColor
		{
			get => this.InternalGetColor(nameof(this.TransparentColor));
			set => this.InternalSetColor(nameof(this.TransparentColor), value, "Color", String.Empty);
		}

		public double? DiffuseFactor
		{
			get => this.InternalGetPrimitive<double>(nameof(this.DiffuseFactor), IElementPropertyType.Double);
			set => this.InternalSetPrimitive<double>(nameof(this.DiffuseFactor), IElementPropertyType.Double, value, "Number", String.Empty);
		}
		public double? SpecularFactor
		{
			get => this.InternalGetPrimitive<double>(nameof(this.SpecularFactor), IElementPropertyType.Double);
			set => this.InternalSetPrimitive<double>(nameof(this.SpecularFactor), IElementPropertyType.Double, value, "Number", String.Empty);
		}
		public double? ReflectionFactor
		{
			get => this.InternalGetPrimitive<double>(nameof(this.ReflectionFactor), IElementPropertyType.Double);
			set => this.InternalSetPrimitive<double>(nameof(this.ReflectionFactor), IElementPropertyType.Double, value, "Number", String.Empty);
		}
		public double? Shininess
		{
			get => this.InternalGetPrimitive<double>(nameof(this.Shininess), IElementPropertyType.Double);
			set => this.InternalSetPrimitive<double>(nameof(this.Shininess), IElementPropertyType.Double, value, "Number", String.Empty);
		}
		public double? ShininessExponent
		{
			get => this.InternalGetPrimitive<double>(nameof(this.ShininessExponent), IElementPropertyType.Double);
			set => this.InternalSetPrimitive<double>(nameof(this.ShininessExponent), IElementPropertyType.Double, value, "Number", String.Empty);
		}
		public double? AmbientFactor
		{
			get => this.InternalGetPrimitive<double>(nameof(this.AmbientFactor), IElementPropertyType.Double);
			set => this.InternalSetPrimitive<double>(nameof(this.AmbientFactor), IElementPropertyType.Double, value, "Number", String.Empty);
		}
		public double? BumpFactor
		{
			get => this.InternalGetPrimitive<double>(nameof(this.BumpFactor), IElementPropertyType.Double);
			set => this.InternalSetPrimitive<double>(nameof(this.BumpFactor), IElementPropertyType.Double, value, "double", "Number");
		}
		public double? EmissiveFactor
		{
			get => this.InternalGetPrimitive<double>(nameof(this.EmissiveFactor), IElementPropertyType.Double);
			set => this.InternalSetPrimitive<double>(nameof(this.EmissiveFactor), IElementPropertyType.Double, value, "Number", String.Empty);
		}
		public double? TransparencyFactor
		{
			get => this.InternalGetPrimitive<double>(nameof(this.TransparencyFactor), IElementPropertyType.Double);
			set => this.InternalSetPrimitive<double>(nameof(this.TransparencyFactor), IElementPropertyType.Double, value, "Number", String.Empty);
		}

		internal Material(IElement element, IScene scene) : base(element, scene)
		{
			this.m_channels = new List<Channel>();
			this.m_readonly = new ReadOnlyCollection<Channel>(this.m_channels);

			var shading = element.FindChild("ShadingModel");

			if (!(shading is null) && shading.Attributes.Length > 0 && shading.Attributes[0].Type == IElementAttributeType.String)
			{
				this.m_shadingModel = shading.Attributes[0].GetElementValue().ToString() ?? String.Empty;
			}
			else
			{
				this.m_shadingModel = String.Empty;
			}

			var multi = element.FindChild("MultiLayer");

			if (!(multi is null) && multi.Attributes.Length > 0)
			{
				this.MultiLayer = Convert.ToBoolean(multi.Attributes[0].GetElementValue());
			}
		}

		internal void InternalSetChannel(in Channel channel) => this.m_channels.Add(channel);
	}
}
