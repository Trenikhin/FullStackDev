namespace Game.UI
{
	using System.Collections;
	using DG.Tweening;
	using UnityEngine;
	using Zenject;

	public interface IFlyIcons
	{
		void Fly(Transform coin, int delta);
	}
	
	public class FlyIcons : MonoBehaviour, IFlyIcons
	{
		[SerializeField] Transform _coinTarget;
		[SerializeField] float _duration = 0.5f;
		
		[Inject] IShowCoins _showCoins;
		
		public void Fly( Transform coin, int delta ) => StartCoroutine(FlyRoutine(coin, delta));

		IEnumerator FlyRoutine( Transform coin, int delta )
		{
			// Fly
			var startPos = coin.position;
			
			yield return coin
				.DOMove(_coinTarget.position, _duration)
				.SetEase(Ease.OutQuad)
				.SetLink(coin.gameObject)
				.WaitForCompletion();
			
			coin.gameObject.SetActive(false);
			coin.transform.position = startPos;
			
			// Change Coins
			var cur = _showCoins.Current.Value;
			var tgt = cur + delta;
			
			DOVirtual
				.Float(cur, tgt, 0.4f, v => _showCoins.Set((int)v ))
				.SetLink( coin.gameObject )
				.SetEase(Ease.Linear);
		}
	}
}