using System;

namespace FBXSharp.ValueTypes
{
	public class BinaryBlob
	{
		private object[] m_datas;

		public object[] Datas
		{
			get => this.m_datas;
			set => this.m_datas = value ?? Array.Empty<object>();
		}

		public BinaryBlob()
		{
			this.m_datas = Array.Empty<object>();
		}
		public BinaryBlob(object[] datas)
		{
			this.m_datas = datas ?? Array.Empty<object>();
		}
	}
}
