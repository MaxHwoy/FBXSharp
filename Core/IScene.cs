using System;
using System.Collections.ObjectModel;

namespace FBXSharp.Core
{
	public interface IScene
	{
		FBXObject Root { get; }
		ReadOnlyCollection<FBXObject> Objects { get; }
	}
}
