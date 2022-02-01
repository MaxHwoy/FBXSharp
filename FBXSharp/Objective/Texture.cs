using FBXSharp.Core;
using FBXSharp.ValueTypes;
using System;

namespace FBXSharp.Objective
{
	public class Texture : FBXObject
	{
		public enum AlphaSourceType
		{
			None,
			RGB_Intensity,
			Alpha_Black,
		}

		public enum MappingType
		{
			Null,
			Planar,
			Spherical,
			Cylindrical,
			Box,
			Face,
			UV,
			Environment,
		}

		public enum PlanarMappingNormalType
		{
			X,
			Y,
			Z,
		}

		public enum TextureUseType
		{
			Standard,
			ShadowMap,
			LightMap,
			SphericalReflexionMap,
			SphereReflexionMap,
			BumpNormalMap,
		}

		public enum WrapMode
		{
			Repeat,
			Clamp,
		}

		public struct CropBase
		{
			public int V0 { get; set; }
			public int V1 { get; set; }
			public int V2 { get; set; }
			public int V3 { get; set; }

			public override string ToString() => $"<{this.V0}, {this.V1}, {this.V2}, {this.V3}>";
		}

		private Video m_video;
		private string m_absolute;
		private string m_relative;
		private string m_media;
		private string m_textureName;

		public static readonly FBXObjectType FType = FBXObjectType.Texture;

		public override FBXObjectType Type => Texture.FType;

		public Video Data => this.m_video;

		public string AbsolutePath => this.m_absolute;

		public string RelativePath => this.m_relative;

		public string Media => this.m_media;

		public string TextureName => this.m_textureName;

		public AlphaSourceType? AlphaSource { get; set; }

		public Vector2? UVTranslation { get; set; }

		public Vector2? UVScaling { get; set; }

		public CropBase? Cropping { get; set; }

		public WrapMode? WrapModeU
		{
			get => this.InternalGetEnumType(nameof(this.WrapModeU), out WrapMode mode) ? mode : (WrapMode?)null;
			set => this.InternalSetEnumType(nameof(this.WrapModeU), value.HasValue, (int)(value ?? 0), "enum", String.Empty);
		}

		public WrapMode? WrapModeV
		{
			get => this.InternalGetEnumType(nameof(this.WrapModeV), out WrapMode mode) ? mode : (WrapMode?)null;
			set => this.InternalSetEnumType(nameof(this.WrapModeV), value.HasValue, (int)(value ?? 0), "enum", String.Empty);
		}

		public bool? UseMaterial
		{
			get => this.InternalGetPrimitive<bool>(nameof(this.UseMaterial), IElementPropertyType.Bool);
			set => this.InternalSetPrimitive<bool>(nameof(this.UseMaterial), IElementPropertyType.Bool, value, "bool", String.Empty);
		}

		public bool? UseMipMap
		{
			get => this.InternalGetPrimitive<bool>(nameof(this.UseMipMap), IElementPropertyType.Bool);
			set => this.InternalSetPrimitive<bool>(nameof(this.UseMipMap), IElementPropertyType.Bool, value, "bool", String.Empty);
		}

		internal Texture(IElement element, IScene scene) : base(element, scene)
		{
			var absolute = element.FindChild("FileName");

			if (!(absolute is null) && absolute.Attributes.Length > 0 && absolute.Attributes[0].Type == IElementAttributeType.String)
			{
				this.m_absolute = absolute.Attributes[0].GetElementValue().ToString() ?? String.Empty;
			}
			else
			{
				this.m_absolute = String.Empty;
			}

			var relative = element.FindChild("RelativeFilename");

			if (!(relative is null) && relative.Attributes.Length > 0 && relative.Attributes[0].Type == IElementAttributeType.String)
			{
				this.m_relative = relative.Attributes[0].GetElementValue().ToString() ?? String.Empty;
			}
			else
			{
				this.m_relative = String.Empty;
			}

			var media = element.FindChild("Media");

			if (!(media is null) && media.Attributes.Length > 0 && media.Attributes[0].Type == IElementAttributeType.String)
			{
				this.m_media = media.Attributes[0].GetElementValue().ToString() ?? String.Empty;
			}
			else
			{
				this.m_media = String.Empty;
			}

			var texture = element.FindChild("TextureName");

			if (!(texture is null) && texture.Attributes.Length > 0 && texture.Attributes[0].Type == IElementAttributeType.String)
			{
				this.m_textureName = texture.Attributes[0].GetElementValue().ToString() ?? String.Empty;
			}
			else
			{
				this.m_textureName = String.Empty;
			}

			var uvTranslation = element.FindChild("ModelUVTranslation");

			if (!(uvTranslation is null) && uvTranslation.Attributes.Length > 1)
			{
				this.UVTranslation = new Vector2
				(
					Convert.ToDouble(uvTranslation.Attributes[0].GetElementValue()),
					Convert.ToDouble(uvTranslation.Attributes[1].GetElementValue())
				);
			}

			var uvScaling = element.FindChild("ModelUVScaling");

			if (!(uvScaling is null) && uvScaling.Attributes.Length > 1)
			{
				this.UVScaling = new Vector2
				(
					Convert.ToDouble(uvScaling.Attributes[0].GetElementValue()),
					Convert.ToDouble(uvScaling.Attributes[1].GetElementValue())
				);
			}

			var alphaSource = element.FindChild("Texture_Alpha_Source");

			if (!(alphaSource is null) && alphaSource.Attributes.Length > 0 && alphaSource.Attributes[0].Type == IElementAttributeType.String)
			{
				if (Enum.TryParse(alphaSource.Attributes[0].GetElementValue().ToString(), out AlphaSourceType type))
				{
					this.AlphaSource = type;
				}
			}

			var cropping = element.FindChild("Cropping");

			if (!(cropping is null) && cropping.Attributes.Length > 3)
			{
				this.Cropping = new CropBase()
				{
					V0 = Convert.ToInt32(cropping.Attributes[0].GetElementValue()),
					V1 = Convert.ToInt32(cropping.Attributes[0].GetElementValue()),
					V2 = Convert.ToInt32(cropping.Attributes[0].GetElementValue()),
					V3 = Convert.ToInt32(cropping.Attributes[0].GetElementValue()),
				};
			}
		}

		internal void InternalSetVideo(Video video) => this.m_video = video;

		public override Connection[] GetConnections()
		{
			if (this.m_video is null)
			{
				return Array.Empty<Connection>();
			}
			else
			{
				return new Connection[]
				{
					new Connection(Connection.ConnectionType.Object, this.m_video.GetHashCode(), this.GetHashCode()),
				};
			}
		}

		public override IElement AsElement()
		{
			int index = 0;
			int count = 7 +
				(this.UVTranslation.HasValue ? 1 : 0) +
				(this.UVScaling.HasValue ? 1 : 0) +
				(this.AlphaSource.HasValue ? 1 : 0) +
				(this.Cropping.HasValue ? 1 : 0);

			var elements = new IElement[count];

			elements[index++] = Element.WithAttribute("Type", ElementaryFactory.GetElementAttribute("TextureVideoClip"));
			elements[index++] = Element.WithAttribute("Version", ElementaryFactory.GetElementAttribute(202));
			elements[index++] = Element.WithAttribute("TextureName", ElementaryFactory.GetElementAttribute(this.m_textureName));
			elements[index++] = this.BuildProperties70();
			elements[index++] = Element.WithAttribute("Media", ElementaryFactory.GetElementAttribute(this.m_media));
			elements[index++] = Element.WithAttribute("FileName", ElementaryFactory.GetElementAttribute(this.m_absolute));
			elements[index++] = Element.WithAttribute("RelativeFilename", ElementaryFactory.GetElementAttribute(this.m_relative));

			if (this.UVTranslation.HasValue)
			{
				elements[index++] = new Element("ModelUVTranslation", null, new IElementAttribute[]
				{
					ElementaryFactory.GetElementAttribute(this.UVTranslation.Value.X),
					ElementaryFactory.GetElementAttribute(this.UVTranslation.Value.Y),
				});
			}

			if (this.UVScaling.HasValue)
			{
				elements[index++] = new Element("ModelUVScaling", null, new IElementAttribute[]
				{
					ElementaryFactory.GetElementAttribute(this.UVScaling.Value.X),
					ElementaryFactory.GetElementAttribute(this.UVScaling.Value.Y),
				});
			}

			if (this.AlphaSource.HasValue)
			{
				elements[index++] = new Element("Texture_Alpha_Source", null, new IElementAttribute[]
				{
					ElementaryFactory.GetElementAttribute(this.AlphaSource.ToString()),
				});
			}

			if (this.Cropping.HasValue)
			{
				elements[index++] = new Element("Cropping", null, new IElementAttribute[]
				{
					ElementaryFactory.GetElementAttribute(this.Cropping.Value.V0),
					ElementaryFactory.GetElementAttribute(this.Cropping.Value.V1),
					ElementaryFactory.GetElementAttribute(this.Cropping.Value.V2),
					ElementaryFactory.GetElementAttribute(this.Cropping.Value.V3),
				});
			}

			return new Element("Texture", elements, this.BuildAttributes(String.Empty));
		}
	}
}
