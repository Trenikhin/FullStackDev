namespace DefaultNamespace
{
	using UnityEngine;

	public class ItemView : MonoBehaviour
	{
		[SerializeField] SpriteRenderer _icon;
		
		public void Init( Sprite sprite )
		{
			_icon.sprite = sprite;
		}
	}
}