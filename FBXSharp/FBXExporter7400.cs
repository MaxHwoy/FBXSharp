using FBXSharp.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;

namespace FBXSharp
{
	public class FBXExporter7400
	{
		public struct Options
		{
			public string Creator { get; set; }
			public string AppVendor { get; set; }
			public string AppName { get; set; }
			public string AppVersion { get; set; }
			public DateTime SaveTime { get; set; }
			public bool CompressData { get; set; }
		}
		
		private readonly IScene m_scene;

		public FBXExporter7400(IScene scene)
		{
			if (scene is null)
			{
				throw new ArgumentNullException("Scene provided cannot be null");
			}

			this.m_scene = scene;
		}

		private static byte[] GetFileID(in DateTime time)
		{
			// https://github.com/hamish-milne/FbxWriter/blob/master/Fbx/FbxBinary.cs

			var strinc = time.ToString("ssMMHHddffyyyymm");
			
			var result = new byte[0x10]
			{
				0x58, 0xAB, 0xA9, 0xF0,
				0x6C, 0xA2, 0xD8, 0x3F,
				0x4D, 0x47, 0x49, 0xA3,
				0xB4, 0xB2, 0xE7, 0x3D,
			};

			for (byte i = 0, k = 0x40; i < 0x10; ++i)
			{
				k = result[i] ^= (byte)(k ^ (byte)strinc[i]);
			}

			return result;
		}

		private static byte[] GetEncryption(in DateTime time)
		{
			var strinc = time.ToString("ssMMHHddffyyyymm");

			var crypto = new byte[0x10]
			{
				0xE2, 0x4F, 0x7B, 0x5F,
				0xCD, 0xE4, 0xC8, 0x6D,
				0xDB, 0xD8, 0xFB, 0xD7,
				0x40, 0x58, 0xC6, 0x78,
			};

			var result = new byte[0x10]
			{
				0x58, 0xAB, 0xA9, 0xF0,
				0x6C, 0xA2, 0xD8, 0x3F,
				0x4D, 0x47, 0x49, 0xA3,
				0xB4, 0xB2, 0xE7, 0x3D,
			};

			for (byte i = 0, k = 0x40; i < 0x10; ++i)
			{
				k = result[i] ^= (byte)(k ^ (byte)strinc[i]);
			}

			for (byte i = 0, k = 0x40; i < 0x10; ++i)
			{
				k = result[i] ^= (byte)(k ^ crypto[i]);
			}

			for (byte i = 0, k = 0x40; i < 0x10; ++i)
			{
				k = result[i] ^= (byte)(k ^ (byte)strinc[i]);
			}

			return result;
		}

		private static IElement GetProperties70(IReadOnlyList<IElementProperty> properties)
		{
			var children = new IElement[properties.Count];

			for (int i = 0; i < children.Length; ++i)
			{
				children[i] = PropertyFactory.AsElement(properties[i]);
			}

			return new Element("Properties70", children, null);
		}

		private static IElement GetDefaultExtensionProperties(in Options options)
		{
			var properties = new IElementProperty[13];

			properties[0] = new FBXProperty<string>("KString", "Url", "DocumentUrl", IElementPropertyFlags.None, "/foobar.fbx");
			properties[1] = new FBXProperty<string>("KString", "Url", "SrcDocumentUrl", IElementPropertyFlags.None, "/foobar.fbx");
			properties[2] = new FBXProperty<object>("Compound", String.Empty, "Original");
			properties[3] = new FBXProperty<string>("KString", String.Empty, "Original|ApplicationVendor", IElementPropertyFlags.None, options.AppVendor);
			properties[4] = new FBXProperty<string>("KString", String.Empty, "Original|ApplicationName", IElementPropertyFlags.None, options.AppName);
			properties[5] = new FBXProperty<string>("KString", String.Empty, "Original|ApplicationVersion", IElementPropertyFlags.None, options.AppVersion);
			properties[6] = new FBXProperty<DateTime>("DateTime", String.Empty, "Original|DateTime_GMT", IElementPropertyFlags.None, options.SaveTime);
			properties[7] = new FBXProperty<string>("KString", String.Empty, "Original|FileName", IElementPropertyFlags.None, "/foobar.fbx");
			properties[8] = new FBXProperty<object>("Compound", String.Empty, "LastSaved");
			properties[9] = new FBXProperty<string>("KString", String.Empty, "LastSaved|ApplicationVendor", IElementPropertyFlags.None, options.AppVendor);
			properties[10] = new FBXProperty<string>("KString", String.Empty, "LastSaved|ApplicationName", IElementPropertyFlags.None, options.AppName);
			properties[11] = new FBXProperty<string>("KString", String.Empty, "LastSaved|ApplicationVersion", IElementPropertyFlags.None, options.AppVersion);
			properties[12] = new FBXProperty<DateTime>("DateTime", String.Empty, "LastSaved|DateTime_GMT", IElementPropertyFlags.None, options.SaveTime);

			return FBXExporter7400.GetProperties70(properties);
		}

		private static IElement GetDefaultDocumentProperties()
		{
			var properties = new IElementProperty[2];

			properties[0] = new FBXProperty<object>("object", String.Empty, "SourceObject");
			properties[1] = new FBXProperty<string>("KString", String.Empty, "ActiveAnimStackName", IElementPropertyFlags.None, String.Empty);

			return FBXExporter7400.GetProperties70(properties);
		}

		private IElement GetFBXHeaderExtension(in Options options)
		{
			var children = new IElement[6];

			children[0] = Element.WithAttribute("FBXHeaderVersion", ElementaryFactory.GetElementAttribute(1003));
			children[1] = Element.WithAttribute("FBXVersion", ElementaryFactory.GetElementAttribute(7400));
			children[2] = Element.WithAttribute("EncryptionType", ElementaryFactory.GetElementAttribute(0));
			children[4] = Element.WithAttribute("Creator", ElementaryFactory.GetElementAttribute(options.Creator));

			children[3] = new Element("CreationTimeStamp", new IElement[]
			{
				Element.WithAttribute("Version", ElementaryFactory.GetElementAttribute(1000)),
				Element.WithAttribute("Year", ElementaryFactory.GetElementAttribute(options.SaveTime.Year)),
				Element.WithAttribute("Month", ElementaryFactory.GetElementAttribute(options.SaveTime.Month)),
				Element.WithAttribute("Day", ElementaryFactory.GetElementAttribute(options.SaveTime.Day)),
				Element.WithAttribute("Hour", ElementaryFactory.GetElementAttribute(options.SaveTime.Hour)),
				Element.WithAttribute("Minute", ElementaryFactory.GetElementAttribute(options.SaveTime.Minute)),
				Element.WithAttribute("Second", ElementaryFactory.GetElementAttribute(options.SaveTime.Second)),
				Element.WithAttribute("Millisecond", ElementaryFactory.GetElementAttribute(options.SaveTime.Millisecond)),
			}, null);

			children[5] = new Element
			(
				"SceneInfo",
				new IElement[]
				{
					Element.WithAttribute("Type", ElementaryFactory.GetElementAttribute("UserData")),
					Element.WithAttribute("Version", ElementaryFactory.GetElementAttribute(100)),
					new Element("MetaData", new IElement[]
					{
						Element.WithAttribute("Version", ElementaryFactory.GetElementAttribute(100)),
						Element.WithAttribute("Title", ElementaryFactory.GetElementAttribute(String.Empty)),
						Element.WithAttribute("Subject", ElementaryFactory.GetElementAttribute(String.Empty)),
						Element.WithAttribute("Author", ElementaryFactory.GetElementAttribute(String.Empty)),
						Element.WithAttribute("Keywords", ElementaryFactory.GetElementAttribute(String.Empty)),
						Element.WithAttribute("Revision", ElementaryFactory.GetElementAttribute(String.Empty)),
						Element.WithAttribute("Comment", ElementaryFactory.GetElementAttribute(String.Empty)),
					}, null),
					FBXExporter7400.GetDefaultExtensionProperties(options),
				},
				new IElementAttribute[]
				{
					ElementaryFactory.GetElementAttribute("GlobalInfo::SceneInfo"),
					ElementaryFactory.GetElementAttribute("UserData")
				}
			);

			return new Element("FBXHeaderExtension", children, null);
		}

		private IElement GetFileId(in Options options)
		{
			return Element.WithAttribute("FileId", ElementaryFactory.GetElementAttribute(FBXExporter7400.GetFileID(options.SaveTime)));
		}

		private IElement GetCreationTime(in Options options)
		{
			return Element.WithAttribute("CreationTime", ElementaryFactory.GetElementAttribute(options.SaveTime.ToString("yyyy-MM-dd HH:mm:ss:fff")));
		}

		private IElement GetCreator(in Options options)
		{
			return Element.WithAttribute("Creator", ElementaryFactory.GetElementAttribute(options.Creator));
		}

		private IElement GetGlobalSettings()
		{
			return new Element("GlobalSettings", new IElement[]
			{
				Element.WithAttribute("Version", ElementaryFactory.GetElementAttribute(1000)),
				FBXExporter7400.GetProperties70(this.m_scene.Settings.Properties),
			}, null);
		}

		private IElement GetDocuments()
		{
			return new Element("Documents", new IElement[]
			{
				Element.WithAttribute("Count", ElementaryFactory.GetElementAttribute(1)),
				new Element
				(
					"Document",
					new IElement[]
					{
						FBXExporter7400.GetDefaultDocumentProperties(),
						Element.WithAttribute("RootNode", ElementaryFactory.GetElementAttribute(0L)),
					},
					new IElementAttribute[]
					{
						ElementaryFactory.GetElementAttribute(this.m_scene.GetHashCode()),
						ElementaryFactory.GetElementAttribute("Scene"),
						ElementaryFactory.GetElementAttribute("Scene"),
					}
				),
			}, null);
		}

		private IElement GetReferences()
		{
			return new Element("References", null, null);
		}

		private IElement GetDefinitions()
		{
			int length = 0;
			var mapper = new int[(int)FBXClassType.Count];

			for (int i = 0; i < this.m_scene.Objects.Count; ++i)
			{
				++mapper[(int)this.m_scene.Objects[i].Class];
			}

			for (int i = 0; i < mapper.Length; ++i)
			{
				if (mapper[i] != 0)
				{
					++length;
				}
			}

			var templates = new IElement[length + 3];
			int tempindex = 3;

			templates[0] = Element.WithAttribute("Version", ElementaryFactory.GetElementAttribute(100));
			templates[1] = Element.WithAttribute("Count", ElementaryFactory.GetElementAttribute(this.m_scene.Objects.Count + 1));
			templates[2] = new Element("ObjectType", new IElement[]
			{
				Element.WithAttribute("Count", ElementaryFactory.GetElementAttribute(1)),
			}, new IElementAttribute[] { ElementaryFactory.GetElementAttribute(FBXClassType.GlobalSettings.ToString()) });

			for (FBXClassType type = FBXClassType.Video; type < FBXClassType.Count; ++type)
			{
				int count = mapper[(int)type];

				if (count == 0)
				{
					continue;
				}

				var attributes = new IElementAttribute[]
				{
					ElementaryFactory.GetElementAttribute(type.ToString()),
				};

				var template = this.m_scene.GetTemplateObject(type);

				if (template is null)
				{
					var elements = new IElement[]
					{
						Element.WithAttribute("Count", ElementaryFactory.GetElementAttribute(count)),
					};

					templates[tempindex++] = new Element("ObjectType", null, attributes);
				}
				else
				{
					var elements = new IElement[]
					{
						Element.WithAttribute("Count", ElementaryFactory.GetElementAttribute(count)),
						new Element("PropertyTemplate", new IElement[]
						{
							FBXExporter7400.GetProperties70(template.Properties),
						}, new IElementAttribute[] { ElementaryFactory.GetElementAttribute(template.Name) }),
					};

					templates[tempindex++] = new Element("ObjectType", elements, attributes);
				}
			}

			return new Element("Definitions", templates, null);
		}

		private IElement GetObjects()
		{
			var elements = new IElement[this.m_scene.Objects.Count];

			for (int i = 0; i < elements.Length; ++i)
			{
				elements[i] = this.m_scene.Objects[i].AsElement(true);
			}

			return new Element("Objects", elements, null);
		}

		private IElement GetConnections()
		{
			var connections = new List<Connection>(this.m_scene.Root.GetConnections());

			foreach (var @object in this.m_scene.Objects)
			{
				connections.AddRange(@object.GetConnections());
			}

			var elements = new IElement[connections.Count];

			for (int i = 0; i < elements.Length; ++i)
			{
				var connection = connections[i];

				switch (connection.Type)
				{
					case Connection.ConnectionType.Object:
						{
							elements[i] = new Element("C", null, new IElementAttribute[]
							{
								ElementaryFactory.GetElementAttribute("OO"),
								ElementaryFactory.GetElementAttribute(connection.Source),
								ElementaryFactory.GetElementAttribute(connection.Destination),
							});

							break;
						}

					case Connection.ConnectionType.Property:
						{
							elements[i] = new Element("C", null, new IElementAttribute[]
							{
								ElementaryFactory.GetElementAttribute("OP"),
								ElementaryFactory.GetElementAttribute(connection.Source),
								ElementaryFactory.GetElementAttribute(connection.Destination),
								connection.Property,
							});

							break;
						}

					default:
						{
							elements[i] = new Element("C", null, new IElementAttribute[]
							{
								ElementaryFactory.GetElementAttribute(String.Empty),
								ElementaryFactory.GetElementAttribute(connection.Source),
								ElementaryFactory.GetElementAttribute(connection.Destination),
							});

							break;
						}
				}
			}

			return new Element("Connections", elements, null);
		}

		private IElement GetTakes()
		{
			var elements = new IElement[this.m_scene.TakeInfos.Count + 1];

			elements[0] = Element.WithAttribute("Current", ElementaryFactory.GetElementAttribute(String.Empty));

			for (int i = 0; i < this.m_scene.TakeInfos.Count; ++i)
			{
				var takeInfo = this.m_scene.TakeInfos[i];

				elements[i + 1] = new Element("Take", new IElement[]
				{
					new Element("FileName", null, new IElementAttribute[]
					{
						ElementaryFactory.GetElementAttribute(takeInfo.Filename),
					}),
					new Element("LocalTime", null, new IElementAttribute[]
					{
						ElementaryFactory.GetElementAttribute(MathExtensions.SecondsToFBXTime(takeInfo.LocalTimeFrom)),
						ElementaryFactory.GetElementAttribute(MathExtensions.SecondsToFBXTime(takeInfo.LocalTimeTo)),
					}),
					new Element("ReferenceTime", null, new IElementAttribute[]
					{
						ElementaryFactory.GetElementAttribute(MathExtensions.SecondsToFBXTime(takeInfo.ReferenceTimeFrom)),
						ElementaryFactory.GetElementAttribute(MathExtensions.SecondsToFBXTime(takeInfo.ReferenceTimeTo)),
					}),
				}, new IElementAttribute[] { ElementaryFactory.GetElementAttribute(takeInfo.Name) });
			}

			return new Element("Takes", elements, null);
		}

		private static void WriteArrayAttribute<T>(BinaryWriter bw, T[] array, bool compress) where T : unmanaged
		{
			if (array.Length == 0)
			{
				bw.Write(0); // array length
				bw.Write(0); // decompressed
				bw.Write(0); // buffer length

				return;
			}

			if (compress)
			{
				unsafe
				{
					var buffer = new byte[sizeof(T) * array.Length];
					var output = new byte[buffer.Length << 1];

					fixed (T* ptr = &array[0])
					{
						Marshal.Copy(new IntPtr(ptr), buffer, 0, buffer.Length);
					}

					var deflater = new ICSharpCode.SharpZipLib.Zip.Compression.Deflater();

					deflater.SetInput(buffer);

					deflater.Finish();

					var outlen = deflater.Deflate(output);

					bw.Write(array.Length);
					bw.Write(1);
					bw.Write(outlen);
					bw.BaseStream.Write(output, 0, outlen);
				}
			}
			else
			{
				unsafe
				{
					var buffer = new byte[sizeof(T) * array.Length];

					fixed (T* ptr = &array[0])
					{
						Marshal.Copy(new IntPtr(ptr), buffer, 0, buffer.Length);
					}

					bw.Write(array.Length);
					bw.Write(0);
					bw.Write(buffer.Length);
					bw.Write(buffer);
				}
			}
		}

		private static void WriteAttribute(BinaryWriter bw, IElementAttribute attribute, bool compress)
		{
			bw.Write((byte)attribute.Type);

			switch (attribute.Type)
			{
				case IElementAttributeType.Byte: bw.Write((byte)attribute.GetElementValue()); break;
				case IElementAttributeType.Int16: bw.Write((short)attribute.GetElementValue()); break;
				case IElementAttributeType.Int32: bw.Write((int)attribute.GetElementValue()); break;
				case IElementAttributeType.Int64: bw.Write((long)attribute.GetElementValue()); break;
				case IElementAttributeType.Single: bw.Write((float)attribute.GetElementValue()); break;
				case IElementAttributeType.Double: bw.Write((double)attribute.GetElementValue()); break;
				case IElementAttributeType.String: bw.WriteStringPrefixInt(attribute.GetElementValue().ToString()); break;
				case IElementAttributeType.Binary: bw.WriteBytesPrefixed(attribute.GetElementValue() as byte[]); break;
				case IElementAttributeType.ArrayBoolean: FBXExporter7400.WriteArrayAttribute(bw, attribute.GetElementValue() as byte[], compress); break;
				case IElementAttributeType.ArrayInt32: FBXExporter7400.WriteArrayAttribute(bw, attribute.GetElementValue() as int[], compress); break;
				case IElementAttributeType.ArrayInt64: FBXExporter7400.WriteArrayAttribute(bw, attribute.GetElementValue() as long[], compress); break;
				case IElementAttributeType.ArraySingle: FBXExporter7400.WriteArrayAttribute(bw, attribute.GetElementValue() as float[], compress); break;
				case IElementAttributeType.ArrayDouble: FBXExporter7400.WriteArrayAttribute(bw, attribute.GetElementValue() as double[], compress); break;
			}
		}

		private static void WriteElements(BinaryWriter bw, IElement[] elements, long start, bool compress)
		{
			for (int i = 0; i < elements.Length; ++i)
			{
				var element = elements[i];
				var current = bw.BaseStream.Position;

				bw.Write(0); // temporary offset
				bw.Write(0); // temporary count
				bw.Write(0); // temporary length

				bw.WriteStringPrefixByte(element.Name);

				var alength = bw.BaseStream.Position;

				for (int k = 0; k < element.Attributes.Length; ++k)
				{
					FBXExporter7400.WriteAttribute(bw, element.Attributes[k], compress);
				}

				var blength = bw.BaseStream.Position;

				if (element.Children.Length != 0)
				{
					FBXExporter7400.WriteElements(bw, element.Children, start, compress);
				}

				var tailpos = bw.BaseStream.Position;

				bw.BaseStream.Position = current;

				bw.Write((int)(tailpos - start));
				bw.Write(element.Attributes.Length);
				bw.Write((int)(blength - alength));

				bw.BaseStream.Position = tailpos;
			}

			bw.Write(0);
			bw.Write(0);
			bw.Write(0);
			bw.Write((byte)0);
		}

		public void Save(Stream stream, in Options options)
		{
			if (!stream.CanWrite)
			{
				throw new Exception($"Cannot write to stream provided");
			}

			var main = new IElement[11];

			main[0] = this.GetFBXHeaderExtension(options);
			main[1] = this.GetFileId(options);
			main[2] = this.GetCreationTime(options);
			main[3] = this.GetCreator(options);
			main[4] = this.GetGlobalSettings();
			main[5] = this.GetDocuments();
			main[6] = this.GetReferences();
			main[7] = this.GetDefinitions();
			main[8] = this.GetObjects();
			main[9] = this.GetConnections();
			main[10] = this.GetTakes();

			using (var bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, true))
			{
				var start = stream.Position;

				bw.WriteManaged(Header.CreateNew(7400));

				FBXExporter7400.WriteElements(bw, main, start, options.CompressData);

				bw.Write(FBXExporter7400.GetEncryption(options.SaveTime));

				bw.FillBuffer(0x10);

				bw.WriteManaged(Footer.CreateNew(7400));
			}
		}
	}
}
