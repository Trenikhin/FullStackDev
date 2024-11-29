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
	
	public class FlyCoins : MonoBehaviour, IFlyIcons
	{
		[SerializeField] Transform _coinTarget;
		[SerializeField] GameObject _coinTemplate;
		[SerializeField] float _duration = 0.5f;
		
		[Inject] ICoins _coins;
		
		public void Fly( Vector3 from, int delta ) => StartCoroutine(FlyRoutine(from, delta));

		IEnumerator FlyRoutine( Vector3 from, int delta )
		{
			_coins.Hide(delta);
			
			// Fly
			var coin = Instantiate( _coinTemplate, _coinTarget );
			coin.transform.position = from;
			
			yield return coin.transform
				.DOMove(_coinTarget.position, _duration)
				.SetEase(Ease.OutQuad)
				.WaitForCompletion();
			
			Destroy(coin.gameObject);
			
			// Change Coins
			DOVirtual
				.Float(delta, 0, 0.4f, v => _coins.Hide((int)v ))
				.SetEase(Ease.Linear);
		}
	}
}