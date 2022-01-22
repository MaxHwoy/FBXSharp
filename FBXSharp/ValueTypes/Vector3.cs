using System.Runtime.InteropServices;

namespace FBXSharp.ValueTypes
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Vector3
	{
		public const int SizeOf = 0x18;

		public static readonly Vector3 Zero = new Vector3(0.0, 0.0, 0.0);
		public static readonly Vector3 One = new Vector3(1.0, 1.0, 1.0);

		public double X;
		public double Y;
		public double Z;

		public Vector3(double x, double y, double z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		public static Vector3 operator -(in Vector3 vector) => new Vector3(-vector.X, -vector.Y, -vector.Z);

		public override string ToString() => $"<{this.X}, {this.Y}, {this.Z}>";
	}
}
