using System;
using Game.Configs;
using Game.Obj;
using NUnit.Framework;
using UnityEngine;

public partial class ConverterTests
{

    [TestCase(4, 4, true)]
    [TestCase(3, 1, false)]
    [TestCase(1, 1, false)]
    [TestCase(0, 0, true)]
    public void Instantiate( int startRawAmount, int startConvertedAmount, bool isOn )
    {
        var cfg = ScriptableObject.CreateInstance<ConvertConfig>();
        var converted = new Converter(cfg, startRawAmount, startConvertedAmount, isOn);
        
        Assert.IsTrue( converted.IsOn == isOn );
        Assert.AreEqual( startRawAmount, converted.RawMaterialsAmount );
        Assert.AreEqual( startConvertedAmount, converted.ConvertedMaterialsAmount );
        Assert.AreEqual( cfg.RawMaterialsCapacity, converted.RawCapacity );
        Assert.AreEqual( cfg.ConvertedMaterialsCapacity, converted.ConvertedCapacity );
        Assert.AreEqual( cfg.InputAmount, converted.CycleInput );
        Assert.AreEqual( cfg.OutputAmount, converted.CycleOutput );
        Assert.AreEqual( cfg.ConvertTime, converted.ConvertTime );
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
        var cfg = ScriptableObject.CreateInstance<ConvertConfig>();
        
        cfg.RawMaterialsCapacity = rawCapacity;
        cfg.ConvertedMaterialsCapacity = convertedCapacity;
        Assert.Catch<ArgumentException>(() => new Converter(cfg, startRawAmount, startConvertedAmount ));
    }
}
