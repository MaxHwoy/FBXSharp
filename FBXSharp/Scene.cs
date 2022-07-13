using FBXSharp.Core;
using FBXSharp.Objective;
using System;
using System.Collections.Generic;

namespace FBXSharp
{
	public class Scene : IScene
	{
		private readonly Root m_root;
		private readonly GlobalSettings m_settings;
		private readonly List<FBXObject> m_objects;
		private readonly List<TakeInfo> m_takeInfos;
		private readonly List<TemplateObject> m_templates;
		private readonly Dictionary<(FBXClassType, FBXObjectType), Func<IElement, FBXObject>> m_activatorMap;

		public Root RootNode => this.m_root;

		public FBXObject Root => this.m_root;

		public GlobalSettings Settings => this.m_settings;

		public IReadOnlyList<FBXObject> Objects => this.m_objects;

		public IReadOnlyList<TakeInfo> TakeInfos => this.m_takeInfos;

		public IReadOnlyList<TemplateObject> Templates => this.m_templates;

		public Scene()
		{
			this.m_root = new Root(this);
			this.m_objects = new List<FBXObject>();
			this.m_takeInfos = new List<TakeInfo>();
			this.m_templates = new List<TemplateObject>();
			this.m_settings = new GlobalSettings(null, this);

			this.m_activatorMap = new Dictionary<(FBXClassType, FBXObjectType), Func<IElement, FBXObject>>()
			{
				// AnimationCurve
				{ (FBXClassType.AnimationCurve, FBXObjectType.AnimationCurve), (element) => new AnimationCurve(element, this) },

				// AnimationCurveNode
				{ (FBXClassType.AnimationCurveNode, FBXObjectType.AnimationCurveNode), (element) => new AnimationCurveNode(element, this) },

				// AnimationLayer
				{ (FBXClassType.AnimationLayer, FBXObjectType.AnimationLayer), (element) => new AnimationLayer(element, this) },

				// AnimationStack
				{ (FBXClassType.AnimationStack, FBXObjectType.AnimationStack), (element) => new AnimationStack(element, this) },

				// Deformer
				{ (FBXClassType.Deformer, FBXObjectType.BlendShape), (element) => new BlendShape(element, this) },
				{ (FBXClassType.Deformer, FBXObjectType.BlendShapeChannel), (element) => new BlendShapeChannel(element, this) },
				{ (FBXClassType.Deformer, FBXObjectType.Cluster), (element) => new Cluster(element, this) },
				{ (FBXClassType.Deformer, FBXObjectType.Skin), (element) => new Skin(element, this) },

				// Geometry
				{ (FBXClassType.Geometry, FBXObjectType.Mesh), (element) => new Geometry(element, this) },
				{ (FBXClassType.Geometry, FBXObjectType.Shape), (element) => new Shape(element, this) },

				// Material
				{ (FBXClassType.Material, FBXObjectType.Material), (element) => new Material(element, this) },

				// Model
				{ (FBXClassType.Model, FBXObjectType.Camera), (element) => new Camera(element, this) },
				{ (FBXClassType.Model, FBXObjectType.Light), (element) => new Light(element, this) },
				{ (FBXClassType.Model, FBXObjectType.LimbNode), (element) => new LimbNode(element, this) },
				{ (FBXClassType.Model, FBXObjectType.Mesh), (element) => new Mesh(element, this) },
				{ (FBXClassType.Model, FBXObjectType.Null), (element) => new NullNode(element, this) },

				// NodeAttribute
				{ (FBXClassType.NodeAttribute, FBXObjectType.Camera), (element) => new CameraAttribute(element, this) },
				{ (FBXClassType.NodeAttribute, FBXObjectType.Light), (element) => new LightAttribute(element, this) },
				{ (FBXClassType.NodeAttribute, FBXObjectType.LimbNode), (element) => new LimbNodeAttribute(element, this) },
				{ (FBXClassType.NodeAttribute, FBXObjectType.Null), (element) => new NullAttribute(element, this) },

				// Pose
				{ (FBXClassType.Pose, FBXObjectType.BindPose), (element) => new BindPose(element, this) },

				// Texture
				{ (FBXClassType.Texture, FBXObjectType.Texture), (element) => new Texture(element, this) },

				// Video
				{ (FBXClassType.Video, FBXObjectType.Clip), (element) => new Clip(element, this) },
			};
		}

		private T AddObjectAndReturn<T>(T value) where T : FBXObject
		{
			this.m_objects.Add(value);
			return value;
		}

		internal void InternalAddObject(FBXObject @object) => this.m_objects.Add(@object);
		internal void InternalSetTakeInfos(TakeInfo[] takeInfos) => this.m_takeInfos.AddRange(takeInfos ?? Array.Empty<TakeInfo>());
		internal void InternalSetTemplates(TemplateObject[] templates) => this.m_templates.AddRange(templates ?? Array.Empty<TemplateObject>());

		public void AddTakeInfo(TakeInfo takeInfo) => this.m_takeInfos.Add(takeInfo);
		public void RemoveTakeInfo(TakeInfo takeInfo) => this.m_takeInfos.Remove(takeInfo);

		public TemplateObject GetTemplateObject(string name) => this.m_templates.Find(_ => _.Name == name);
		public TemplateObject GetTemplateObject(FBXClassType classType) => this.m_templates.Find(_ => _.OverridableType == classType);

		public TemplateObject CreateEmptyTemplate(FBXClassType classType, TemplateCreationType creationType)
		{
			var template = this.m_templates.Find(_ => _.OverridableType == classType);

			if (template is null)
			{
				this.m_templates.Add(template = new TemplateObject(classType, null, this));

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
		public TemplateObject CreatePredefinedTemplate(FBXClassType classType, TemplateCreationType creationType)
		{
			var indexer = this.m_templates.FindIndex(_ => _.OverridableType == classType);

			if (indexer < 0)
			{
				var template = TemplateFactory.GetTemplateForType(classType, this);

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
					this.m_templates[indexer] = TemplateFactory.GetTemplateForType(classType, this);

					return this.m_templates[indexer];
				}

				if (creationType == TemplateCreationType.MergeIfExistingIsFound)
				{
					this.m_templates[indexer].MergeWith(TemplateFactory.GetTemplateForType(classType, null));

					return this.m_templates[indexer];
				}

				throw new Exception($"Template creation type passed is invalid");
			}
		}

		public FBXObject CreateFBXObject(FBXClassType classType, FBXObjectType objectType, IElement element = null)
		{
			if (this.m_activatorMap.TryGetValue((classType, objectType), out var activator))
			{
				return this.AddObjectAndReturn(activator(element));
			}

			return null;
		}

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

		public void RegisterObjectType<T>(FBXClassType classType, FBXObjectType objectType) where T : FBXObject
		{
			this.m_activatorMap[(classType, objectType)] = (element) => (T)Activator.CreateInstance(typeof(T), new object[] { element, this });
		}
		public void RegisterObjectType<T>(FBXClassType classType, FBXObjectType objectType, Func<IElement, T> activator) where T : FBXObject
		{
			if (activator is null)
			{
				this.RegisterObjectType<T>(classType, objectType);
			}
			else
			{
				this.m_activatorMap[(classType, objectType)] = activator;
			}
		}

		public Clip CreateVideo() => this.AddObjectAndReturn(new Clip(null, this));
		public Texture CreateTexture() => this.AddObjectAndReturn(new Texture(null, this));
		public Material CreateMaterial() => this.AddObjectAndReturn(new Material(null, this));
		public Geometry CreateGeometry() => this.AddObjectAndReturn(new Geometry(null, this));
		public Shape CreateShape() => this.AddObjectAndReturn(new Shape(null, this));

		public BindPose CreateBindPose() => this.AddObjectAndReturn(new BindPose(null, this));
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
		public LimbNodeAttribute CreateLimbNodeAttribute() => this.AddObjectAndReturn(new LimbNodeAttribute(null, this));

		public IEnumerable<T> GetObjectsOfType<T>() where T : FBXObject
		{
			for (int i = 0; i < this.m_objects.Count; ++i)
			{
				if (this.m_objects[i] is T @object)
				{
					yield return @object;
				}
			}
		}

		public IEnumerable<FBXObject> GetObjectsOfType(FBXClassType classType, FBXObjectType objectType)
		{
			for (int i = 0; i < this.m_objects.Count; ++i)
			{
				var @object = this.m_objects[i];

				if (@object.Class == classType && @object.Type == objectType)
				{
					yield return this.m_objects[i];
				}
			}
		}
	}
}
