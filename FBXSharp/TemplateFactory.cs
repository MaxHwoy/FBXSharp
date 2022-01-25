using FBXSharp.Objective;
using FBXSharp.ValueTypes;
using System;
using IEPF = FBXSharp.Core.IElementPropertyFlags;

namespace FBXSharp
{
	public static class TemplateFactory
	{
		public static TemplateObject GetAnimationStackTemplate()
		{
			var template = new TemplateObject(FBXObjectType.AnimationStack, null, null)
			{
				Name = "FbxAnimStack",
			};
			
			template.AddProperty(new FBXProperty<string>("KString", String.Empty, "Description", IEPF.Imported, String.Empty));
			template.AddProperty(new FBXProperty<TimeBase>("KTime", "Time", "LocalStart", IEPF.Imported, new TimeBase(0.0)));
			template.AddProperty(new FBXProperty<TimeBase>("KTime", "Time", "LocalStop", IEPF.Imported, new TimeBase(0.0)));
			template.AddProperty(new FBXProperty<TimeBase>("KTime", "Time", "ReferenceStart", IEPF.Imported, new TimeBase(0.0)));
			template.AddProperty(new FBXProperty<TimeBase>("KTime", "Time", "ReferenceStop", IEPF.Imported, new TimeBase(0.0)));

			return template;
		}

		public static TemplateObject GetAnimationLayerTemplate()
		{
			var template = new TemplateObject(FBXObjectType.AnimationLayer, null, null)
			{
				Name = "FbxAnimLayer",
			};

			template.AddProperty(new FBXProperty<double>("Number", String.Empty, "Weight", IEPF.Imported | IEPF.Animatable, 100.0));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "Mute", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "Solo", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "Lock", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<Vector3>("ColorRGB", "Color", "Color", IEPF.Imported, new Vector3(0.8, 0.8, 0.8)));
			template.AddProperty(new FBXProperty<Enumeration>("enum", String.Empty, "BlendMode", IEPF.Imported, new Enumeration()));
			template.AddProperty(new FBXProperty<Enumeration>("enum", String.Empty, "RotationAccumulationMode", IEPF.Imported, new Enumeration()));
			template.AddProperty(new FBXProperty<Enumeration>("enum", String.Empty, "ScaleAccumulationMode", IEPF.Imported, new Enumeration()));
			template.AddProperty(new FBXProperty<ulong>("ULongLong", String.Empty, "BlendModeBypass", IEPF.Imported, 0));

			return template;
		}

		public static TemplateObject GetModelTemplate()
		{
			var template = new TemplateObject(FBXObjectType.Model, null, null)
			{
				Name = "FbxNode",
			};

			template.AddProperty(new FBXProperty<Enumeration>("enum", String.Empty, "QuaternionInterpolate", IEPF.Imported, new Enumeration()));
			template.AddProperty(new FBXProperty<Vector3>("Vector3D", "Vector", "RotationOffset", IEPF.Imported, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<Vector3>("Vector3D", "Vector", "RotationPivot", IEPF.Imported, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<Vector3>("Vector3D", "Vector", "ScalingOffset", IEPF.Imported, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<Vector3>("Vector3D", "Vector", "ScalingPivot", IEPF.Imported, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "TranslationActive", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<Vector3>("Vector3D", "Vector", "TranslationMin", IEPF.Imported, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<Vector3>("Vector3D", "Vector", "TranslationMax", IEPF.Imported, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "TranslationMinX", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "TranslationMinY", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "TranslationMinZ", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "TranslationMaxX", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "TranslationMaxY", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "TranslationMaxZ", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<Enumeration>("enum", String.Empty, "RotationOrder", IEPF.Imported, new Enumeration()));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "RotationSpaceForLimitOnly", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<double>("double", "Number", "RotationStiffnessX", IEPF.Imported, 0.0));
			template.AddProperty(new FBXProperty<double>("double", "Number", "RotationStiffnessY", IEPF.Imported, 0.0));
			template.AddProperty(new FBXProperty<double>("double", "Number", "RotationStiffnessZ", IEPF.Imported, 0.0));
			template.AddProperty(new FBXProperty<double>("double", "Number", "AxisLen", IEPF.Imported, 10.0));
			template.AddProperty(new FBXProperty<Vector3>("Vector3D", "Vector", "PreRotation", IEPF.Imported, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<Vector3>("Vector3D", "Vector", "PostRotation", IEPF.Imported, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "RotationActive", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<Vector3>("Vector3D", "Vector", "RotationMin", IEPF.Imported, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<Vector3>("Vector3D", "Vector", "RotationMax", IEPF.Imported, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "RotationMinX", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "RotationMinY", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "RotationMinZ", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "RotationMaxX", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "RotationMaxY", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "RotationMaxZ", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<Enumeration>("enum", String.Empty, "InheritType", IEPF.Imported, new Enumeration()));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "ScalingActive", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<Vector3>("Vector3D", "Vector", "ScalingMin", IEPF.Imported, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<Vector3>("Vector3D", "Vector", "ScalingMax", IEPF.Imported, new Vector3(1.0, 1.0, 1.0)));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "ScalingMinX", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "ScalingMinY", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "ScalingMinZ", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "ScalingMaxX", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "ScalingMaxY", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "ScalingMaxZ", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<Vector3>("Vector3D", "Vector", "GeometricTranslation", IEPF.Imported, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<Vector3>("Vector3D", "Vector", "GeometricRotation", IEPF.Imported, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<Vector3>("Vector3D", "Vector", "GeometricScaling", IEPF.Imported, new Vector3(1.0, 1.0, 1.0)));
			template.AddProperty(new FBXProperty<double>("double", "Number", "MinDampRangeX", IEPF.Imported, 0.0));
			template.AddProperty(new FBXProperty<double>("double", "Number", "MinDampRangeY", IEPF.Imported, 0.0));
			template.AddProperty(new FBXProperty<double>("double", "Number", "MinDampRangeZ", IEPF.Imported, 0.0));
			template.AddProperty(new FBXProperty<double>("double", "Number", "MaxDampRangeX", IEPF.Imported, 0.0));
			template.AddProperty(new FBXProperty<double>("double", "Number", "MaxDampRangeY", IEPF.Imported, 0.0));
			template.AddProperty(new FBXProperty<double>("double", "Number", "MaxDampRangeZ", IEPF.Imported, 0.0));
			template.AddProperty(new FBXProperty<double>("double", "Number", "MinDampStrengthX", IEPF.Imported, 0.0));
			template.AddProperty(new FBXProperty<double>("double", "Number", "MinDampStrengthY", IEPF.Imported, 0.0));
			template.AddProperty(new FBXProperty<double>("double", "Number", "MinDampStrengthZ", IEPF.Imported, 0.0));
			template.AddProperty(new FBXProperty<double>("double", "Number", "MaxDampStrengthX", IEPF.Imported, 0.0));
			template.AddProperty(new FBXProperty<double>("double", "Number", "MaxDampStrengthY", IEPF.Imported, 0.0));
			template.AddProperty(new FBXProperty<double>("double", "Number", "MaxDampStrengthZ", IEPF.Imported, 0.0));
			template.AddProperty(new FBXProperty<double>("double", "Number", "PreferedAngleX", IEPF.Imported, 0.0));
			template.AddProperty(new FBXProperty<double>("double", "Number", "PreferedAngleY", IEPF.Imported, 0.0));
			template.AddProperty(new FBXProperty<double>("double", "Number", "PreferedAngleZ", IEPF.Imported, 0.0));
			template.AddProperty(new FBXProperty<object>("object", String.Empty, "LookAtProperty", IEPF.Imported, null));
			template.AddProperty(new FBXProperty<object>("object", String.Empty, "UpVectorProperty", IEPF.Imported, null));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "Show", IEPF.Imported, true));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "NegativePercentShapeSupport", IEPF.Imported, true));
			template.AddProperty(new FBXProperty<int>("int", "Integer", "DefaultAttributeIndex", IEPF.Imported, -1));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "Freeze", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "LODBox", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<Vector3>("Lcl Translation", String.Empty, "Lcl Translation", IEPF.Imported | IEPF.Animatable, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<Vector3>("Lcl Rotation", String.Empty, "Lcl Rotation", IEPF.Imported | IEPF.Animatable, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<Vector3>("Lcl Scaling", String.Empty, "Lcl Scaling", IEPF.Imported | IEPF.Animatable, new Vector3(1.0, 1.0, 1.0)));
			template.AddProperty(new FBXProperty<double>("Visibility", String.Empty, "Visibility", IEPF.Imported | IEPF.Animatable, 1.0));
			template.AddProperty(new FBXProperty<bool>("Visibility Inheritance", String.Empty, "Visibility Inheritance", IEPF.Imported, true));

			return template;
		}
		
		public static TemplateObject GetNodeAttributeTemplate()
		{
			var template = new TemplateObject(FBXObjectType.NodeAttribute, null, null)
			{
				Name = "FbxNull",
			};

			template.AddProperty(new FBXProperty<Vector3>("ColorRGB", "Color", "Color", IEPF.Imported, new Vector3(0.8, 0.8, 0.8)));
			template.AddProperty(new FBXProperty<double>("double", "Number", "Size", IEPF.Imported, 100.0));
			template.AddProperty(new FBXProperty<Enumeration>("enum", String.Empty, "Look", IEPF.Imported, new Enumeration(1)));

			return template;
		}

		public static TemplateObject GetGeometryTemplate()
		{
			var template = new TemplateObject(FBXObjectType.Geometry, null, null)
			{
				Name = "FbxMesh",
			};

			template.AddProperty(new FBXProperty<Vector3>("ColorRGB", "Color", "Color", IEPF.Imported, new Vector3(0.8, 0.8, 0.8)));
			template.AddProperty(new FBXProperty<Vector3>("Vector3D", "Vector", "BBoxMin", IEPF.Imported, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<Vector3>("Vector3D", "Vector", "BBoxMax", IEPF.Imported, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "Primary Visibility", IEPF.Imported, true));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "Casts Shadows", IEPF.Imported, true));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "Receive Shadows", IEPF.Imported, true));

			return template;
		}

		public static TemplateObject GetMaterialTemplate(string shading = "")
		{
			switch (shading)
			{
				// #TODO

				case "":
				case "Phong":
				default:
					return TemplateFactory.GetSurfacePhongMaterialTemplate();
			}
		}

		public static TemplateObject GetTextureTemplate()
		{
			var template = new TemplateObject(FBXObjectType.Texture, null, null)
			{
				Name = "FbxFileTexture",
			};

			template.AddProperty(new FBXProperty<Enumeration>("enum", String.Empty, "TextureTypeUse", IEPF.Imported, new Enumeration()));
			template.AddProperty(new FBXProperty<Enumeration>("enum", String.Empty, "AlphaSource", IEPF.Imported, new Enumeration(2)));
			template.AddProperty(new FBXProperty<double>("Number", String.Empty, "Texture alpha", IEPF.Imported | IEPF.Animatable, 1.0));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "PremultiplyAlpha", IEPF.Imported, true));
			template.AddProperty(new FBXProperty<Enumeration>("enum", String.Empty, "CurrentMappingType", IEPF.Imported, new Enumeration()));
			template.AddProperty(new FBXProperty<Enumeration>("enum", String.Empty, "WrapModeU", IEPF.Imported, new Enumeration()));
			template.AddProperty(new FBXProperty<Enumeration>("enum", String.Empty, "WrapModeV", IEPF.Imported, new Enumeration()));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "UVSwap", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "PremultiplyAlpha", IEPF.Imported, true));
			template.AddProperty(new FBXProperty<Vector3>("Vector", String.Empty, "Translation", IEPF.Imported | IEPF.Animatable, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<Vector3>("Vector", String.Empty, "Rotation", IEPF.Imported | IEPF.Animatable, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<Vector3>("Vector", String.Empty, "Scaling", IEPF.Imported | IEPF.Animatable, new Vector3(1.0, 1.0, 1.0)));
			template.AddProperty(new FBXProperty<Vector3>("Vector3D", "Vector", "TextureRotationPivot", IEPF.Imported, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<Vector3>("Vector3D", "Vector", "TextureScalingPivot", IEPF.Imported, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<Enumeration>("enum", String.Empty, "CurrentTextureBlendMode", IEPF.Imported, new Enumeration(1)));
			template.AddProperty(new FBXProperty<string>("KString", String.Empty, "UVSet", IEPF.Imported, "default"));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "UseMaterial", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "UseMipMap", IEPF.Imported, false));

			return template;
		}

		public static TemplateObject GetVideoTemplate()
		{
			var template = new TemplateObject(FBXObjectType.Video, null, null)
			{
				Name = "FbxVideo",
			};

			template.AddProperty(new FBXProperty<string>("KString", "XRefUrl", "Path", IEPF.Imported, String.Empty));
			template.AddProperty(new FBXProperty<string>("KString", "XRefUrl", "RelPath", IEPF.Imported, String.Empty));
			template.AddProperty(new FBXProperty<Vector3>("ColorRGB", "Color", "Color", IEPF.Imported, new Vector3(0.8, 0.8, 0.8)));
			template.AddProperty(new FBXProperty<TimeBase>("KTime", "Time", "ClipIn", IEPF.Imported, new TimeBase(0.0)));
			template.AddProperty(new FBXProperty<TimeBase>("KTime", "Time", "ClipOut", IEPF.Imported, new TimeBase(0.0)));
			template.AddProperty(new FBXProperty<TimeBase>("KTime", "Time", "Offset", IEPF.Imported, new TimeBase(0.0)));
			template.AddProperty(new FBXProperty<double>("double", "Number", "PlaySpeed", IEPF.Imported, 0.0));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "FreeRunning", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "Loop", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "Mute", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<Enumeration>("enum", String.Empty, "AccessMode", IEPF.Imported, new Enumeration()));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "ImageSequence", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<int>("int", "Integer", "ImageSequenceOffset", IEPF.Imported, 0));
			template.AddProperty(new FBXProperty<double>("double", "Number", "FrameRate", IEPF.Imported, 0.0));
			template.AddProperty(new FBXProperty<int>("int", "Integer", "LastFrame", IEPF.Imported, 0));
			template.AddProperty(new FBXProperty<int>("int", "Integer", "Width", IEPF.Imported, 0));
			template.AddProperty(new FBXProperty<int>("int", "Integer", "Height", IEPF.Imported, 0));
			template.AddProperty(new FBXProperty<int>("int", "Integer", "StartFrame", IEPF.Imported, 0));
			template.AddProperty(new FBXProperty<int>("int", "Integer", "StopFrame", IEPF.Imported, 0));
			template.AddProperty(new FBXProperty<Enumeration>("enum", String.Empty, "InterlaceMode", IEPF.Imported, new Enumeration()));

			return template;
		}

		public static TemplateObject GetSurfacePhongMaterialTemplate()
		{
			var template = new TemplateObject(FBXObjectType.Material, null, null)
			{
				Name = "FbxSurfacePhong",
			};

			template.AddProperty(new FBXProperty<string>("KString", String.Empty, "ShadingModel", IEPF.Imported, "Phong"));
			template.AddProperty(new FBXProperty<bool>("bool", String.Empty, "MultiLayer", IEPF.Imported, false));
			template.AddProperty(new FBXProperty<Vector3>("Color", String.Empty, "EmissiveColor", IEPF.Imported | IEPF.Animatable, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<double>("Number", String.Empty, "EmissiveFactor", IEPF.Imported | IEPF.Animatable, 1.0));
			template.AddProperty(new FBXProperty<Vector3>("Color", String.Empty, "AmbientColor", IEPF.Imported | IEPF.Animatable, new Vector3(0.2, 0.2, 0.2)));
			template.AddProperty(new FBXProperty<double>("Number", String.Empty, "AmbientFactor", IEPF.Imported | IEPF.Animatable, 1.0));
			template.AddProperty(new FBXProperty<Vector3>("Color", String.Empty, "DiffuseColor", IEPF.Imported | IEPF.Animatable, new Vector3(0.8, 0.8, 0.8)));
			template.AddProperty(new FBXProperty<double>("Number", String.Empty, "DiffuseFactor", IEPF.Imported | IEPF.Animatable, 1.0));
			template.AddProperty(new FBXProperty<Vector3>("Vector3D", "Vector", "Bump", IEPF.Imported, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<Vector3>("Vector3D", "Vector", "NormalMap", IEPF.Imported, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<double>("double", "Number", "BumpFactor", IEPF.Imported, 1.0));
			template.AddProperty(new FBXProperty<Vector3>("Color", String.Empty, "TransparentColor", IEPF.Imported | IEPF.Animatable, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<double>("Number", String.Empty, "TransparencyFactor", IEPF.Imported | IEPF.Animatable, 0.0));
			template.AddProperty(new FBXProperty<Vector3>("ColorRGB", "Color", "DisplacementColor", IEPF.Imported, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<double>("double", "Number", "DisplacementFactor", IEPF.Imported, 1.0));
			template.AddProperty(new FBXProperty<Vector3>("ColorRGB", "Color", "VectorDisplacementColor", IEPF.Imported, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<double>("double", "Number", "VectorDisplacementFactor", IEPF.Imported, 1.0));
			template.AddProperty(new FBXProperty<Vector3>("Color", String.Empty, "SpecularColor", IEPF.Imported | IEPF.Animatable, new Vector3(0.2, 0.2, 0.2)));
			template.AddProperty(new FBXProperty<double>("Number", String.Empty, "SpecularFactor", IEPF.Imported | IEPF.Animatable, 1.0));
			template.AddProperty(new FBXProperty<double>("Number", String.Empty, "ShininessExponent", IEPF.Imported | IEPF.Animatable, 20.0));
			template.AddProperty(new FBXProperty<Vector3>("Color", String.Empty, "ReflectionColor", IEPF.Imported | IEPF.Animatable, new Vector3(0.0, 0.0, 0.0)));
			template.AddProperty(new FBXProperty<double>("Number", String.Empty, "ReflectionFactor", IEPF.Imported | IEPF.Animatable, 1.0));

			return template;
		}

		public static TemplateObject GetTemplateForType(FBXObjectType type)
		{
			switch (type)
			{
				case FBXObjectType.AnimationLayer: return TemplateFactory.GetAnimationLayerTemplate();
				case FBXObjectType.AnimationStack: return TemplateFactory.GetAnimationStackTemplate();
				case FBXObjectType.Model: return TemplateFactory.GetModelTemplate();
				case FBXObjectType.Geometry: return TemplateFactory.GetGeometryTemplate();
				case FBXObjectType.Material: return TemplateFactory.GetMaterialTemplate();
				case FBXObjectType.Texture: return TemplateFactory.GetTextureTemplate();
				case FBXObjectType.Video: return TemplateFactory.GetVideoTemplate();
				default: return null;
			}
		}

		public static TemplateObject GetTemplateForObject(FBXObject @object)
		{
			if (@object is null)
			{
				return null;
			}

			switch (@object.Type)
			{
				case FBXObjectType.AnimationLayer: return TemplateFactory.GetAnimationLayerTemplate();
				case FBXObjectType.AnimationStack: return TemplateFactory.GetAnimationStackTemplate();
				case FBXObjectType.Model: return TemplateFactory.GetModelTemplate();
				case FBXObjectType.Geometry: return TemplateFactory.GetGeometryTemplate();
				case FBXObjectType.Material: return TemplateFactory.GetMaterialTemplate((@object as Material).ShadingModel);
				case FBXObjectType.Texture: return TemplateFactory.GetTextureTemplate();
				case FBXObjectType.Video: return TemplateFactory.GetVideoTemplate();
				default: return null;
			}
		}
	}
}
