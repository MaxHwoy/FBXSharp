namespace FBXSharp.ValueTypes
{
	public enum RotationOrder : int
	{
		XYZ,
		XZY,
		YZX,
		YXZ,
		ZXY,
		ZYX,
		SPHERIC, // Currently unsupported. Treated as EULER_XYZ.
	}
}
