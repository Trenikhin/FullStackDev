namespace Game.SaveSystem
{
	using Cysharp.Threading.Tasks;

	public interface ISaver
	{
		int LastSaveVersion { get; }
		
		UniTask SaveAsync();
		UniTask LoadAsync( int ver );
	}
}