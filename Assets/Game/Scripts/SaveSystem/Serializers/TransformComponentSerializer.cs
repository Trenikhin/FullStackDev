namespace Game.SaveSystem
{
	using UnityEngine;

	public class TransformComponentSerializer : BaseComponentSerializer< Transform, (Vector3 pos, Quaternion rotation) >
	{
		protected override (Vector3 pos, Quaternion rotation) Get(Transform component)
		{
			return (component.position, component.rotation);
		}
		
		protected override void Set(Transform component, (Vector3 pos, Quaternion rotation) data)
		{
			component.position = data.pos;
			component.rotation = data.rotation;
		}
	}
}