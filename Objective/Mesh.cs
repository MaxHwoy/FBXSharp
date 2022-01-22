using FBXSharp.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FBXSharp.Objective
{
	public class Mesh : NullNode
	{
		private readonly List<Material> m_materials;
		private readonly ReadOnlyCollection<Material> m_readonly;
		private Geometry m_geometry;
		private Pose m_pose;

		public static new readonly FBXObjectType FType = FBXObjectType.Mesh;

		public override FBXObjectType Type => Mesh.FType;

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

		//virtual const Pose* getPose() const = 0;
		//virtual const Geometry* getGeometry() const = 0;
		//virtual Matrix getGeometricMatrix() const = 0;
		//virtual const Material* getMaterial(int idx) const = 0;
		//virtual int getMaterialCount() const = 0;
	}
}
