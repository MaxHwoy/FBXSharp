﻿using FBXSharp.Core;
using System;
using System.IO;

namespace FBXSharp.Objective
{
	public class Video : FBXObject
	{
		private byte[] m_content;
		private string m_absolute;
		private string m_relative;
		private bool m_useMipMaps;

		public static readonly FBXObjectType FType = FBXObjectType.Video;

		public override FBXObjectType Type => Video.FType;

		public byte[] Content => this.m_content;

		public string AbsolutePath => this.m_absolute;

		public string RelativePath => this.m_relative;

		public bool UsesMipMaps => this.m_useMipMaps;

		// Type

		internal Video(IElement element, IScene scene) : base(element, scene)
		{
			var content = element.FindChild("Content");

			if (!(content is null) && content.Attributes.Length > 0 && content.Attributes[0].Type == IElementAttributeType.Binary)
			{
				this.m_content = content.Attributes[0].GetElementValue() as byte[];
			}
			else
			{
				this.m_content = Array.Empty<byte>();
			}

			var absolute = element.FindChild("Filename");

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

			var useMipMap = element.FindChild("UseMipMap");

			if (!(useMipMap is null) && useMipMap.Attributes.Length > 0)
			{
				this.m_useMipMaps = Convert.ToBoolean(useMipMap.Attributes[0].GetElementValue());
			}
		}

		public void SetAbsolutePath(string path)
		{
			if (!File.Exists(path))
			{
				throw new FileNotFoundException($"File with absolute path {path} does not exist");
			}

			this.m_absolute = path;
			this.m_relative = Path.GetFileName(path);
		}

		public void SetRelativePath(string path)
		{
			if (!File.Exists(path))
			{
				throw new FileNotFoundException($"File with relative path {path} does not exist");
			}

			this.m_relative = path;
			this.m_absolute = Path.GetFullPath(path);
		}

		public void SetContent(byte[] content)
		{
			this.m_content = content;

			if (content is null)
			{
				return;
			}

			// #TODO : detect mipmaps and texture type
		}

		public override IElement AsElement()
		{
			var attributes = new IElementAttribute[3]
			{
				ElementaryFactory.GetElementAttribute((long)this.GetHashCode()),
				ElementaryFactory.GetElementAttribute(this.Name),
				ElementaryFactory.GetElementAttribute("Clip"),
			};

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

			return new Element("Video", elements, attributes);
		}
	}
}
