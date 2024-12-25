namespace Game.UI
{
	using System;
	using DG.Tweening;
	using Modules.UI;
	using TMPro;
	using UnityEngine;
	using Zenject;

	public interface ICoinsView
	{
		string Text { set; }
		
		void Fly(Vector3 pos, int coins, Action<int> onCoinAdded);
	}
	
	public class CoinsView : MonoBehaviour, ICoinsView
	{
		[SerializeField] TextMeshProUGUI _coinsText;
		[SerializeField] RectTransform _coinsParent;
		
		[Inject] ParticleAnimator _particleAnimator;
		
		public string Text { set => _coinsText.text = value; }
		
		public void Fly( Vector3 pos, int coins, Action<int> onCoinAdded )
		{
			onCoinAdded(coins);
			
			_particleAnimator.Emit( pos, _coinsParent.position, 1, ()
				=>
				DOVirtual.Float( coins, 0, 0.4f, c => onCoinAdded((int)c)));
		}
	}
}