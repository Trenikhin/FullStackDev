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
		Vector3 Position { get; }
		
		void Add(int coins, Action<int> onCoinAdded);
	}
	
	public class CoinsView : MonoBehaviour, ICoinsView
	{
		[SerializeField] TextMeshProUGUI _coinsText;
		[SerializeField] RectTransform _coinsParent;
		
		[Inject] ParticleAnimator _particleAnimator;
		
		public string Text { set => _coinsText.text = value; }
		public Vector3 Position => _coinsParent.position;

		public void Add( int coins, Action<int> onCoinAdded )
		{
			DOVirtual.Float(coins, 0, 0.4f, c => onCoinAdded((int)c));
		}
	}
}