namespace Game.UI
{
	using System.Collections;
	using DG.Tweening;
	using UnityEngine;
	using Zenject;

	public interface IFlyIcons
	{
		void Fly(Vector3 from, int delta);
	}
	
	public class FlyIcons : MonoBehaviour, IFlyIcons
	{
		[SerializeField] Transform _coinTarget;
		[SerializeField] GameObject _coinTemplate;
		[SerializeField] float _duration = 0.5f;
		
		[Inject] IShowCoins _showCoins;
		
		public void Fly( Vector3 from, int delta ) => StartCoroutine(FlyRoutine(from, delta));

		IEnumerator FlyRoutine( Vector3 from, int delta )
		{
			// Fly
			var coin = Instantiate( _coinTemplate, _coinTarget );
			coin.transform.position = from;
			
			yield return coin.transform
				.DOMove(_coinTarget.position, _duration)
				.SetEase(Ease.OutQuad)
				.WaitForCompletion();
			
			Destroy(coin.gameObject);
			
			// Change Coins
			var cur = _showCoins.Current.Value;
			var tgt = cur + delta;
			
			DOVirtual
				.Float(cur, tgt, 0.4f, v => _showCoins.Set((int)v ))
				.SetEase(Ease.Linear);
		}
	}
}