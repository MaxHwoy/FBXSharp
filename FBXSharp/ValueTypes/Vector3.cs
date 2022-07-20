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

		public static Vector3 operator +(in Vector3 a, in Vector3 b) => new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

		public static Vector3 operator -(in Vector3 a, in Vector3 b) => new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

		public static bool operator ==(in Vector3 lhs, in Vector3 rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z;

		public static bool operator !=(in Vector3 lhs, in Vector3 rhs) => lhs.X != rhs.X || lhs.Y != rhs.Y || lhs.Z != rhs.Z;

		public override bool Equals(object obj) => obj is Vector3 vector && this == vector;

		public override int GetHashCode() => (this.X, this.Y, this.Z).GetHashCode();

		public override string ToString() => $"<{this.X}, {this.Y}, {this.Z}>";
	}
}
