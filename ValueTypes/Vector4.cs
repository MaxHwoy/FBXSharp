using System.Runtime.InteropServices;

namespace FBXSharp.ValueTypes
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Vector4
	{
		public const int SizeOf = 0x20;

		public double X;
		public double Y;
		public double Z;
		public double W;

		public Vector4(double x, double y, double z, double w = 1.0)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.W = w;
		}

		public static Vector4 operator -(in Vector4 vector) => new Vector4(-vector.X, -vector.Y, -vector.Z, -vector.W);

		public override string ToString() => $"<{this.X}, {this.Y}, {this.Z}, {this.W}>";
	}
}
