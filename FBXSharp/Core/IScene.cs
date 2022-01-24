using System;
using System.Collections.ObjectModel;

namespace FBXSharp.Core
{
	public interface IScene
	{
		FBXObject Root { get; }
		GlobalSettings Settings { get; }
		ReadOnlyCollection<FBXObject> Objects { get; }
		ReadOnlyCollection<TakeInfo> TakeInfos { get; }
	}
}
