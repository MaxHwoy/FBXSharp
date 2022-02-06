using System.Collections.ObjectModel;

namespace FBXSharp.Core
{
	public interface IScene
	{
		FBXObject Root { get; }
		GlobalSettings Settings { get; }
		ReadOnlyCollection<FBXObject> Objects { get; }
		ReadOnlyCollection<TakeInfo> TakeInfos { get; }
		ReadOnlyCollection<TemplateObject> Templates { get; }

		TemplateObject GetTemplateObject(string name);
		TemplateObject GetTemplateObject(FBXObjectType objectType);

		FBXObject CreateFBXObject(FBXObjectType type);
		void DestroyFBXObject(FBXObject @object);
	}
}
