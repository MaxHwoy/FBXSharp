using FBXSharp.Core;

namespace FBXSharp.Objective
{
	public class Skin : FBXObject
	{
		public static readonly FBXObjectType FType = FBXObjectType.Skin;

		public static readonly FBXClassType FClass = FBXClassType.Deformer;

		public override FBXObjectType Type => Skin.FType;

		public override FBXClassType Class => Skin.FClass;

		internal Skin(IElement element, IScene scene) : base(element, scene)
		{
		}

		//virtual int getClusterCount() const = 0;
		//virtual const Cluster* getCluster(int idx) const = 0;

		public override IElement AsElement(bool binary)
		{
			return new Element(this.Class.ToString(), null, this.BuildAttributes("Deformer", this.Type.ToString(), binary)); // #TODO
		}
	}
}
