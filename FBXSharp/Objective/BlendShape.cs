using FBXSharp.Core;
using System;
using System.Collections.Generic;

namespace FBXSharp.Objective
{
	public class BlendShape : FBXObject
	{
		private readonly List<BlendShapeChannel> m_channels;

		public static readonly FBXObjectType FType = FBXObjectType.BlendShape;

		public static readonly FBXClassType FClass = FBXClassType.Deformer;

		public override FBXObjectType Type => BlendShape.FType;

		public override FBXClassType Class => BlendShape.FClass;

		public IReadOnlyList<BlendShapeChannel> Channels => this.m_channels;

		internal BlendShape(IElement element, IScene scene) : base(element, scene)
		{
			this.m_channels = new List<BlendShapeChannel>();
		}

		public void AddChannel(BlendShapeChannel channel)
		{
			if (channel is null)
			{
				return;
			}

			if (channel.Scene != this.Scene)
			{
				throw new Exception("Blend shape channel should share same scene with blend shape");
			}

			this.m_channels.Add(channel);
		}
		public void RemoveChannel(BlendShapeChannel channel)
		{
			if (channel is null || channel.Scene != this.Scene)
			{
				return;
			}

			_ = this.m_channels.Remove(channel);
		}
		public void AddChannelAt(BlendShapeChannel channel, int index)
		{
			if (channel is null)
			{
				return;
			}

			if (channel.Scene != this.Scene)
			{
				throw new Exception("Blend shape channel should share same scene with blend shape");
			}

			if (index < 0 || index > this.m_channels.Count)
			{
				throw new ArgumentOutOfRangeException("Index should be in range 0 to blend shape channel count inclusively");
			}

			this.m_channels.Insert(index, channel);
		}
		public void RemoveChannelAt(int index)
		{
			if (index < 0 || index >= this.m_channels.Count)
			{
				throw new ArgumentOutOfRangeException("Index should be in 0 to blend shape channel count range");
			}

			this.m_channels.RemoveAt(index);
		}

		public override Connection[] GetConnections()
		{
			if (this.m_channels.Count == 0)
			{
				return Array.Empty<Connection>();
			}
			else
			{
				int thisHashKey = this.GetHashCode();
				var connections = new Connection[this.m_channels.Count];

				for (int i = 0; i < connections.Length; ++i)
				{
					connections[i] = new Connection(Connection.ConnectionType.Object, this.m_channels[i].GetHashCode(), thisHashKey);
				}

				return connections;
			}
		}

		public override void ResolveLink(FBXObject linker, IElementAttribute attribute)
		{
			if (linker.Class == FBXClassType.Deformer && linker.Type == FBXObjectType.BlendShapeChannel)
			{
				this.AddChannel(linker as BlendShapeChannel);
			}
		}

		public override IElement AsElement(bool binary)
		{
			bool hasAnyProperties = this.Properties.Count != 0;

			var elements = new IElement[1 + (hasAnyProperties ? 1 : 0)];

			elements[0] = Element.WithAttribute("Version", ElementaryFactory.GetElementAttribute(100));

			if (hasAnyProperties)
			{
				elements[1] = this.BuildProperties70();
			}

			return new Element(this.Class.ToString(), elements, this.BuildAttributes("Deformer", this.Type.ToString(), binary));
		}
	}
}
