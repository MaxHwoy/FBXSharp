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

		private Clip m_video;
		private string m_absolute;
		private string m_relative;
		private string m_media;
		private string m_textureName;

		public static readonly FBXObjectType FType = FBXObjectType.Texture;

		public static readonly FBXClassType FClass = FBXClassType.Texture;

		public override FBXObjectType Type => Texture.FType;

		public override FBXClassType Class => Texture.FClass;

		public Clip Data => this.m_video;

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

		public string UVSet
		{
			get => this.InternalGetReference<string>(nameof(this.UVSet), IElementPropertyType.String);
			set => this.InternalSetReference<string>(nameof(this.UVSet), IElementPropertyType.String, value, "KString", String.Empty);
		}

		internal Texture(IElement element, IScene scene) : base(element, scene)
		{
			this.m_absolute = String.Empty;
			this.m_relative = String.Empty;
			this.m_media = String.Empty;
			this.m_textureName = String.Empty;

			if (element is null)
			{
				return;
			}

			var absolute = element.FindChild("FileName");

			if (!(absolute is null) && absolute.Attributes.Length > 0 && absolute.Attributes[0].Type == IElementAttributeType.String)
			{
				this.m_absolute = absolute.Attributes[0].GetElementValue().ToString() ?? String.Empty;
			}

			var relative = element.FindChild("RelativeFilename");

			if (!(relative is null) && relative.Attributes.Length > 0 && relative.Attributes[0].Type == IElementAttributeType.String)
			{
				this.m_relative = relative.Attributes[0].GetElementValue().ToString() ?? String.Empty;
			}

			var media = element.FindChild("Media");

			if (!(media is null) && media.Attributes.Length > 0 && media.Attributes[0].Type == IElementAttributeType.String)
			{
				this.m_media = media.Attributes[0].GetElementValue().ToString().Substring("::").Substring("\x00\x01").Trim('\x00') ?? String.Empty;
			}

			var texture = element.FindChild("TextureName");

			if (!(texture is null) && texture.Attributes.Length > 0 && texture.Attributes[0].Type == IElementAttributeType.String)
			{
				this.m_textureName = texture.Attributes[0].GetElementValue().ToString().Substring("::").Substring("\x00\x01").Trim('\x00') ?? String.Empty;
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

		internal void InternalSetVideo(Clip video) => this.m_video = video;
		internal void InternalSetAbsolutePath(string path) => this.m_absolute = path;
		internal void InternalSetRelativePath(string path) => this.m_relative = path;
		internal void InternalSetMediaString(string media) => this.m_media = media;
		internal void InternalSetTextureName(string name) => this.m_textureName = name;

		public void SetAbsolutePath(string path)
		{
			this.m_absolute = path;
		}

		public void SetRelativePath(string path)
		{
			this.m_relative = path;
		}

		public void SetClip(Clip clip)
		{
			if (clip is null)
			{
				this.m_video = null;

				return;
			}

			if (clip.Scene != this.Scene)
			{
				throw new Exception("Clip should share same scene with texture");
			}

			this.m_video = clip;
		}

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

		public override void ResolveLink(FBXObject linker, IElementAttribute attribute)
		{
			if (linker.Class == FBXClassType.Video && linker.Type == FBXObjectType.Clip)
			{
				this.SetClip(linker as Clip);
			}
		}

		public override IElement AsElement(bool binary)
		{
			int index = 0;
			int count = 7 +
				(this.UVTranslation.HasValue ? 1 : 0) +
				(this.UVScaling.HasValue ? 1 : 0) +
				(this.AlphaSource.HasValue ? 1 : 0) +
				(this.Cropping.HasValue ? 1 : 0);

			var textureName = String.IsNullOrWhiteSpace(this.m_textureName)
				? String.Empty
				: this.m_textureName + (binary ? "\x00\x01" : "::") + "Texture";

			var mediaName = String.IsNullOrWhiteSpace(this.m_media)
				? String.Empty
				: this.m_media + (binary ? "\x00\x01" : "::") + "Video";

			var elements = new IElement[count];

			elements[index++] = Element.WithAttribute("Type", ElementaryFactory.GetElementAttribute("TextureVideoClip"));
			elements[index++] = Element.WithAttribute("Version", ElementaryFactory.GetElementAttribute(202));
			elements[index++] = Element.WithAttribute("TextureName", ElementaryFactory.GetElementAttribute(textureName));
			elements[index++] = this.BuildProperties70();
			elements[index++] = Element.WithAttribute("Media", ElementaryFactory.GetElementAttribute(mediaName));
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

			return new Element(this.Class.ToString(), elements, this.BuildAttributes("Texture", String.Empty, binary));
		}
	}

	public class TextureBuilder : BuilderBase
	{
		private Clip m_video;
		private string m_absolute;
		private string m_relative;
		private string m_media;
		private string m_textureName;
		private Vector2? m_uvTraslation;
		private Vector2? m_uvScaling;
		private Texture.AlphaSourceType? m_alpha;
		private Texture.CropBase? m_cropping;

		public TextureBuilder(Scene scene) : base(scene)
		{
		}

		public Texture BuildTexture()
		{
			var texture = this.m_scene.CreateTexture();

			texture.Name = this.m_name;
			texture.InternalSetMediaString(this.m_media ?? String.Empty);
			texture.InternalSetTextureName(this.m_textureName ?? String.Empty);

			if (this.m_video is null)
			{
				texture.InternalSetAbsolutePath(this.m_absolute ?? String.Empty);
				texture.InternalSetRelativePath(this.m_relative ?? String.Empty);
			}
			else
			{
				texture.InternalSetVideo(this.m_video);
				texture.InternalSetAbsolutePath(this.m_video.AbsolutePath);
				texture.InternalSetRelativePath(this.m_video.RelativePath);
				texture.UseMipMap = this.m_video.UsesMipMaps;
			}

			texture.UVTranslation = this.m_uvTraslation;
			texture.UVScaling = this.m_uvScaling;
			texture.AlphaSource = this.m_alpha;
			texture.Cropping = this.m_cropping;

			foreach (var property in this.m_properties)
			{
				texture.AddProperty(property);
			}

			return texture;
		}

		public TextureBuilder WithName(string name)
		{
			this.SetObjectName(name);
			return this;
		}

		public TextureBuilder WithFBXProperty<T>(string name, T value, bool isUser = false)
		{
			this.SetFBXProperty(name, value, isUser);
			return this;
		}
		public TextureBuilder WithFBXProperty<T>(string name, T value, IElementPropertyFlags flags)
		{
			this.SetFBXProperty(name, value, flags);
			return this;
		}
		public TextureBuilder WithFBXProperty<T>(FBXProperty<T> property)
		{
			this.SetFBXProperty(property);
			return this;
		}

		public TextureBuilder WithVideo(Clip video)
		{
			if (video is null || video.Scene == this.m_scene)
			{
				this.m_video = video;

				return this;
			}

			throw new ArgumentException("Video should share same scene as the texture");
		}

		public TextureBuilder WithAbsolutePath(string path)
		{
			if (this.m_video is null)
			{
				this.m_absolute = path;
			}

			return this;
		}
		public TextureBuilder WithRelativePath(string path)
		{
			if (this.m_video is null && !String.IsNullOrWhiteSpace(path))
			{
				this.m_relative = path;
			}

			return this;
		}
		public TextureBuilder WithMedia(string media)
		{
			this.m_media = media;
			return this;
		}
		public TextureBuilder WithTextureName(string name)
		{
			this.m_textureName = name;
			return this;
		}

		public TextureBuilder WithUVTranslation(Vector2 uv)
		{
			this.m_uvTraslation = uv;
			return this;
		}
		public TextureBuilder WithUVScaling(Vector2 uv)
		{
			this.m_uvScaling = uv;
			return this;
		}
		public TextureBuilder WithAlphaSource(Texture.AlphaSourceType alphaSource)
		{
			this.m_alpha = alphaSource;
			return this;
		}
		public TextureBuilder WithCropping(Texture.CropBase cropping)
		{
			this.m_cropping = cropping;
			return this;
		}
	}
}
