using FBXSharp.Core;
using System;

namespace FBXSharp.Objective
{
	public class AnimationCurve : FBXObject
	{
		private long[] m_keyTimes;
		private float[] m_keyValues;

		public static readonly FBXObjectType FType = FBXObjectType.AnimationCurve;

		public static readonly FBXClassType FClass = FBXClassType.AnimationCurve;

		public override FBXObjectType Type => AnimationCurve.FType;

		public override FBXClassType Class => AnimationCurve.FClass;

		public long[] KeyTimes
		{
			get => this.m_keyTimes;
			set => this.m_keyTimes = value ?? Array.Empty<long>();
		}

		public float[] KeyValues
		{
			get => this.m_keyValues;
			set => this.m_keyValues = value ?? Array.Empty<float>();
		}

		internal AnimationCurve(IElement element, IScene scene) : base(element, scene)
		{
			this.m_keyTimes = Array.Empty<long>();
			this.m_keyValues = Array.Empty<float>();

			if (element is null)
			{
				return;
			}

			var times = element.FindChild("KeyTime");
			var value = element.FindChild("KeyValueFloat");

			if (!(times is null) && times.Attributes.Length > 0 && times.Attributes[0].GetElementValue() is Array timesArray)
			{
				this.m_keyTimes = new long[timesArray.Length];
				Array.Copy(timesArray, this.m_keyTimes, this.m_keyTimes.Length);
			}

			if (!(value is null) && value.Attributes.Length > 0 && value.Attributes[0].GetElementValue() is Array valueArray)
			{
				this.m_keyValues = new float[valueArray.Length];
				Array.Copy(valueArray, this.m_keyValues, this.m_keyValues.Length);
			}

			if (this.m_keyValues.Length != this.m_keyTimes.Length)
			{
				throw new Exception($"Invalid animation curve with name {this.Name}");
			}
		}

		public override IElement AsElement(bool binary)
		{
			return new Element(this.Class.ToString(), null, this.BuildAttributes("AnimCurve", String.Empty, binary)); // #TODO
		}
	}
}
