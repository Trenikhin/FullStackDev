using System;
using Game.Configs;
using Game.Obj;
using NUnit.Framework;
using UnityEngine;

public partial class ConverterTests
{
	[TestCase( 2, 1, 1 )]
	[TestCase( 3, 1, 2 )]
	[TestCase( 10, 5, 5 )]
	[TestCase( 1, 1, 0 )]
	[TestCase( 5, 5, 0 )]
	[TestCase( 4, 5, 0 )]
	[TestCase( 4, 7, 0 )]
	public void AddResources(int pushAmount, int rawCapacity, int expectedExtra)
	{
		// Setup
		var cfg = ScriptableObject.CreateInstance<ConvertConfig>();
		cfg.RawMaterialsCapacity = rawCapacity;
		var converted = new Converter(cfg);
        
		converted.AddResources( pushAmount, out int outOfRange );
        
		Assert.AreEqual( expectedExtra, outOfRange );
		Assert.AreEqual( pushAmount - outOfRange, converted.RawMaterialsAmount );
	}
    
	[TestCase( -2 )]
	[TestCase( -1 )]
	[TestCase( -100 )]
	[TestCase( -123 )]
	public void AddResourcesWithWrongAmount(int pushAmount)
	{
		// Setup
		var cfg = ScriptableObject.CreateInstance<ConvertConfig>();
		var converted = new Converter(cfg);
        
		Assert.Catch<ArgumentException>(() => converted.AddResources( pushAmount, out _ ));
	}
}