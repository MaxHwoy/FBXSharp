using FBXSharp.Core;
using System;
using System.Collections.Generic;

namespace FBXSharp.Objective
{
	public class AnimationLayer : FBXObject
	{
		private readonly List<AnimationCurveNode> m_curves;

		public static readonly FBXObjectType FType = FBXObjectType.AnimationLayer;

		public static readonly FBXClassType FClass = FBXClassType.AnimationLayer;

		public override FBXObjectType Type => AnimationLayer.FType;

		public override FBXClassType Class => AnimationLayer.FClass;

		public IReadOnlyList<AnimationCurveNode> CurveNodes => this.m_curves;

		internal AnimationLayer(IElement element, IScene scene) : base(element, scene)
		{
			this.m_curves = new List<AnimationCurveNode>();
		}

		public override Connection[] GetConnections()
		{
			if (this.m_curves.Count == 0)
			{
				return Array.Empty<Connection>();
			}

			var thisHashKey = this.GetHashCode();
			var connections = new Connection[this.m_curves.Count];

			for (int i = 0; i < connections.Length; ++i)
			{
				connections[i] = new Connection(Connection.ConnectionType.Object, this.m_curves[i].GetHashCode(), thisHashKey);
			}

			return connections;
		}

		public override void ResolveLink(FBXObject linker, IElementAttribute attribute)
		{
			if (linker.Class == FBXClassType.AnimationCurveNode && linker.Type == FBXObjectType.AnimationCurveNode)
			{
				this.m_curves.Add(linker as AnimationCurveNode); // #TODO
			}
		}

		public override IElement AsElement(bool binary)
		{
			var elements = this.Properties.Count == 0
				? Array.Empty<IElement>()
				: new IElement[] { this.BuildProperties70() };

			return new Element(this.Class.ToString(), elements, this.BuildAttributes("AnimLayer", String.Empty, binary));
		}
	}
}
