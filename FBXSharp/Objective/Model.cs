﻿using FBXSharp.Core;
using FBXSharp.ValueTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FBXSharp.Objective
{
	public abstract class Model : FBXObject
	{
		private readonly List<Model> m_children;
		private readonly ReadOnlyCollection<Model> m_readonly;
		private Model m_parent;

		public static readonly FBXObjectType FType = FBXObjectType.Model;

		public override FBXObjectType Type => Model.FType;

		public abstract bool SupportsAttribute { get; }

		public abstract NodeAttribute Attribute { get; set; }

		public Model Parent => this.m_parent;

		public ReadOnlyCollection<Model> Children => this.m_readonly;

		public int MultiLayer { get; set; } = 0;

		public int MultiTake { get; set; } = 0;

		public bool Shading { get; set; } = true;

		public string Culling { get; set; } = "CullingOff";

		public string NodeFlags { get; set; }
		
		public Enumeration RotationOrder
		{
			get => this.InternalGetEnumeration(nameof(this.RotationOrder));
			set => this.InternalSetEnumeration(nameof(this.RotationOrder), value, "enum", System.String.Empty);
		}

		public Vector3? RotationOffset
		{
			get => this.InternalGetPrimitive<Vector3>(nameof(this.RotationOffset), IElementPropertyType.Double3);
			set => this.InternalSetPrimitive<Vector3>(nameof(this.RotationOffset), IElementPropertyType.Double3, value, "Vector3D", "Vector3D", IElementPropertyFlags.Animatable);
		}

		public Vector3? RotationPivot
		{
			get => this.InternalGetPrimitive<Vector3>(nameof(this.RotationPivot), IElementPropertyType.Double3);
			set => this.InternalSetPrimitive<Vector3>(nameof(this.RotationPivot), IElementPropertyType.Double3, value, "Vector3D", "Vector3D", IElementPropertyFlags.Animatable);
		}

		public Vector3? PreRotation
		{
			get => this.InternalGetPrimitive<Vector3>(nameof(this.PreRotation), IElementPropertyType.Double3);
			set => this.InternalSetPrimitive<Vector3>(nameof(this.PreRotation), IElementPropertyType.Double3, value, "Vector3D", "Vector3D", IElementPropertyFlags.Animatable);
		}

		public Vector3? PostRotation
		{
			get => this.InternalGetPrimitive<Vector3>(nameof(this.PostRotation), IElementPropertyType.Double3);
			set => this.InternalSetPrimitive<Vector3>(nameof(this.PostRotation), IElementPropertyType.Double3, value, "Vector3D", "Vector3D", IElementPropertyFlags.Animatable);
		}

		public Vector3? ScalingOffset
		{
			get => this.InternalGetPrimitive<Vector3>(nameof(this.ScalingOffset), IElementPropertyType.Double3);
			set => this.InternalSetPrimitive<Vector3>(nameof(this.ScalingOffset), IElementPropertyType.Double3, value, "Vector3D", "Vector3D", IElementPropertyFlags.Animatable);
		}

		public Vector3? ScalingPivot
		{
			get => this.InternalGetPrimitive<Vector3>(nameof(this.ScalingPivot), IElementPropertyType.Double3);
			set => this.InternalSetPrimitive<Vector3>(nameof(this.ScalingPivot), IElementPropertyType.Double3, value, "Vector3D", "Vector3D", IElementPropertyFlags.Animatable);
		}

		public Vector3? LocalTranslation
		{
			get => this.InternalGetPrimitive<Vector3>("Lcl Translation", IElementPropertyType.Double3);
			set => this.InternalSetPrimitive<Vector3>("Lcl Translation", IElementPropertyType.Double3, value, "Lcl Translation", "Lcl Translation", IElementPropertyFlags.Animatable);
		}

		public Vector3? LocalRotation
		{
			get => this.InternalGetPrimitive<Vector3>("Lcl Rotation", IElementPropertyType.Double3);
			set => this.InternalSetPrimitive<Vector3>("Lcl Rotation", IElementPropertyType.Double3, value, "Lcl Rotation", "Lcl Rotation", IElementPropertyFlags.Animatable);
		}

		public Vector3? LocalScale
		{
			get => this.InternalGetPrimitive<Vector3>("Lcl Scaling", IElementPropertyType.Double3);
			set => this.InternalSetPrimitive<Vector3>("Lcl Scaling", IElementPropertyType.Double3, value, "Lcl Scaling", "Lcl Scaling", IElementPropertyFlags.Animatable);
		}

		internal Model(IElement element, IScene scene) : base(element, scene)
		{
			this.m_children = new List<Model>();
			this.m_readonly = new ReadOnlyCollection<Model>(this.m_children);
			this.ParseDepthFields(element);
		}

		private void ParseDepthFields(IElement element)
		{
			if (element is null)
			{
				return;
			}

			var multiLayer = element.FindChild(nameof(this.MultiLayer));
			var multiTake = element.FindChild(nameof(this.MultiTake));
			var shading = element.FindChild(nameof(this.Shading));
			var culling = element.FindChild(nameof(this.Culling));

			if (!(multiLayer is null) && multiLayer.Attributes.Length > 0)
			{
				this.MultiLayer = Convert.ToInt32(multiLayer.Attributes[0].GetElementValue());
			}

			if (!(multiTake is null) && multiTake.Attributes.Length > 0)
			{
				this.MultiTake = Convert.ToInt32(multiTake.Attributes[0].GetElementValue());
			}

			if (!(shading is null) && shading.Attributes.Length > 0)
			{
				this.Shading = Convert.ToBoolean(shading.Attributes[0].GetElementValue());
			}

			if (!(culling is null) && culling.Attributes.Length > 0)
			{
				this.Culling = culling.Attributes[0].GetElementValue().ToString();
			}
		}

		internal void InternalSetChild(Model child)
		{
			this.m_children.Add(child);
			child.InternalSetParent(this);
		}
		internal void InternalSetParent(Model parent)
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

		public override Connection[] GetConnections()
		{
			if (this.m_children.Count == 0)
			{
				if (!this.SupportsAttribute || this.Attribute is null)
				{
					return Array.Empty<Connection>();
				}
				else
				{
					return new Connection[1]
					{
						new Connection(Connection.ConnectionType.Object, this.Attribute.GetHashCode(), this.GetHashCode()),
					};
				}
			}

			var attributeOn = this.SupportsAttribute && !(this.Attribute is null);
			var connections = new Connection[this.m_children.Count + (attributeOn ? 1 : 0)];

			for (int i = 0; i < connections.Length; ++i)
			{
				connections[i] = new Connection(Connection.ConnectionType.Object, this.m_children[i].GetHashCode(), this.GetHashCode());
			}

			if (attributeOn)
			{
				connections[this.m_children.Count] = new Connection(Connection.ConnectionType.Object, this.Attribute.GetHashCode(), this.GetHashCode());
			}

			return connections;
		}
	}

	public abstract class NodeAttribute : FBXObject
	{
		public static readonly FBXObjectType FType = FBXObjectType.NodeAttribute;

		public override FBXObjectType Type => NodeAttribute.FType;

		internal NodeAttribute(IElement element, IScene scene) : base(element, scene)
		{
		}
	}
}
