using FBXSharp.Core;
using System;

namespace FBXSharp.Objective
{
	public class Video : FBXObject
	{
		public enum ImageType
		{
			PNG,
			JPG,
			DDS,
			WEBP,
			KTX2,
			Other,
		}

		private byte[] m_content;
		private string m_absolute;
		private string m_relative;
		private bool m_useMipMaps;
		private ImageType m_image;

		public static readonly FBXObjectType FType = FBXObjectType.Video;

		public override FBXObjectType Type => Video.FType;

		public ImageType Image => this.m_image;

		public byte[] Content => this.m_content;

		public string AbsolutePath => this.m_absolute;

		public string RelativePath => this.m_relative;

		public bool UsesMipMaps => this.m_useMipMaps;

		public string Path
		{
			get => this.InternalGetReference<string>(nameof(this.Path), IElementPropertyType.String);
			set => this.InternalSetReference<string>(nameof(this.Path), IElementPropertyType.String, value, "KString", "XRefUrl");
		}

		public string RelPath
		{
			get => this.InternalGetReference<string>(nameof(this.RelPath), IElementPropertyType.String);
			set => this.InternalSetReference<string>(nameof(this.RelPath), IElementPropertyType.String, value, "KString", "XRefUrl");
		}

		internal Video(IElement element, IScene scene) : base(element, scene)
		{
			this.m_image = ImageType.Other;
			this.m_absolute = String.Empty;
			this.m_relative = String.Empty;
			this.m_content = Array.Empty<byte>();

			if (element is null)
			{
				return;
			}

			var content = element.FindChild("Content");

			if (!(content is null) && content.Attributes.Length > 0 && content.Attributes[0].Type == IElementAttributeType.Binary)
			{
				this.m_content = content.Attributes[0].GetElementValue() as byte[];
				this.m_image = Video.IsImage(this.m_content);
			}

			var absolute = element.FindChild("Filename");

			if (!(absolute is null) && absolute.Attributes.Length > 0 && absolute.Attributes[0].Type == IElementAttributeType.String)
			{
				this.m_absolute = absolute.Attributes[0].GetElementValue().ToString() ?? String.Empty;
			}

			var relative = element.FindChild("RelativeFilename");

			if (!(relative is null) && relative.Attributes.Length > 0 && relative.Attributes[0].Type == IElementAttributeType.String)
			{
				this.m_relative = relative.Attributes[0].GetElementValue().ToString() ?? String.Empty;
			}

			var useMipMap = element.FindChild("UseMipMap");

			if (!(useMipMap is null) && useMipMap.Attributes.Length > 0)
			{
				this.m_useMipMaps = Convert.ToBoolean(useMipMap.Attributes[0].GetElementValue());
			}
			else
			{
				this.m_useMipMaps = this.m_image == ImageType.DDS;
			}
		}

		public void SetAbsolutePath(string path)
		{
			this.m_absolute = path;
		}

		public void SetRelativePath(string path)
		{
			this.m_relative = path;
		}

		public void SetContent(byte[] content)
		{
			this.m_content = content ?? Array.Empty<byte>();
			this.m_image = Video.IsImage(this.m_content);
			this.m_useMipMaps = this.m_image == ImageType.DDS;
		}

		public override IElement AsElement(bool binary)
		{
			var elements = new IElement[5 + (this.m_content.Length == 0 ? 0 : 1)];

			elements[0] = Element.WithAttribute("Type", ElementaryFactory.GetElementAttribute("Clip"));
			elements[1] = this.BuildProperties70();
			elements[2] = Element.WithAttribute("UseMipMap", ElementaryFactory.GetElementAttribute(this.m_useMipMaps ? 1 : 0));
			elements[3] = Element.WithAttribute("Filename", ElementaryFactory.GetElementAttribute(this.m_absolute));
			elements[4] = Element.WithAttribute("RelativeFilename", ElementaryFactory.GetElementAttribute(this.m_relative));

			if (this.m_content.Length != 0)
			{
				elements[5] = Element.WithAttribute("Content", ElementaryFactory.GetElementAttribute(this.m_content));
			}

			return new Element("Video", elements, this.BuildAttributes("Clip", binary));
		}

		private static bool IsPngImage(byte[] data)
		{
			return data[0] == 0x89 && data[1] == 0x50 && data[2] == 0x4E && data[3] == 0x47;
		}

		private static bool IsJpgImage(byte[] data)
		{
			return data[0] == 0xFF && data[1] == 0xD8;
		}

		private static bool IsDdsImage(byte[] data)
		{
			return data[0] == 0x44 && data[1] == 0x44 && data[2] == 0x53 && data[3] == 0x20;
		}

		private static bool IsWebpImage(byte[] data)
		{
			return data[0] == 0x52 && data[1] == 0x49 && data[2] == 0x46 && data[3] == 0x46 &&
				   data[8] == 0x57 && data[9] == 0x45 && data[10] == 0x42 && data[11] == 0x50;
		}

		private static bool IsKtx2Image(byte[] data)
		{
			return data[0] == 0xBB && data[1] == 0x30 && data[2] == 0x32 && data[3] == 0x20 &&
				   data[5] == 0x58 && data[6] == 0x54 && data[7] == 0x4B && data[7] == 0xAB &&
				   data[8] == 0x0A && data[9] == 0x1A && data[10] == 0x0A && data[11] == 0x0D;
		}

		private static ImageType IsImage(byte[] data)
		{
			if (data is null || data.Length < 12)
			{
				return ImageType.Other;
			}

			if (Video.IsDdsImage(data))
			{
				return ImageType.DDS;
			}

			if (Video.IsJpgImage(data))
			{
				return ImageType.JPG;
			}

			if (Video.IsPngImage(data))
			{
				return ImageType.PNG;
			}

			if (Video.IsWebpImage(data))
			{
				return ImageType.WEBP;
			}

			if (Video.IsKtx2Image(data))
			{
				return ImageType.KTX2;
			}

			return ImageType.Other;
		}
	}
}
