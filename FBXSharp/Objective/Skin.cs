using FBXSharp.Core;
using System;
using System.Collections.Generic;

namespace FBXSharp.Objective
{
	public class Skin : FBXObject
	{
		private readonly List<Cluster> m_clusters;

		public static readonly FBXObjectType FType = FBXObjectType.Skin;

		public static readonly FBXClassType FClass = FBXClassType.Deformer;

		public override FBXObjectType Type => Skin.FType;

		public override FBXClassType Class => Skin.FClass;

		public double DeformAccuracy { get; set; }

		public IReadOnlyList<Cluster> Clusters => this.m_clusters;

		internal Skin(IElement element, IScene scene) : base(element, scene)
		{
			this.m_clusters = new List<Cluster>();

			if (element is null)
			{
				return;
			}

			var deformAccuracy = element.FindChild("Link_DeformAcuracy");

			if (deformAccuracy is null || deformAccuracy.Attributes.Length == 0 || deformAccuracy.Attributes[0].Type != IElementAttributeType.Double)
			{
				return;
			}

			this.DeformAccuracy = Convert.ToDouble(deformAccuracy.Attributes[0].GetElementValue());
		}

		public void AddCluster(Cluster cluster)
		{
			this.AddClusterAt(cluster, this.m_clusters.Count);
		}
		public void RemoveCluster(Cluster cluster)
		{
			if (cluster is null || cluster.Scene != this.Scene)
			{
				return;
			}

			_ = this.m_clusters.Remove(cluster);
		}
		public void AddClusterAt(Cluster cluster, int index)
		{
			if (cluster is null)
			{
				return;
			}

			if (cluster.Scene != this.Scene)
			{
				throw new Exception("Cluster should share same scene with skin");
			}

			if (index < 0 || index > this.m_clusters.Count)
			{
				throw new ArgumentOutOfRangeException("Index should be in range 0 to cluster count inclusively");
			}

			this.m_clusters.Insert(index, cluster);
		}
		public void RemoveClusterAt(int index)
		{
			if (index < 0 || index >= this.m_clusters.Count)
			{
				throw new ArgumentOutOfRangeException("Index should be in 0 to cluster count range");
			}

			this.m_clusters.RemoveAt(index);
		}

		public override Connection[] GetConnections()
		{
			if (this.m_clusters.Count == 0)
			{
				return Array.Empty<Connection>();
			}

			var thisHashKey = this.GetHashCode();
			var connections = new Connection[this.m_clusters.Count];

			for (int i = 0; i < connections.Length; ++i)
			{
				connections[i] = new Connection(Connection.ConnectionType.Object, this.m_clusters[i].GetHashCode(), thisHashKey);
			}

			return connections;
		}

		public override void ResolveLink(FBXObject linker, IElementAttribute attribute)
		{
			if (linker.Class == FBXClassType.Deformer && linker.Type == FBXObjectType.Cluster)
			{
				this.AddCluster(linker as Cluster);
			}
		}

		public override IElement AsElement(bool binary)
		{
			bool hasAnyProperties = this.Properties.Count != 0;

			var elements = new IElement[2 + (hasAnyProperties ? 1 : 0)];

			elements[0] = Element.WithAttribute("Version", ElementaryFactory.GetElementAttribute(101));
			elements[1] = Element.WithAttribute("Link_DeformAcuracy", ElementaryFactory.GetElementAttribute(this.DeformAccuracy));

			if (hasAnyProperties)
			{
				elements[2] = this.BuildProperties70();
			}

			return new Element(this.Class.ToString(), elements, this.BuildAttributes("Deformer", this.Type.ToString(), binary));
		}
	}
}
