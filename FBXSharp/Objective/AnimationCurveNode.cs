using FBXSharp.Core;
using System;

namespace FBXSharp.Objective
{
	public class AnimationCurveNode : FBXObject
	{
		private AnimationCurve m_curveX;
		private AnimationCurve m_curveY;
		private AnimationCurve m_curveZ;

		public static readonly FBXObjectType FType = FBXObjectType.AnimationCurveNode;

		public static readonly FBXClassType FClass = FBXClassType.AnimationCurveNode;

		public override FBXObjectType Type => AnimationCurveNode.FType;

		public override FBXClassType Class => AnimationCurveNode.FClass;

		public double DeltaX { get; set; }
		public double DeltaY { get; set; }
		public double DeltaZ { get; set; }

		public AnimationCurve CurveX
		{
			get => this.m_curveX;
			set => this.InternalSetCurve(value, ref this.m_curveX);
		}
		public AnimationCurve CurveY
		{
			get => this.m_curveY;
			set => this.InternalSetCurve(value, ref this.m_curveY);
		}
		public AnimationCurve CurveZ
		{
			get => this.m_curveZ;
			set => this.InternalSetCurve(value, ref this.m_curveZ);
		}

		internal AnimationCurveNode(IElement element, IScene scene) : base(element, scene)
		{
		}

		private void InternalSetCurve(AnimationCurve curve, ref AnimationCurve target)
		{
			if (curve is null || curve.Scene == this.Scene)
			{
				target = curve;

				return;
			}

			throw new Exception("Animation curve should share same scene with animation curve node");
		}

		public override Connection[] GetConnections()
		{
			bool noCurveX = this.m_curveX is null;
			bool noCurveY = this.m_curveY is null;
			bool noCurveZ = this.m_curveZ is null;

			if (noCurveX && noCurveY && noCurveZ)
			{
				return Array.Empty<Connection>();
			}

			int currentlyAt = 0;
			int thisHashKey = this.GetHashCode();
			var connections = new Connection[(noCurveX ? 0 : 1) + (noCurveY ? 0 : 1) + (noCurveZ ? 0 : 1)];

			if (!noCurveX)
			{
				connections[currentlyAt++] = new Connection
				(
					Connection.ConnectionType.Property,
					this.m_curveX.GetHashCode(),
					thisHashKey,
					ElementaryFactory.GetElementAttribute("d|X")
				);
			}

			if (!noCurveY)
			{
				connections[currentlyAt++] = new Connection
				(
					Connection.ConnectionType.Property,
					this.m_curveY.GetHashCode(),
					thisHashKey,
					ElementaryFactory.GetElementAttribute("d|Y")
				);
			}

			if (!noCurveZ)
			{
				connections[currentlyAt++] = new Connection
				(
					Connection.ConnectionType.Property,
					this.m_curveZ.GetHashCode(),
					thisHashKey,
					ElementaryFactory.GetElementAttribute("d|Z")
				);
			}

			return connections;
		}

		public override void ResolveLink(FBXObject linker, IElementAttribute attribute)
		{
			if (linker.Class == FBXClassType.AnimationCurve && linker.Type == FBXObjectType.AnimationCurve)
			{
				if (attribute.Type == IElementAttributeType.String)
				{
					switch (attribute.GetElementValue().ToString())
					{
						case "d|X": this.CurveX = linker as AnimationCurve; return;
						case "d|Y": this.CurveY = linker as AnimationCurve; return;
						case "d|Z": this.CurveZ = linker as AnimationCurve; return;
					}
				}
			}
		}

		public override IElement AsElement(bool binary)
		{
			var elements = this.Properties.Count == 0
				? Array.Empty<IElement>()
				: new IElement[] { this.BuildProperties70() };

			return new Element(this.Class.ToString(), elements, this.BuildAttributes("AnimCurveNode", String.Empty, binary));
		}
	}
}
