using UnityEngine;
using UnityEngine.EventSystems;

namespace Modules.Inputs
{
    public sealed class SwipeInput : MonoBehaviour
    {
        private const int LEFT_MOUSE = 0;

        public event SwipeHandler OnSwipe;

        [SerializeField]
        private EventSystem eventSystem;

        [SerializeField]
        private float minSwipePixels = 50;

        private Vector2 startPosition;

        private bool isSwiping;

        private void Start()
        {
            if (this.eventSystem == null)
                this.eventSystem = EventSystem.current;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(LEFT_MOUSE) && !this.eventSystem.IsPointerOverGameObject())
            {
                this.startPosition = Input.mousePosition;
                this.isSwiping = true;
            }
            else if (this.isSwiping && Input.GetMouseButtonUp(LEFT_MOUSE))
            {
                Vector2 endPosition = Input.mousePosition;
                this.isSwiping = false;

                if (this.TryGetDirection(endPosition - this.startPosition, out var direction))
                    this.OnSwipe?.Invoke(this.startPosition, endPosition, direction);
            }
        }

        private bool TryGetDirection(Vector2 moveDistance, out SwipeDirection direction)
        {
            direction = default;

            float distanceX = moveDistance.x;
            float distanceY = moveDistance.y;

            float absDistanceX = Mathf.Abs(distanceX);
            float absDistanceY = Mathf.Abs(distanceY);

            if (absDistanceX > absDistanceY)
            {
                if (absDistanceX < this.minSwipePixels)
                    return false;

                direction = Mathf.Sign(distanceX) > 0 ? SwipeDirection.RIGHT : SwipeDirection.LEFT;
            }
            else
            {
                if (absDistanceY < this.minSwipePixels)
                    return false;

                direction = Mathf.Sign(distanceY) > 0 ? SwipeDirection.UP : SwipeDirection.DOWN;
            }

            return true;
        }
    }
}