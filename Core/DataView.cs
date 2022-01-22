using System;
using System.IO;

namespace FBXSharp.Core
{
	public struct DataView
	{
		public long Start { get; }
		public long End { get; }
		public bool IsBinary { get; }
		public BinaryReader Reader { get; }

		public DataView(long start, long end, bool isBinary, BinaryReader reader)
		{
			this.Start = start;
			this.End = end;
			this.IsBinary = isBinary;
			this.Reader = reader;
		}

		public override bool Equals(object obj)
		{
			return obj is DataView dataView && dataView == this;
		}
		public override int GetHashCode()
		{
			return Tuple.Create(this.Start, this.End, this.IsBinary, this.Reader).GetHashCode();
		}
		public static bool operator ==(DataView dataView1, DataView dataView2)
		{
			return dataView1.Start == dataView2.Start && dataView1.End == dataView2.End &&
				dataView1.IsBinary == dataView2.IsBinary && dataView1.Reader == dataView2.Reader;
		}
		public static bool operator !=(DataView dataView1, DataView dataView2)
		{
			return !(dataView1 == dataView2);
		}

		public int ToInt32(int offset = 0)
		{
			if (this.Reader is null)
			{
				return 0;
			}
			
			var current = this.Reader.BaseStream.Position;
			this.Reader.BaseStream.Position = this.Start + offset;

			if (this.IsBinary)
			{
				int result = this.Reader.ReadInt32();
				this.Reader.BaseStream.Position = current;
				return result;
			}
			else
			{
				int result = this.Reader.ReadTextInt32();
				this.Reader.BaseStream.Position = current;
				return result;
			}
		}
		public uint ToUInt32(int offset = 0)
		{
			if (this.Reader is null)
			{
				return 0;
			}

			var current = this.Reader.BaseStream.Position;
			this.Reader.BaseStream.Position = this.Start + offset;

			if (this.IsBinary)
			{
				uint result = this.Reader.ReadUInt32();
				this.Reader.BaseStream.Position = current;
				return result;
			}
			else
			{
				uint result = this.Reader.ReadTextUInt32();
				this.Reader.BaseStream.Position = current;
				return result;
			}
		}
		public long ToInt64(int offset = 0)
		{
			if (this.Reader is null)
			{
				return 0;
			}

			var current = this.Reader.BaseStream.Position;
			this.Reader.BaseStream.Position = this.Start + offset;

			if (this.IsBinary)
			{
				long result = this.Reader.ReadInt64();
				this.Reader.BaseStream.Position = current;
				return result;
			}
			else
			{
				long result = this.Reader.ReadTextInt64();
				this.Reader.BaseStream.Position = current;
				return result;
			}
		}
		public ulong ToUInt64(int offset = 0)
		{
			if (this.Reader is null)
			{
				return 0;
			}

			var current = this.Reader.BaseStream.Position;
			this.Reader.BaseStream.Position = this.Start + offset;

			if (this.IsBinary)
			{
				ulong result = this.Reader.ReadUInt64();
				this.Reader.BaseStream.Position = current;
				return result;
			}
			else
			{
				ulong result = this.Reader.ReadTextUInt64();
				this.Reader.BaseStream.Position = current;
				return result;
			}
		}
		public float ToSingle(int offset = 0)
		{
			if (this.Reader is null)
			{
				return 0;
			}

			var current = this.Reader.BaseStream.Position;
			this.Reader.BaseStream.Position = this.Start + offset;

			if (this.IsBinary)
			{
				float result = this.Reader.ReadSingle();
				this.Reader.BaseStream.Position = current;
				return result;
			}
			else
			{
				float result = this.Reader.ReadTextSingle();
				this.Reader.BaseStream.Position = current;
				return result;
			}
		}
		public double ToDouble(int offset = 0)
		{
			if (this.Reader is null)
			{
				return 0;
			}

			var current = this.Reader.BaseStream.Position;
			this.Reader.BaseStream.Position = this.Start + offset;

			if (this.IsBinary)
			{
				double result = this.Reader.ReadDouble();
				this.Reader.BaseStream.Position = current;
				return result;
			}
			else
			{
				double result = this.Reader.ReadTextDouble();
				this.Reader.BaseStream.Position = current;
				return result;
			}
		}
		public override string ToString()
		{
			if (this.Reader is null)
			{
				return String.Empty;
			}

			var current = this.Reader.BaseStream.Position;
			this.Reader.BaseStream.Position = this.Start;
			int size = (int)(this.End - this.Start);

			string result = this.Reader.ReadNullTerminated(size);
			this.Reader.BaseStream.Position = current;
			return result;
		}
	}
}
