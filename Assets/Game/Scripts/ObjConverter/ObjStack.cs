namespace Game
{
	using System;
	using R3;

	public class ObjStack
	{
		ReactiveProperty<int> _amountRx;
		
		public int Amount { get; private set; }
		public int Capacity { get; private set; }

		public ReadOnlyReactiveProperty<int> AmountRx;
		
		public ObjStack( int amount, int capacity)
		{
			Amount		= amount;
			Capacity	= capacity;

			_amountRx	= new ReactiveProperty<int>(Amount);
			AmountRx	= _amountRx;
		}

		public void SetAmount(int amount)
		{
			Amount = (int)MathF.Min(Capacity, amount);
			_amountRx.Value = Amount;
		}
	}
}