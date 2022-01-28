using FBXSharp.Core;
using FBXSharp.ValueTypes;
using System;

namespace FBXSharp
{
	public class GlobalSettings : FBXObject
	{
		public static readonly FBXObjectType FType = FBXObjectType.GlobalSettings;

		public override FBXObjectType Type => GlobalSettings.FType;

		public int? UpAxis
		{
			get => this.InternalGetPrimitive<int>(nameof(this.UpAxis), IElementPropertyType.Int);
			set => this.InternalSetPrimitive<int>(nameof(this.UpAxis), IElementPropertyType.Int, value, "int", "Integer");
		}
		public int? UpAxisSign
		{
			get => this.InternalGetPrimitive<int>(nameof(this.UpAxisSign), IElementPropertyType.Int);
			set => this.InternalSetPrimitive<int>(nameof(this.UpAxisSign), IElementPropertyType.Int, value, "int", "Integer");
		}
		public int? FrontAxis
		{
			get => this.InternalGetPrimitive<int>(nameof(this.FrontAxis), IElementPropertyType.Int);
			set => this.InternalSetPrimitive<int>(nameof(this.FrontAxis), IElementPropertyType.Int, value, "int", "Integer");
		}
		public int? FrontAxisSign
		{
			get => this.InternalGetPrimitive<int>(nameof(this.FrontAxisSign), IElementPropertyType.Int);
			set => this.InternalSetPrimitive<int>(nameof(this.FrontAxisSign), IElementPropertyType.Int, value, "int", "Integer");
		}
		public int? CoordAxis
		{
			get => this.InternalGetPrimitive<int>(nameof(this.CoordAxis), IElementPropertyType.Int);
			set => this.InternalSetPrimitive<int>(nameof(this.CoordAxis), IElementPropertyType.Int, value, "int", "Integer");
		}
		public int? CoordAxisSign
		{
			get => this.InternalGetPrimitive<int>(nameof(this.CoordAxisSign), IElementPropertyType.Int);
			set => this.InternalSetPrimitive<int>(nameof(this.CoordAxisSign), IElementPropertyType.Int, value, "int", "Integer");
		}
		public int? OriginalUpAxis
		{
			get => this.InternalGetPrimitive<int>(nameof(this.OriginalUpAxis), IElementPropertyType.Int);
			set => this.InternalSetPrimitive<int>(nameof(this.OriginalUpAxis), IElementPropertyType.Int, value, "int", "Integer");
		}
		public int? OriginalUpAxisSign
		{
			get => this.InternalGetPrimitive<int>(nameof(this.OriginalUpAxisSign), IElementPropertyType.Int);
			set => this.InternalSetPrimitive<int>(nameof(this.OriginalUpAxisSign), IElementPropertyType.Int, value, "int", "Integer");
		}
		public double? UnitScaleFactor
		{
			get => this.InternalGetPrimitive<double>(nameof(this.UnitScaleFactor), IElementPropertyType.Double);
			set => this.InternalSetPrimitive<double>(nameof(this.UnitScaleFactor), IElementPropertyType.Double, value, "double", "Number");
		}
		public double? OriginalUnitScaleFactor
		{
			get => this.InternalGetPrimitive<double>(nameof(this.OriginalUnitScaleFactor), IElementPropertyType.Double);
			set => this.InternalSetPrimitive<double>(nameof(this.OriginalUnitScaleFactor), IElementPropertyType.Double, value, "double", "Number");
		}
		public TimeBase? TimeSpanStart
		{
			get => this.InternalGetPrimitive<TimeBase>(nameof(this.TimeSpanStart), IElementPropertyType.Time);
			set => this.InternalSetPrimitive<TimeBase>(nameof(this.TimeSpanStart), IElementPropertyType.Time, value, "KTime", "Time");
		}
		public TimeBase? TimeSpanStop
		{
			get => this.InternalGetPrimitive<TimeBase>(nameof(this.TimeSpanStop), IElementPropertyType.Time);
			set => this.InternalSetPrimitive<TimeBase>(nameof(this.TimeSpanStop), IElementPropertyType.Time, value, "KTime", "Time");
		}
		public double? CustomFrameRate
		{
			get => this.InternalGetPrimitive<double>(nameof(this.CustomFrameRate), IElementPropertyType.Double);
			set => this.InternalSetPrimitive<double>(nameof(this.CustomFrameRate), IElementPropertyType.Double, value, "double", "Number");
		}
		public Enumeration TimeMode
		{
			get => this.InternalGetEnumeration(nameof(this.TimeMode));
			set => this.InternalSetEnumeration(nameof(this.TimeMode), value, "enum", String.Empty);
		}

		internal GlobalSettings(IElement element, IScene scene) : base(element, scene)
		{
			this.Name = nameof(GlobalSettings);
		}

		internal void InternalFillWithElement(IElement element) => this.FromElement(element);

		public override IElement AsElement() => throw new NotSupportedException("Global Settings cannot be serialized");
	}
}
