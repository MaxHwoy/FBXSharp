using FBXSharp.Core;
using FBXSharp.ValueTypes;
using System;

namespace FBXSharp.Objective
{
	public class Camera : Model
	{
		private CameraAttribute m_attribute;

		public override bool SupportsAttribute => true;

		public override NodeAttribute Attribute
		{
			get => this.m_attribute;
			set => this.SetNodeAttribute(value);
		}

		internal Camera(IElement element, IScene scene) : base(element, scene)
		{
		}

		private void SetNodeAttribute(NodeAttribute attribute)
		{
			if (attribute is CameraAttribute camera)
			{
				this.m_attribute = camera;
			}
			else
			{
				throw new ArgumentException("Node Attribute passed should be of CameraAttribute type");
			}
		}

		public override IElement AsElement()
		{
			return this.MakeElement("Camera");
		}
	}

	public class CameraAttribute : NodeAttribute
	{
		public Vector3 Position { get; set; }

		public Vector3 Up { get; set; }

		public Vector3 LookAt { get; set; }

		public bool ShowInfoOnMoving { get; set; }

		public bool ShowAudio { get; set; }

		public ColorRGB AudioColor { get; set; }

		public double CameraOrthoZoom { get; set; }

		internal CameraAttribute(IElement element, IScene scene) : base(element, scene)
		{
			if (element is null)
			{
				return;
			}

			var position = element.FindChild(nameof(this.Position));
			var up = element.FindChild(nameof(this.Up));
			var lookAt = element.FindChild(nameof(this.LookAt));
			var showInfo = element.FindChild(nameof(this.ShowInfoOnMoving));
			var showAudio = element.FindChild(nameof(this.ShowAudio));
			var audioColor = element.FindChild(nameof(this.AudioColor));
			var orthoZoom = element.FindChild(nameof(this.CameraOrthoZoom));

			if (!(position is null) && position.Attributes.Length > 2)
			{
				this.Position = new Vector3()
				{
					X = Convert.ToDouble(position.Attributes[0].GetElementValue()),
					Y = Convert.ToDouble(position.Attributes[1].GetElementValue()),
					Z = Convert.ToDouble(position.Attributes[2].GetElementValue()),
				};
			}

			if (!(up is null) && up.Attributes.Length > 2)
			{
				this.Up = new Vector3()
				{
					X = Convert.ToDouble(up.Attributes[0].GetElementValue()),
					Y = Convert.ToDouble(up.Attributes[1].GetElementValue()),
					Z = Convert.ToDouble(up.Attributes[2].GetElementValue()),
				};
			}

			if (!(lookAt is null) && lookAt.Attributes.Length > 2)
			{
				this.LookAt = new Vector3()
				{
					X = Convert.ToDouble(lookAt.Attributes[0].GetElementValue()),
					Y = Convert.ToDouble(lookAt.Attributes[1].GetElementValue()),
					Z = Convert.ToDouble(lookAt.Attributes[2].GetElementValue()),
				};
			}

			if (!(showInfo is null) && showInfo.Attributes.Length > 0)
			{
				this.ShowInfoOnMoving = Convert.ToBoolean(showInfo.Attributes[0].GetElementValue());
			}
			else
			{
				this.ShowInfoOnMoving = true;
			}

			if (!(showAudio is null) && showAudio.Attributes.Length > 0)
			{
				this.ShowAudio = Convert.ToBoolean(showAudio.Attributes[0].GetElementValue());
			}
			else
			{
				this.ShowAudio = false;
			}

			if (!(audioColor is null) && audioColor.Attributes.Length > 2)
			{
				this.AudioColor = new ColorRGB()
				{
					R = Convert.ToDouble(audioColor.Attributes[0].GetElementValue()),
					G = Convert.ToDouble(audioColor.Attributes[1].GetElementValue()),
					B = Convert.ToDouble(audioColor.Attributes[2].GetElementValue()),
				};
			}
			else
			{
				this.AudioColor = new ColorRGB(0.0, 1.0, 0.0);
			}

			if (!(orthoZoom is null) && orthoZoom.Attributes.Length > 0)
			{
				this.CameraOrthoZoom = Convert.ToDouble(orthoZoom.Attributes[0].GetElementValue());
			}
			else
			{
				this.CameraOrthoZoom = 1.0;
			}
		}

		public override IElement AsElement()
		{
			var elements = new IElement[10];

			elements[0] = this.BuildProperties70();
			elements[1] = Element.WithAttribute("TypeFlags", ElementaryFactory.GetElementAttribute("Camera"));
			elements[2] = Element.WithAttribute("GeometryVersion", ElementaryFactory.GetElementAttribute(124));

			elements[3] = new Element(nameof(this.Position), null, new IElementAttribute[]
			{
				ElementaryFactory.GetElementAttribute(this.Position.X),
				ElementaryFactory.GetElementAttribute(this.Position.Y),
				ElementaryFactory.GetElementAttribute(this.Position.Z),
			});

			elements[4] = new Element(nameof(this.Up), null, new IElementAttribute[]
			{
				ElementaryFactory.GetElementAttribute(this.Up.X),
				ElementaryFactory.GetElementAttribute(this.Up.Y),
				ElementaryFactory.GetElementAttribute(this.Up.Z),
			});

			elements[5] = new Element(nameof(this.LookAt), null, new IElementAttribute[]
			{
				ElementaryFactory.GetElementAttribute(this.LookAt.X),
				ElementaryFactory.GetElementAttribute(this.LookAt.Y),
				ElementaryFactory.GetElementAttribute(this.LookAt.Z),
			});

			elements[8] = new Element(nameof(this.AudioColor), null, new IElementAttribute[]
			{
				ElementaryFactory.GetElementAttribute(this.AudioColor.R),
				ElementaryFactory.GetElementAttribute(this.AudioColor.G),
				ElementaryFactory.GetElementAttribute(this.AudioColor.B),
			});

			elements[6] = Element.WithAttribute(nameof(this.ShowInfoOnMoving), ElementaryFactory.GetElementAttribute(this.ShowInfoOnMoving));
			elements[7] = Element.WithAttribute(nameof(this.ShowAudio), ElementaryFactory.GetElementAttribute(this.ShowAudio));
			elements[9] = Element.WithAttribute(nameof(this.CameraOrthoZoom), ElementaryFactory.GetElementAttribute(this.CameraOrthoZoom));

			return new Element("NodeAttribute", elements, this.BuildAttributes("Camera"));
		}
	}
}
