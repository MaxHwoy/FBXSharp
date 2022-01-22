namespace FBXSharp.ValueTypes
{
	public struct TimeBase
	{
		public const int SizeOf = 0x08;

		public double Time;

		public TimeBase(double time)
		{
			this.Time = time;
		}
		public TimeBase(long time)
		{
			this.Time = (double)time / 46186158000L;
		}
	}
}
