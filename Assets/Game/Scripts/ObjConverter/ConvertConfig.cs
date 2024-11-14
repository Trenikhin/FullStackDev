namespace Game.Configs
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.Design.Serialization;
	using System.Linq;
	using R3;
	using UnityEngine;

	[CreateAssetMenu(fileName = "ConvertConfig", menuName = "Configs/ConvertConfig", order = 0)]
	public class ConvertConfig : ScriptableObject
	{
		public int RawMaterialsCapacity = 12;
		public int ConvertedMaterialsCapacity = 12;
		public int InputAmount = 3;
		public int OutputAmount = 1;
		public float ConvertTime = 1;

		ReactiveProperty<int> SomeValue = new ReactiveProperty<int>();
	}
}