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
		}

		private Video m_video;
		private string m_absolute;
		private string m_relative;
		private string m_media;
		private string m_textureName;
		private Vector2 m_uvTranslation;
		private Vector2 m_uvScaling;
		private CropBase m_cropping;

		public static readonly FBXObjectType FType = FBXObjectType.Texture;

		public override FBXObjectType Type => Texture.FType;

		public Video Data => this.m_video;

		public string AbsolutePath => this.m_absolute;

		public string RelativePath => this.m_relative;

		public string Media => this.m_media;

		public string TextureName => this.m_textureName;

		public AlphaSourceType AlphaSource { get; set; }

		public Vector2 UVTranslation { get; set; }

		public Vector2 UVScaling { get; set; }

		public CropBase Cropping { get; set; }

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
			var attributes = new IElementAttribute[3]
			{
				ElementaryFactory.GetElementAttribute((long)this.GetHashCode()),
				ElementaryFactory.GetElementAttribute(this.Name),
				ElementaryFactory.GetElementAttribute(String.Empty),
			};

			var elements = new IElement[7];

			elements[0] = Element.WithAttribute("Type", ElementaryFactory.GetElementAttribute("TextureVideoClip"));
			elements[1] = Element.WithAttribute("Version", ElementaryFactory.GetElementAttribute(202));
			elements[2] = Element.WithAttribute("TextureName", ElementaryFactory.GetElementAttribute(this.m_textureName));
			elements[3] = Element.WithAttribute("Media", ElementaryFactory.GetElementAttribute(this.m_media));
			elements[4] = Element.WithAttribute("FileName", ElementaryFactory.GetElementAttribute(this.m_absolute));
			elements[5] = Element.WithAttribute("RelativeFilename", ElementaryFactory.GetElementAttribute(this.m_relative));
			elements[6] = this.BuildProperties70();

			return new Element("Texture", elements, attributes);
		}
	}
}
