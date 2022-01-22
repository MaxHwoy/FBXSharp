using FBXSharp.Core;
using FBXSharp.ValueTypes;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FBXSharp.Objective
{
	public class NullNode : FBXObject
	{
		private readonly List<NullNode> m_children;
		private readonly ReadOnlyCollection<NullNode> m_readonly;
		private NullNode m_parent;

		public static readonly FBXObjectType FType = FBXObjectType.NullNode;

		public override FBXObjectType Type => NullNode.FType;

		public NullNode Parent => this.m_parent;

		public ReadOnlyCollection<NullNode> Children => this.m_readonly;

		public string NodeFlags { get; set; }
		
		public Enumeration RotationOrder
		{
			get => this.InternalGetEnumeration(nameof(this.RotationOrder));
			set => this.InternalSetEnumeration(nameof(this.RotationOrder), value, "enum", System.String.Empty);
		}

		public Vector3? RotationOffset
		{
			get => this.InternalGetPrimitive<Vector3>(nameof(this.RotationOffset), IElementPropertyType.Double3);
			set => this.InternalSetPrimitive<Vector3>(nameof(this.RotationOffset), IElementPropertyType.Double3, value, "Vector3D", "Vector3D");
		}

		public Vector3? RotationPivot
		{
			get => this.InternalGetPrimitive<Vector3>(nameof(this.RotationPivot), IElementPropertyType.Double3);
			set => this.InternalSetPrimitive<Vector3>(nameof(this.RotationPivot), IElementPropertyType.Double3, value, "Vector3D", "Vector3D");
		}

		public Vector3? PreRotation
		{
			get => this.InternalGetPrimitive<Vector3>(nameof(this.PreRotation), IElementPropertyType.Double3);
			set => this.InternalSetPrimitive<Vector3>(nameof(this.PreRotation), IElementPropertyType.Double3, value, "Vector3D", "Vector3D");
		}

		public Vector3? PostRotation
		{
			get => this.InternalGetPrimitive<Vector3>(nameof(this.PostRotation), IElementPropertyType.Double3);
			set => this.InternalSetPrimitive<Vector3>(nameof(this.PostRotation), IElementPropertyType.Double3, value, "Vector3D", "Vector3D");
		}

		public Vector3? ScalingOffset
		{
			get => this.InternalGetPrimitive<Vector3>(nameof(this.ScalingOffset), IElementPropertyType.Double3);
			set => this.InternalSetPrimitive<Vector3>(nameof(this.ScalingOffset), IElementPropertyType.Double3, value, "Vector3D", "Vector3D");
		}

		public Vector3? ScalingPivot
		{
			get => this.InternalGetPrimitive<Vector3>(nameof(this.ScalingPivot), IElementPropertyType.Double3);
			set => this.InternalSetPrimitive<Vector3>(nameof(this.ScalingPivot), IElementPropertyType.Double3, value, "Vector3D", "Vector3D");
		}

		public Vector3? LocalTranslation
		{
			get => this.InternalGetPrimitive<Vector3>("Lcl Translation", IElementPropertyType.Double3);
			set => this.InternalSetPrimitive<Vector3>("Lcl Translation", IElementPropertyType.Double3, value, "Lcl Translation", "Lcl Translation");
		}

		public Vector3? LocalRotation
		{
			get => this.InternalGetPrimitive<Vector3>("Lcl Rotation", IElementPropertyType.Double3);
			set => this.InternalSetPrimitive<Vector3>("Lcl Rotation", IElementPropertyType.Double3, value, "Lcl Rotation", "Lcl Rotation");
		}

		public Vector3? LocalScale
		{
			get => this.InternalGetPrimitive<Vector3>("Lcl Scaling", IElementPropertyType.Double3);
			set => this.InternalSetPrimitive<Vector3>("Lcl Scaling", IElementPropertyType.Double3, value, "Lcl Scaling", "Lcl Scaling");
		}

		internal NullNode(IElement element, IScene scene) : base(element, scene)
		{
			this.m_children = new List<NullNode>();
			this.m_readonly = new ReadOnlyCollection<NullNode>(this.m_children);
		}

		internal void InternalSetChild(NullNode child)
		{
			this.m_children.Add(child);
			child.InternalSetParent(this);
		}
		internal void InternalSetParent(NullNode parent)
		{
			this.m_parent = parent;
		}

		private Matrix4x4 EvaluateLocal(in Vector3 position, in Vector3 rotation)
		{
			var scale = this.LocalScale;

			return this.EvaluateLocal(position, rotation, scale.HasValue ? scale.Value : Vector3.One);
		}

		private Matrix4x4 EvaluateLocal(in Vector3 position, in Vector3 rotation, in Vector3 scale)
		{
			var rotationPivot = this.RotationPivot.GetValueOrDefault();
			var scalingPivot = this.ScalingPivot.GetValueOrDefault();
			var rotationOrder = (RotationOrder)(this.RotationOrder?.Value ?? 0);

			var t = Matrix4x4.CreateTranslation(position);
			var s = Matrix4x4.CreateScale(scale);
			var r = Matrix4x4.CreateFromEuler(rotation, rotationOrder);

			var rpre = Matrix4x4.CreateFromEuler(this.PreRotation.GetValueOrDefault(), ValueTypes.RotationOrder.XYZ);
			var post = Matrix4x4.CreateFromEuler(-this.PostRotation.GetValueOrDefault(), ValueTypes.RotationOrder.ZYX);

			var roff = Matrix4x4.CreateTranslation(this.RotationOffset.GetValueOrDefault());
			var rpip = Matrix4x4.CreateTranslation(rotationPivot);
			var rpii = Matrix4x4.CreateTranslation(-rotationPivot);

			var soff = Matrix4x4.CreateTranslation(this.ScalingOffset.GetValueOrDefault());
			var spip = Matrix4x4.CreateTranslation(scalingPivot);
			var spii = Matrix4x4.CreateTranslation(-scalingPivot);

			return t * roff * rpip * rpre * r * post * rpii * soff * spip * s * spii;
		}

		public Matrix4x4 GetLocalTransform()
		{
			var translation = this.LocalTranslation;
			var rotation = this.LocalRotation;
			var scale = this.LocalScale;

			return this.EvaluateLocal(translation.GetValueOrDefault(), rotation.GetValueOrDefault(), scale.HasValue ? scale.Value : Vector3.One);
		}

		public Matrix4x4 GetGlobalTransform()
		{
			if (this.Parent is null)
			{
				return this.EvaluateLocal(this.LocalTranslation.GetValueOrDefault(), this.LocalRotation.GetValueOrDefault());
			}
			else
			{
				return this.Parent.GetGlobalTransform() * this.EvaluateLocal(this.LocalTranslation.GetValueOrDefault(), this.LocalRotation.GetValueOrDefault());
			}
		}
	}
}
