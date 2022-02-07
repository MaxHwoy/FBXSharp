using FBXSharp.Core;
using FBXSharp.Objective;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FBXSharp
{
	public class Scene : IScene
	{
		public enum TemplateCreationType
		{
			DontCreateIfDuplicated,
			MergeIfExistingIsFound,
			NewOverrideAnyExisting,
		}

		private readonly Root m_root;
		private readonly GlobalSettings m_settings;
		private readonly List<FBXObject> m_objects;
		private readonly List<TakeInfo> m_takeInfos;
		private readonly List<TemplateObject> m_templates;
		private readonly ReadOnlyCollection<FBXObject> m_readonlyObjects;
		private readonly ReadOnlyCollection<TakeInfo> m_readonlyTakeInfos;
		private readonly ReadOnlyCollection<TemplateObject> m_readonlyTemplates;

		public Root RootNode => this.m_root;

		public FBXObject Root => this.m_root;

		public GlobalSettings Settings => this.m_settings;

		public ReadOnlyCollection<FBXObject> Objects => this.m_readonlyObjects;

		public ReadOnlyCollection<TakeInfo> TakeInfos => this.m_readonlyTakeInfos;

		public ReadOnlyCollection<TemplateObject> Templates => this.m_readonlyTemplates;

		public Scene()
		{
			this.m_root = new Root(this);
			this.m_objects = new List<FBXObject>();
			this.m_takeInfos = new List<TakeInfo>();
			this.m_templates = new List<TemplateObject>();
			this.m_settings = new GlobalSettings(null, this);
			this.m_readonlyObjects = new ReadOnlyCollection<FBXObject>(this.m_objects);
			this.m_readonlyTakeInfos = new ReadOnlyCollection<TakeInfo>(this.m_takeInfos);
			this.m_readonlyTemplates = new ReadOnlyCollection<TemplateObject>(this.m_templates);
		}

		private T AddObjectAndReturn<T>(T value) where T : FBXObject
		{
			this.m_objects.Add(value);
			return value;
		}

		internal void InternalAddObject(FBXObject @object) => this.m_objects.Add(@object);
		internal void InternalSetTakeInfos(TakeInfo[] takeInfos) => this.m_takeInfos.AddRange(takeInfos ?? Array.Empty<TakeInfo>());
		internal void InternalSetTemplates(TemplateObject[] templates) => this.m_templates.AddRange(templates ?? Array.Empty<TemplateObject>());

		public TemplateObject GetTemplateObject(string name) => this.m_templates.Find(_ => _.Name == name);
		public TemplateObject GetTemplateObject(FBXObjectType objectType) => this.m_templates.Find(_ => _.OverridableType == objectType);

		public TemplateObject CreateEmptyTemplate(FBXObjectType objectType, TemplateCreationType creationType)
		{
			var template = this.m_templates.Find(_ => _.OverridableType == objectType);

			if (template is null)
			{
				this.m_templates.Add(template = new TemplateObject(objectType, null, this));

				return template;
			}
			else
			{
				if (creationType == TemplateCreationType.NewOverrideAnyExisting)
				{
					template.RemoveAllProperties();
					template.Name = String.Empty;
				}

				return template;
			}
		}
		public TemplateObject CreatePredefinedTemplate(FBXObjectType objectType, TemplateCreationType creationType)
		{
			var indexer = this.m_templates.FindIndex(_ => _.OverridableType == objectType);

			if (indexer < 0)
			{
				var template = TemplateFactory.GetTemplateForType(objectType, this);

				this.m_templates.Add(template);

				return template;
			}
			else
			{
				if (creationType == TemplateCreationType.DontCreateIfDuplicated)
				{
					return this.m_templates[indexer];
				}

				if (creationType == TemplateCreationType.NewOverrideAnyExisting)
				{
					this.m_templates[indexer] = TemplateFactory.GetTemplateForType(objectType, this);

					return this.m_templates[indexer];
				}

				if (creationType == TemplateCreationType.MergeIfExistingIsFound)
				{
					this.m_templates[indexer].MergeWith(TemplateFactory.GetTemplateForType(objectType, null));

					return this.m_templates[indexer];
				}

				throw new Exception($"Template creation type passed is invalid");
			}
		}

		public FBXObject CreateFBXObject(FBXObjectType type)
		{
			switch (type)
			{
				case FBXObjectType.Video: return this.CreateVideo();
				case FBXObjectType.Texture: return this.CreateTexture();
				case FBXObjectType.Material: return this.CreateMaterial();
				case FBXObjectType.Geometry: return this.CreateGeometry();
				case FBXObjectType.Shape: return this.CreateShape();

				case FBXObjectType.Cluster: return this.CreateCluster();
				case FBXObjectType.Skin: return this.CreateSkin();
				case FBXObjectType.BlendShape: return this.CreateBlendShape();
				case FBXObjectType.BlendShapeChannel: return this.CreateBlendShapeChannel();

				case FBXObjectType.AnimationStack: return this.CreateAnimationStack();
				case FBXObjectType.AnimationLayer: return this.CreateAnimationLayer();
				case FBXObjectType.AnimationCurve: return this.CreateAnimationCurve();
				case FBXObjectType.AnimationCurveNode: return this.CreateAnimationCurveNode();

				case FBXObjectType.Pose:
				case FBXObjectType.Deformer:
				default: return null;
			}
		}

		public Video CreateVideo() => this.AddObjectAndReturn(new Video(null, this));
		public Texture CreateTexture() => this.AddObjectAndReturn(new Texture(null, this));
		public Material CreateMaterial() => this.AddObjectAndReturn(new Material(null, this));
		public Geometry CreateGeometry() => this.AddObjectAndReturn(new Geometry(null, this));
		public Shape CreateShape() => this.AddObjectAndReturn(new Shape(null, this));

		public Cluster CreateCluster() => this.AddObjectAndReturn(new Cluster(null, this));
		public Skin CreateSkin() => this.AddObjectAndReturn(new Skin(null, this));
		public BlendShape CreateBlendShape() => this.AddObjectAndReturn(new BlendShape(null, this));
		public BlendShapeChannel CreateBlendShapeChannel() => this.AddObjectAndReturn(new BlendShapeChannel(null, this));

		public AnimationStack CreateAnimationStack() => this.AddObjectAndReturn(new AnimationStack(null, this));
		public AnimationLayer CreateAnimationLayer() => this.AddObjectAndReturn(new AnimationLayer(null, this));
		public AnimationCurve CreateAnimationCurve() => this.AddObjectAndReturn(new AnimationCurve(null, this));
		public AnimationCurveNode CreateAnimationCurveNode() => this.AddObjectAndReturn(new AnimationCurveNode(null, this));

		public Mesh CreateMesh() => this.AddObjectAndReturn(new Mesh(null, this));
		public Light CreateLight() => this.AddObjectAndReturn(new Light(null, this));
		public Camera CreateCamera() => this.AddObjectAndReturn(new Camera(null, this));
		public NullNode CreateNullNode() => this.AddObjectAndReturn(new NullNode(null, this));

		public NullAttribute CreateNullAttribute() => this.AddObjectAndReturn(new NullAttribute(null, this));
		public LightAttribute CreateLightAttribute() => this.AddObjectAndReturn(new LightAttribute(null, this));
		public CameraAttribute CreateCameraAttribute() => this.AddObjectAndReturn(new CameraAttribute(null, this));

		public void DestroyFBXObject(FBXObject @object)
		{
			if (@object is null)
			{
				return;
			}

			if (this.m_objects.Remove(@object))
			{
				@object.Destroy();
			}
		}
	}
}
