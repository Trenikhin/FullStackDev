namespace Game.Scripts
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.Design.Serialization;
	using System.Linq;
	using R3;

	[System.Serializable]
	public class ConvertOperation
	{
		public Convert From;
		public Convert To;
	}
	
	[System.Serializable]
	public class Convert
	{
		public ObjConfig Obj;
		public int Amount;
	}
	
	public class ConvertConfig
	{
		public int RawMaterialsCapacity = 12;
		public int ConvertedMaterialsCapacity = 12;
		public int InputAmount = 3;
		public int OutputAmount = 1;
		public float ConvertTime = 1;
		public ObjConfig InputMaterial;
		public ObjConfig OutputMaterial;
		
		// Setup convertables
		public List<ConvertOperation> ConvertOperations = new List<ConvertOperation>()
		{
			new ConvertOperation()
			{
				// Input
				From = new Convert()
				{
					Obj = new ObjConfig()
					{
						Name = "Wood"
					},
					Amount = 3,
				},
				
				// Output
				To = new Convert()
				{
					Obj = new ObjConfig()
					{
						Name = "Plank"
					},
					Amount = 3,
				},
			}
		};

		public bool CanConvert(ObjConfig n) => ConvertOperations
			.Select(c => c.From.Obj.Name)
			.Contains( n.Name );
		
		public bool CanConvert(ObjType n) => ConvertOperations
			.Select(c => c.From.Obj.Name)
			.Contains( n.Name );
		
		public bool CanConvert(string name) => ConvertOperations
			.Select(c => c.From.Obj.Name)
			.Contains( name );
	}

	public class ObjsConfig
	{
		public List<ObjConfig> _objsConfigs = new ();
		
		public ObjConfig GetConfig(string name) => _objsConfigs.FirstOrDefault(c => c.Name == name);
	}
	
	public class ObjConfig
	{
		public string Name;

		public ObjType GetObj() => new ObjType( Name, 1 );
	}
	
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
	
	public struct ObjType : IEquatable<ObjType>
	{
		public string Name { get; private set; }
		public int Weight { get; private set; }
		
		public ObjType(string name, int weight)
		{
			Name = name;
			Weight = weight;
		}

		public override int GetHashCode()
		{
			return Name?.GetHashCode() ?? 0; // Если Name null, возвращаем 0
		}
		
		public static bool operator ==(ObjType left, ObjType right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(ObjType left, ObjType right)
		{
			return !left.Equals(right);
		}

		public override bool Equals(object obj)
		{
			if (obj is ObjType other)
			{
				return string.Equals(Name, other.Name, StringComparison.Ordinal);
			}
			return false;
		}
		
		public bool Equals(ObjType other)
		{
			return Name == other.Name && Weight == other.Weight;
		}
	}
}