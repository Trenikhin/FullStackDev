using UnityEngine;

namespace Game.UI
{
    public sealed class BackgroundView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _renderer;

        public void SetSprite(Sprite sprite)
        {
            _renderer.sprite = sprite;
        }
    }
}