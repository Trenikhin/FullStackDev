using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Common
{
    [CreateAssetMenu(
        fileName = "ItemSpriteMap",
        menuName = "Game/UI/New ItemSpriteMap"
    )]
    public sealed class ItemSpriteMap : ScriptableObject
    {
        [SerializeField]
        private Item[] _items = Array.Empty<Item>();

        public Sprite GetBaseSprite(ItemType type)
        {
            for (int i = 0, count = _items.Length; i < count; i++)
            {
                Item item = _items[i];
                if (item.type == type)
                    return item.baseSprite;
            }
        
            throw new KeyNotFoundException($"Sprite of type {type} is not found!");
        }

        public Sprite GetItemSprite(ItemType type)
        {
            for (int i = 0, count = _items.Length; i < count; i++)
            {
                Item item = _items[i];
                if (item.type == type)
                    return item.itemSprite;
            }

            throw new KeyNotFoundException($"Sprite of type {type} is not found!");
        }

        public Sprite GetQuestSprite(ItemType type)
        {
            for (int i = 0, count = _items.Length; i < count; i++)
            {
                Item item = _items[i];
                if (item.type == type)
                    return item.questSprite;
            }
        
            throw new KeyNotFoundException($"Sprite of type {type} is not found!");
        }
        
        [Serializable]
        private struct Item
        {
            public ItemType type;

            public Sprite baseSprite;
            
            [FormerlySerializedAs("sprite")]
            public Sprite itemSprite;
            
            public Sprite questSprite;
        }
    }
}