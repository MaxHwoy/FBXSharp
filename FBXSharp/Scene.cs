using FBXSharp.Core;
using FBXSharp.Objective;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FBXSharp
{
	public class Scene : IScene
	{
		private readonly Root m_root;
		private readonly GlobalSettings m_settings;
		private readonly List<FBXObject> m_objects;
		private readonly List<TakeInfo> m_takeInfos;
		private readonly ReadOnlyCollection<FBXObject> m_readonlyObjects;
		private readonly ReadOnlyCollection<TakeInfo> m_readonlyTakeInfos;

		public FBXObject Root => this.m_root;

		public GlobalSettings Settings => this.m_settings;

		public ReadOnlyCollection<FBXObject> Objects => this.m_readonlyObjects;

		public ReadOnlyCollection<TakeInfo> TakeInfos => this.m_readonlyTakeInfos;

		public Scene()
		{
			this.m_root = new Root(this);
			this.m_objects = new List<FBXObject>();
			this.m_takeInfos = new List<TakeInfo>();
			this.m_settings = new GlobalSettings(null, this);
			this.m_readonlyObjects = new ReadOnlyCollection<FBXObject>(this.m_objects);
			this.m_readonlyTakeInfos = new ReadOnlyCollection<TakeInfo>(this.m_takeInfos);
		}

		internal void InternalAddObject(FBXObject @object) => this.m_objects.Add(@object);

		internal void InternalSetTakeInfos(TakeInfo[] takeInfos) => this.m_takeInfos.AddRange(takeInfos ?? Array.Empty<TakeInfo>());
	}
}
