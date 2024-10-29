using UnityEngine;

namespace ShootEmUp
{
    public sealed class LevelBounds : MonoBehaviour
    {
        [SerializeField] Transform leftBorder;
        [SerializeField] Transform rightBorder;
        [SerializeField] Transform downBorder;
        [SerializeField] Transform topBorder;
        
        public bool InBounds(Vector3 position)
        {
            var positionX = position.x;
            var positionY = position.y;
            return positionX > this.leftBorder.position.x
                   && positionX < this.rightBorder.position.x
                   && positionY > this.downBorder.position.y
                   && positionY < this.topBorder.position.y;
        }
    }
}