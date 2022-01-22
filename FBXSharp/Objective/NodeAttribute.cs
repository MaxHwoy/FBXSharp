using FBXSharp.Core;

namespace FBXSharp.Objective
{
	public class NodeAttribute : FBXObject
	{
		public static readonly FBXObjectType FType = FBXObjectType.NodeAttribute;

		public override FBXObjectType Type => NodeAttribute.FType;

		public string Flags { get; set; }

		internal NodeAttribute(IElement element, IScene scene) : base(element, scene)
		{
			var flags = element.FindChild("TypeFlags");

			if (!(flags is null) && flags.Attributes.Length > 0)
			{
				this.Flags = flags.Attributes[0].GetElementValue().ToString();
			}
		}
	}
}
