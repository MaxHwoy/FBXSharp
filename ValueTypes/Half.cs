using System.Runtime.InteropServices;

namespace FBXSharp.ValueTypes
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Half
	{
		[StructLayout(LayoutKind.Explicit, Pack = 1)]
		private ref struct Union
		{
			[FieldOffset(0)]
			public uint UInt;

			[FieldOffset(0)]
			public float Float;

			public Union(float value)
			{
				this.UInt = 0;
				this.Float = value;
			}
		}

		public const int SizeOf = 0x02;

		public ushort Value;

		public Half(short value) => this.Value = (ushort)value;
		public Half(ushort value) => this.Value = value;
		public Half(float value)
		{
			var union = new Union(value);

			var sign = union.UInt >> 31;
			var expt = union.UInt & 0x7F800000;
			var mant = union.UInt & 0x007FFFFF;

			if (expt < 0x47800000u)
			{
				if (expt > 0x38000000u)
				{
					this.Value = (ushort)((mant >> 13) | ((expt - 0x38000000u) >> 13) | (sign << 15));
				}
				else
				{
					this.Value = (ushort)((mant >> (int)(((0x38000000u - expt) >> 23) + 14u)) | (sign << 15));
				}
			}
			else
			{
				uint max = (mant != 0u && expt == 0x7F800000u) ? 0x007FFFFFu : 0u;

				this.Value = (ushort)((max >> 13) | (sign << 15) | 0x7C00u);
			}
		}
	}
}
