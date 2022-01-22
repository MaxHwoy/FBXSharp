using System.Runtime.InteropServices;

namespace FBXSharp.Core
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Header
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x15)]
		public byte[] Magic;

		public byte Reserved1;
		public byte Reserved2;
		public uint Version;

		public const int SizeOf = 0x1B; // amazing lmao
	}
}
