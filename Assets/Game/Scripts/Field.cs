namespace DefaultNamespace
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Game.App;
	using Game.Common;
	using Sirenix.OdinInspector;
	using Sirenix.Utilities;
	using UnityEngine;
	
	public class Field : SerializedMonoBehaviour
	{
		[SerializeField] LevelConfig _levelConfig;
		[SerializeField] ItemView _item;
		
		[SerializeField] Dictionary<ItemType, Sprite> _itemIcons;
		[SerializeField] float _spaces = 10f;
		
		SpriteRenderer _cell => _item.GetComponentInChildren<SpriteRenderer>();
		
		public void Start()
		{
			_levelConfig.Field.items.ForEach(i =>
			{
				var item = Instantiate(_item, transform);
				
				item.Init( _itemIcons[i.type] );
				var pos = GridToWorld( i.point, 5, 6 );
				item.transform.position = pos;
			});
		}
		
		float CellWidth => _cell.bounds.size.x + _spaces;
		float CellHeight => _cell.bounds.size.x + _spaces;
		
		Vector3 GridToWorld(Vector2Int pos, int width, int height)
		{
			width -= 1;
			height -= 1;
			
			float xOffset = (CellWidth * width) / 2;
			float xPos = CellWidth * pos.x;
			float yOffset = (CellHeight * height) / 2;
			float yPos = CellHeight * pos.y;
			
			return new Vector3( xPos - xOffset, yPos - yOffset, 0f );
		}
	}
}