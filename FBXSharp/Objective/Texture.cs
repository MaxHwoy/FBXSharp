using FBXSharp.Core;
using System;

namespace FBXSharp.Objective
{
	public class Texture : FBXObject
	{
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

		// Type

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
	}
}
