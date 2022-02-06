using FBXSharp.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FBXSharp.Objective
{
	public class Mesh : Model
	{
		private readonly List<Material> m_materials;
		private readonly ReadOnlyCollection<Material> m_readonly;
		private Geometry m_geometry;
		private Pose m_pose;

		public override bool SupportsAttribute => false;

		public override NodeAttribute Attribute
		{
			get => throw new NotSupportedException("Meshes do not support node attributes");
			set => throw new NotSupportedException("Meshes do not support node attributes");
		}

		public Pose Pose => this.m_pose;

		public Geometry Geometry => this.m_geometry;

		public ReadOnlyCollection<Material> Materials => this.m_readonly;

		internal Mesh(IElement element, IScene scene) : base(element, scene)
		{
			this.m_materials = new List<Material>();
			this.m_readonly = new ReadOnlyCollection<Material>(this.m_materials);
		}

		internal void InternalSetPose(Pose pose) => this.m_pose = pose;
		internal void InternalSetGeometry(Geometry geometry) => this.m_geometry = geometry;
		internal void InternalSetMaterial(Material material) => this.m_materials.Add(material);

		public void SetGeometry(Geometry geometry)
		{
			if (geometry is null)
			{
				this.m_geometry = null;
				return;
			}

			if (geometry.Scene != this.Scene)
			{
				throw new Exception("Geometry should share same scene with model");
			}

			this.m_geometry = geometry;
		}

		public void AddMaterial(Material material)
		{
			if (material is null)
			{
				throw new Exception("Cannot add null material");
			}

			if (material.Scene != this.Scene)
			{
				throw new Exception("Material should share same scene with model");
			}

			this.m_materials.Add(material);
		}
		public void RemoveMaterial(Material material)
		{
			if (material is null || material.Scene != this.Scene)
			{
				return;
			}

			_ = this.m_materials.Remove(material);
		}
		public void AddMaterialAt(Material material, int index)
		{
			if (material is null)
			{
				throw new Exception("Cannot add null material");
			}

			if (material.Scene != this.Scene)
			{
				throw new Exception("Material should share same scene with model");
			}

			if (index < 0 || index > this.m_materials.Count)
			{
				throw new ArgumentOutOfRangeException("Index should be in range 0 to material count inclusively");
			}

			this.m_materials.Insert(index, material);
		}
		public void RemoveMaterialAt(int index)
		{
			if (index < 0 || index >= this.m_materials.Count)
			{
				throw new ArgumentOutOfRangeException("Index should be in 0 to material count range");
			}

			this.m_materials.RemoveAt(index);
		}

		public override Connection[] GetConnections()
		{
			int counter = 0;
			int indexer = 0;

			counter += this.m_geometry is null ? 0 : 1;
			counter += this.m_materials.Count;
			counter += this.Children.Count;

			if (counter == 0)
			{
				return Array.Empty<Connection>();
			}

			var connections = new Connection[counter];

			if (!(this.m_geometry is null))
			{
				connections[indexer++] = new Connection(Connection.ConnectionType.Object, this.m_geometry.GetHashCode(), this.GetHashCode());
			}

			for (int i = 0; i < this.m_materials.Count; ++i)
			{
				connections[indexer++] = new Connection(Connection.ConnectionType.Object, this.m_materials[i].GetHashCode(), this.GetHashCode());
			}

			for (int i = 0; i < this.Children.Count; ++i)
			{
				connections[indexer++] = new Connection(Connection.ConnectionType.Object, this.Children[i].GetHashCode(), this.GetHashCode());
			}

			return connections;
		}

		public override IElement AsElement(bool binary)
		{
			return this.MakeElement("Mesh", binary);
		}
	}
}
