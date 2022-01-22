using System;

namespace FBXSharp.ValueTypes
{
	public class Enumeration
	{
		private string[] m_flags;

		public int Value { get; set; }

		public string[] Flags
		{
			get => this.m_flags;
			set => this.m_flags = value ?? Array.Empty<string>();
		}

		public Enumeration()
		{
			this.Value = 0;
			this.m_flags = Array.Empty<string>();
		}
		public Enumeration(int value)
		{
			this.Value = value;
			this.m_flags = Array.Empty<string>();
		}
		public Enumeration(string[] flags)
		{
			this.Value = 0;
			this.m_flags = flags ?? Array.Empty<string>();
		}
		public Enumeration(int value, string[] flags)
		{
			this.Value = value;
			this.m_flags = flags ?? Array.Empty<string>();
		}
	}
}
