using FBXSharp.Core;
using System;
using System.Collections.Generic;

namespace FBXSharp.Objective
{
	public class AnimationStack : FBXObject
	{
		private readonly List<AnimationLayer> m_layers;

		public static readonly FBXObjectType FType = FBXObjectType.AnimationStack;

		public static readonly FBXClassType FClass = FBXClassType.AnimationStack;

		public override FBXObjectType Type => AnimationStack.FType;

		public override FBXClassType Class => AnimationStack.FClass;

		public IReadOnlyList<AnimationLayer> Layers => this.m_layers;

		internal AnimationStack(IElement element, IScene scene) : base(element, scene)
		{
			this.m_layers = new List<AnimationLayer>();
		}

		public override Connection[] GetConnections()
		{
			if (this.m_layers.Count == 0)
			{
				return Array.Empty<Connection>();
			}

			var thisHashKey = this.GetHashCode();
			var connections = new Connection[this.m_layers.Count];

			for (int i = 0; i < connections.Length; ++i)
			{
				connections[i] = new Connection(Connection.ConnectionType.Object, this.m_layers[i].GetHashCode(), thisHashKey);
			}

			return connections;
		}

		public override void ResolveLink(FBXObject linker, IElementAttribute attribute)
		{
			if (linker.Class == FBXClassType.AnimationLayer && linker.Type == FBXObjectType.AnimationLayer)
			{
				this.m_layers.Add(linker as AnimationLayer); // #TODO
			}
		}

		public override IElement AsElement(bool binary)
		{
			var elements = this.Properties.Count == 0
				? Array.Empty<IElement>()
				: new IElement[] { this.BuildProperties70() };

			return new Element(this.Class.ToString(), elements, this.BuildAttributes("AnimStack", String.Empty, binary));
		}
	}
}
