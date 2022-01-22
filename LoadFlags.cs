using System;

namespace FBXSharp
{
	[Flags]
	public enum LoadFlags : int
	{
		None = 0,
		Triangulate = 1 << 0,
		IgnoreGeometry = 1 << 1,
		IgnoreBlendShapes = 1 << 2,
	}
}
