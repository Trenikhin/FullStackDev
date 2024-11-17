using System;
using System.Collections;
using System.Collections.Generic;
using Game.Configs;
using Game.Obj;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.TestTools;

public class ConverterTests
{
    ConvertConfig Config = new ConvertConfig()
    {
        RawMaterialsCapacity = 12,
        ConvertedMaterialsCapacity = 12,
        InputAmount = 3,
        OutputAmount = 1,
        ConvertTime = 1,
    };
    
    [TestCase(4, 4, true)]
    [TestCase(3, 1, false)]
    [TestCase(1, 1, false)]
    [TestCase(0, 0, true)]
    public void Instantiate( int startRawAmount, int startConvertedAmount, bool isOn )
    {
        var converted = new Converter(Config, startRawAmount, startConvertedAmount, isOn);
        
        Assert.IsTrue( converted.IsOn == isOn );
        Assert.AreEqual( startRawAmount, converted.RawMaterialsAmount );
        Assert.AreEqual( startConvertedAmount, converted.ConvertedMaterialsAmount );
        Assert.AreEqual( Config.RawMaterialsCapacity, converted.RawCapacity );
        Assert.AreEqual( Config.ConvertedMaterialsCapacity, converted.ConvertedCapacity );
        Assert.AreEqual( Config.InputAmount, converted.CycleInput );
        Assert.AreEqual( Config.OutputAmount, converted.CycleOutput );
        Assert.AreEqual( Config.ConvertTime, converted.ConvertTime );
    }
    
    [Test]
    public void InstantiateWithNullConfig(  )
    {
        Assert.Catch<ArgumentNullException>(() => new Converter(null));
    }
    
    [TestCase( -1, 1, 1, 1 )]
    [TestCase( 1, -1, 1, 1 )]
    [TestCase( 1, 0, -1, 1 )]
    [TestCase( 1, 0, 1, -1 )]
    [TestCase( -1, -1, 1, -1 )]
    [TestCase( 2, 1, 1, 1 )]
    [TestCase( 1, 1, 2, 1 )]
    [TestCase( 2, 1, 2, 1 )]
    [TestCase( 3, 1, 3, 1 )]
    [TestCase( 6, 5, 3, 1 )]
    public void InstantiateWithWrongAmount(int startRawAmount, int rawCapacity, int startConvertedAmount, int convertedCapacity)
    {
        Config.RawMaterialsCapacity = rawCapacity;
        Config.ConvertedMaterialsCapacity = convertedCapacity;
        Assert.Catch<ArgumentException>(() => new Converter(Config, startRawAmount, startConvertedAmount ));
    }
    
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
        var cfg = Config;
        cfg.RawMaterialsCapacity = rawCapacity;
        var converted = new Converter(cfg);
        
        converted.AddResources( pushAmount, out int outOfRange );
        
        Assert.AreEqual( expectedExtra, outOfRange );
    }
    
    [TestCase( -2 )]
    [TestCase( -1 )]
    [TestCase( -100 )]
    [TestCase( -123 )]
    public void AddResourcesWithWrongAmount(int pushAmount)
    {
        // Setup
        var converted = new Converter(Config);
        
        Assert.Catch<ArgumentException>(() => converted.AddResources( pushAmount, out _ ));
    }
    
    [TestCase( true)]
    [TestCase( false )]
    public void Toggle(bool isOn)
    {
        var converted = new Converter(Config);
        converted.Toggle( isOn );
        
        Assert.IsTrue( converted.IsOn == isOn );
    }
    
    
    public void TryStartRecycling(int startRawAmount, int rawCapacity, int startConvertedAmount, int convertedCapacity,  bool expectedResult)
    {
        Config.RawMaterialsCapacity = rawCapacity;
        Config.ConvertedMaterialsCapacity = convertedCapacity;
        
        var converted = new Converter(Config, startRawAmount, startConvertedAmount, true);

        bool canStart = converted.TryStartRecycle();
        
        Assert.IsTrue( expectedResult == canStart );
    }
}
