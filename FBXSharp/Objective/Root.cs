using FBXSharp.Core;
using System;

namespace FBXSharp.Objective
{
	public class Root : Model
	{
		public static readonly FBXObjectType FType = FBXObjectType.Root;

		public override FBXObjectType Type => Root.FType;

		public override bool SupportsAttribute => false;

		internal Root(IScene scene) : base(null, scene)
		{
			this.Name = "RootNode";
		}

		public override Connection[] GetConnections()
		{
			if (this.Children.Count == 0)
			{
				return Array.Empty<Connection>();
			}

			var connections = new Connection[this.Children.Count];

			for (int i = 0; i < connections.Length; ++i)
			{
				connections[i] = new Connection(Connection.ConnectionType.Object, this.Children[i].GetHashCode(), 0L);
			}

			return connections;
		}

		public override IElement AsElement(bool binary) => throw new NotSupportedException("Root nodes cannot be serialized");
	}
}
