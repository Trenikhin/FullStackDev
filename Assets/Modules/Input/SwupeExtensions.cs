using System;
using UnityEngine;

namespace Modules.Inputs
{
    public static class SwupeExtensions
    {
        public static Vector2Int ToVector2Int(this SwipeDirection direction)
        {
            return direction switch
            {
                SwipeDirection.UP => new Vector2Int(0, 1),
                SwipeDirection.RIGHT => new Vector2Int(1, 0),
                SwipeDirection.DOWN => new Vector2Int(0, -1),
                SwipeDirection.LEFT => new Vector2Int(-1, 0),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}