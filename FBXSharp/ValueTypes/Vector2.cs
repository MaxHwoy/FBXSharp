using System.Runtime.InteropServices;

namespace FBXSharp.ValueTypes
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Vector2
	{
		public const int SizeOf = 0x10;

		public double X;
		public double Y;

		public Vector2(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		public static Vector2 operator -(in Vector2 vector) => new Vector2(-vector.X, -vector.Y);

		public static bool operator ==(in Vector2 lhs, in Vector2 rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y;

		public static bool operator !=(in Vector2 lhs, in Vector2 rhs) => lhs.X != rhs.X || lhs.Y != rhs.Y;

		public override bool Equals(object obj) => obj is Vector2 vector && this == vector;

		public override int GetHashCode() => (this.X, this.Y).GetHashCode();

		public override string ToString() => $"<{this.X}, {this.Y}>";
	}
}
