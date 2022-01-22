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

		public override string ToString() => $"<{this.X}, {this.Y}>";
	}
}
