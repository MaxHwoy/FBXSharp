using System;

namespace FBXSharp
{
	public class TakeInfo
	{
		public string Name { get; }
		public string Filename { get; }
		public double LocalTimeFrom { get; }
		public double LocalTimeTo { get; }
		public double ReferenceTimeFrom { get; }
		public double ReferenceTimeTo { get; }

		public TakeInfo(string name, string filename, double localFrom, double localTo, double refFrom, double refTo)
		{
			this.Name = name ?? String.Empty;
			this.Filename = filename ?? String.Empty;
			this.LocalTimeFrom = localFrom;
			this.LocalTimeTo = localTo;
			this.ReferenceTimeFrom = refFrom;
			this.ReferenceTimeTo = refTo;
		}

		public override string ToString() => this.Name;
	}
}
