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
	}
}
