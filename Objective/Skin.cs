using FBXSharp.Core;

namespace FBXSharp.Objective
{
	public class Skin : FBXObject
	{
		public static readonly FBXObjectType FType = FBXObjectType.Skin;

		public override FBXObjectType Type => Skin.FType;

		internal Skin(IElement element, IScene scene) : base(element, scene)
		{
		}

		//virtual int getClusterCount() const = 0;
		//virtual const Cluster* getCluster(int idx) const = 0;
	}
}
