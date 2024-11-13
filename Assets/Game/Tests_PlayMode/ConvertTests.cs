using System;
using System.Collections;
using Game.Obj;
using Game.Configs;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.TestTools;

public class ConvertTests
{
    ConvertConfig Config = new ()
    {
        RawMaterialsCapacity = 12,
        ConvertedMaterialsCapacity = 12,
        InputAmount = 3,
        OutputAmount = 1,
        ConvertTime = 1,
    };
    
    [TestCase( true )]
    [TestCase( false )]
    public void TestConverterProperties( bool isOn )
    {
        ConvertConfig cfg = Config;
        ObjConverter converter = new ObjConverter( cfg, isOn );
        
        Assert.IsTrue( converter.RawMaterialsAmount == 0 );
        Assert.IsTrue( converter.ConvertedMaterialsAmount == 0 );
        Assert.IsTrue( converter.IsOn == isOn );
        Assert.IsTrue( Math.Abs(converter.ConvertTime.TotalSeconds - cfg.ConvertTime) < 0.1f);
        Assert.IsTrue( cfg.InputAmount == converter.CycleInput );
        Assert.IsTrue( cfg.OutputAmount == converter.CycleOutput );
        Assert.IsTrue( cfg.RawMaterialsCapacity == converter.RawCapacity );
        Assert.IsTrue( cfg.ConvertedMaterialsCapacity == converter.ConvertedCapacity );
    }
    
    [Test]
    public void InstantiateConverterWithNullConfig()
    {
        Assert.Catch<ArgumentNullException>(() => new ObjConverter(null, false));
    }
    
    [TestCase( -99 )]
    [TestCase( -1 )]
    [TestCase( 0 )]
    public void PushNegativeAmount( int pushingMaterials )
    {
        var converter = new ObjConverter( Config, false );
        
        Assert.Catch<ArgumentException>(() => converter.PushRaw( -1, out int _ ));
    }
    
    [Test]
    public void ToggleOnOffTest()
    {
        var converter = new ObjConverter( Config, false );
        
        converter.Toggle( true );
        
        Assert.IsTrue( converter.IsOn );
        
        converter.Toggle( true );
        
        Assert.IsTrue( converter.IsOn );
    }
    
    [TestCase(5, 5, 0)]
    [TestCase(2, 1, 1)]
    [TestCase(5, 5, 0)]
    [TestCase(5, 10, 0)]
    [TestCase(1, 2, 0)]
    public void TestExtraMaterials( int pushingMaterials, int rawCapacity , int expectedExtras )
    {
        ConvertConfig cfg = Config;
        cfg.RawMaterialsCapacity = rawCapacity;
        
        ObjConverter converter = new ObjConverter( cfg, false );
        converter.Toggle( false );
        converter.PushRaw( pushingMaterials, out int extra );
       
        Assert.AreEqual( expectedExtras, extra );
    }

    [UnityTest]
    public IEnumerator TestConverterCycles_MaxCapacity()
    {
        int pushAmount = 12;
        yield return TestConverterCycles(Config, pushAmount);
    }

    [UnityTest]
    public IEnumerator TestConverterCycles_6()
    {
        int pushAmount = 6;
        yield return TestConverterCycles(Config, pushAmount);
    }

    [UnityTest]
    public IEnumerator TestConverterCycles_Capacity11()
    {
        int pushAmount = 11; 
        yield return TestConverterCycles(Config, pushAmount);
    }
    
    IEnumerator TestConverterCycles( ConvertConfig cfg, int pushAmount )
    {
        // Setup
        int cycles = Mathf.FloorToInt(Mathf.Min(pushAmount, cfg.RawMaterialsCapacity) / (float)cfg.InputAmount);
        int expectedRawMaterials = pushAmount - (cycles * cfg.InputAmount);
        int expectedConvertedMaterials = cycles * cfg.OutputAmount;
        var converter = new ObjConverter( Config, false );
        
        converter.PushRaw( pushAmount, out int outOfCapacity );
        
        Assert.IsTrue( converter.RawMaterialsAmount == pushAmount );
        Assert.IsTrue( converter.ConvertedMaterialsAmount == 0 );
        
        converter.Toggle( true );
        
        yield return new WaitForSeconds( cycles *  cfg.ConvertTime + 0.1f );
        
        Assert.IsTrue( converter.RawMaterialsAmount == expectedRawMaterials );
        Assert.IsTrue( converter.ConvertedMaterialsAmount == expectedConvertedMaterials );
        
        Assert.IsTrue( Math.Abs(converter.ConvertTime.TotalSeconds - cfg.ConvertTime) < 0.1f);
        Assert.IsTrue( cfg.InputAmount == converter.CycleInput );
        Assert.IsTrue( cfg.OutputAmount == converter.CycleOutput );
        Assert.IsTrue( cfg.RawMaterialsCapacity == converter.RawCapacity );
        Assert.IsTrue( cfg.ConvertedMaterialsCapacity == converter.ConvertedCapacity );
    }
    
    [UnityTest]
    public IEnumerator TestConverterStopCase_4()
    {
        int pushAmount = Config.InputAmount * 4;
        yield return TestConverterStop(Config, pushAmount);
    }

    [UnityTest]
    public IEnumerator TestConverterStopCase_3()
    {
        int pushAmount = Config.InputAmount * 3;
        yield return TestConverterStop(Config, pushAmount);
    }

    [UnityTest]
    public IEnumerator TestConverterStopCase_1()
    {
        int pushAmount = Config.InputAmount * 2;
        yield return TestConverterStop(Config, pushAmount);
    }
    
    IEnumerator TestConverterStop( ConvertConfig cfg, int pushAmount )
    {
        var converter = new ObjConverter( Config, false );
        
        converter.PushRaw( pushAmount, out int _ );
        converter.Toggle( true );
        
        yield return new WaitForSeconds(  cfg.ConvertTime / 5 );
        
        converter.PushRaw( pushAmount, out int _ );
        
        yield return new WaitForSeconds(  cfg.ConvertTime / 5 );
        
        converter.Toggle( false );
        
        yield return new WaitForSeconds(  cfg.ConvertTime / 5 );
        
        Assert.AreEqual( cfg.RawMaterialsCapacity, converter.RawMaterialsAmount );
        Assert.AreEqual( 0, converter.ConvertedMaterialsAmount );
        
        converter.Toggle( true );
        
        int cycles = Mathf.FloorToInt(cfg.RawMaterialsCapacity / (float)cfg.InputAmount);
        int expectedRawMaterials = (Mathf.Min( converter.RawCapacity, pushAmount * 2 )) - (cycles * cfg.InputAmount);
        int expectedConvertedMaterials = cycles * cfg.OutputAmount;
        
        yield return new WaitForSeconds( cfg.ConvertTime * cycles + 0.1f );
        
        Assert.AreEqual( expectedRawMaterials, converter.RawMaterialsAmount );
        Assert.AreEqual( expectedConvertedMaterials, converter.ConvertedMaterialsAmount );
    }
}