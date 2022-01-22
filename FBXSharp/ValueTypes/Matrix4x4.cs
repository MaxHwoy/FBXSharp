using System;
using System.Runtime.InteropServices;

namespace FBXSharp.ValueTypes
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Matrix4x4
	{
		public const int SizeOf = 0x80;

		public static readonly Matrix4x4 Identity = new Matrix4x4
		(
			1.0, 0.0, 0.0, 0.0,
			0.0, 1.0, 0.0, 0.0,
			0.0, 0.0, 1.0, 0.0,
			0.0, 0.0, 0.0, 1.0
		);

		public double M11;
		public double M12;
		public double M13;
		public double M14;
		public double M21;
		public double M22;
		public double M23;
		public double M24;
		public double M31;
		public double M32;
		public double M33;
		public double M34;
		public double M41;
		public double M42;
		public double M43;
		public double M44;

		public Matrix4x4(
			double m11, double m12, double m13, double m14,
			double m21, double m22, double m23, double m24,
			double m31, double m32, double m33, double m34,
			double m41, double m42, double m43, double m44)
		{
			this.M11 = m11;
			this.M12 = m12;
			this.M13 = m13;
			this.M14 = m14;
			this.M21 = m21;
			this.M22 = m22;
			this.M23 = m23;
			this.M24 = m24;
			this.M31 = m31;
			this.M32 = m32;
			this.M33 = m33;
			this.M34 = m34;
			this.M41 = m41;
			this.M42 = m42;
			this.M43 = m43;
			this.M44 = m44;
		}

		public static Matrix4x4 CreateFromEuler(in Vector3 euler, RotationOrder order = RotationOrder.XYZ)
		{
			const double kToRad = Math.PI / 180.0;

			var rx = Matrix4x4.CreateRotationX(euler.X * kToRad);
			var ry = Matrix4x4.CreateRotationY(euler.Y * kToRad);
			var rz = Matrix4x4.CreateRotationZ(euler.Z * kToRad);

			switch (order)
			{
				case RotationOrder.XYZ: return rz * ry * rx;
				case RotationOrder.XZY: return ry * rz * rx;
				case RotationOrder.YXZ: return rz * rx * ry;
				case RotationOrder.YZX: return rx * rz * ry;
				case RotationOrder.ZXY: return ry * rx * rz;
				case RotationOrder.ZYX: return rx * ry * rz;
				default: return rx * ry * rz;
			}
		}

		public static Matrix4x4 CreateRotationX(double radians)
		{
			var cos = Math.Cos(radians);
			var sin = Math.Sin(radians);

			return new Matrix4x4
			(
				1.0, 0.0, 0.0, 0.0,
				0.0, +cos, sin, 0.0,
				0.0, -sin, cos, 0.0,
				0.0, 0.0, 0.0, 1.0
			);
		}

		public static Matrix4x4 CreateRotationY(double radians)
		{
			var cos = Math.Cos(radians);
			var sin = Math.Sin(radians);

			return new Matrix4x4
			(
				cos, 0.0, -sin, 0.0,
				0.0, 1.0, 0.0, 0.0,
				sin, 0.0, cos, 0.0,
				0.0, 0.0, 0.0, 1.0
			);
		}

		public static Matrix4x4 CreateRotationZ(double radians)
		{
			var cos = Math.Cos(radians);
			var sin = Math.Sin(radians);

			return new Matrix4x4
			(
				cos, sin, 0.0, 0.0,
				-sin, cos, 0.0, 0.0,
				0.0, 0.0, 1.0, 0.0,
				0.0, 0.0, 0.0, 1.0
			);
		}

		public static Matrix4x4 CreateScale(in Vector3 scale)
		{
			return new Matrix4x4
			(
				scale.X, 0.0, 0.0, 0.0,
				0.0, scale.Y, 0.0, 0.0,
				0.0, 0.0, scale.Z, 0.0,
				0.0, 0.0, 0.0, 1.0
			);
		}

		public static Matrix4x4 CreateTranslation(in Vector3 translation)
		{
			return new Matrix4x4
			(
				1.0, 0.0, 0.0, 0.0,
				0.0, 1.0, 0.0, 0.0,
				0.0, 0.0, 1.0, 0.0,
				translation.X, translation.Y, translation.Z, 1.0
			);
		}

		public static Matrix4x4 operator *(in Matrix4x4 a, in Matrix4x4 b)
		{
			return new Matrix4x4
			(
				a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31 + a.M14 * b.M41,
				a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32 + a.M14 * b.M42,
				a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33 + a.M14 * b.M43,
				a.M11 * b.M14 + a.M12 * b.M24 + a.M13 * b.M34 + a.M14 * b.M44,
				a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31 + a.M24 * b.M41,
				a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32 + a.M24 * b.M42,
				a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33 + a.M24 * b.M43,
				a.M21 * b.M14 + a.M22 * b.M24 + a.M23 * b.M34 + a.M24 * b.M44,
				a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31 + a.M34 * b.M41,
				a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32 + a.M34 * b.M42,
				a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33 + a.M34 * b.M43,
				a.M31 * b.M14 + a.M32 * b.M24 + a.M33 * b.M34 + a.M34 * b.M44,
				a.M41 * b.M11 + a.M42 * b.M21 + a.M43 * b.M31 + a.M44 * b.M41,
				a.M41 * b.M12 + a.M42 * b.M22 + a.M43 * b.M32 + a.M44 * b.M42,
				a.M41 * b.M13 + a.M42 * b.M23 + a.M43 * b.M33 + a.M44 * b.M43,
				a.M41 * b.M14 + a.M42 * b.M24 + a.M43 * b.M34 + a.M44 * b.M44
			);
		}
	}
}
