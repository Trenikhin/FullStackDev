namespace Ui.PlayerHud
{
    using TMPro;
    using UnityEngine;

    public class UiPlayerHudView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _health;
        
        public void SetHealth( string text ) => _health.text = text;
    }
}