using System;
using Game.Common;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.App
{
    [Serializable]
    public struct LevelSnapshot
    {
        [SerializeField]
        public Item[] items;

        public LevelSnapshot(params Item[] items)
        {
            this.items = items;
        }

        [Serializable]
        public struct Item
        {
            [HorizontalGroup]
            public Vector2Int point;

            [HorizontalGroup]
            public ItemType type;

            public Item(Vector2Int point, ItemType type)
            {
                this.point = point;
                this.type = type;
            }
        }
    }
}