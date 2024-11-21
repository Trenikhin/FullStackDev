using Game.Configs;
using Game.Obj;
using NUnit.Framework;
using UnityEngine;

public partial class ConverterTests
{
    [TestCase( true)]
    [TestCase( false )]
    public void Toggle(bool isOn)
    {
        var cfg = ScriptableObject.CreateInstance<ConvertConfig>();
        var converted = new Converter(cfg);
        converted.Toggle( isOn );
        
        Assert.IsTrue( converted.IsOn == isOn );
    }

    [TestCase( 3, 3, 0, 3, 1, 3, true, true  )]
    [TestCase( 6, 3, 1, 3, 1, 10,true, true  )]
    [TestCase( 6, 12, 3, 3, 1, 10,true, false  )]
    [TestCase( 3, 3, 3, 3, 1, 10,true, false  )]
    [TestCase( 6, 3, 1, 3, 1, 10,false, false  )]
    public void TryStartRecycling(
        int startRawAmount,
        int cycleInput,
        int startConvertedAmount,
        int convertedCapacity,
        int cycleOutput,
        int rawCapacity,
        bool isOn,
        bool expectedResult)
    {
        // Setup
        var cfg = ScriptableObject.CreateInstance<ConvertConfig>();
        cfg.RawMaterialsCapacity         = rawCapacity;
        cfg.ConvertedMaterialsCapacity   = convertedCapacity;
        cfg.InputAmount                  = cycleInput;
        cfg.OutputAmount                 = cycleOutput;
        
        var converted = new Converter(cfg, startRawAmount, startConvertedAmount, isOn);
        bool canStart = converted.TryStartRecycle();
        
        Assert.IsTrue( expectedResult == canStart );

        if (canStart)
        {
            Assert.AreEqual( startRawAmount - cycleInput, converted.RawMaterialsAmount );
        }
        else
        {
            Assert.AreEqual( startRawAmount, converted.RawMaterialsAmount );
        }
        
        Assert.AreEqual( startConvertedAmount, converted.ConvertedMaterialsAmount );   
    }


    [TestCase( 1, 0.5f, 10, 0, 10, 0 )]
    [TestCase( 1, 0.8f, 10, 0, 10, 0 )]
    [TestCase( 1, 1.8f, 10, 0, 10, 0 )]
    public void StopRecycling( 
        float convertTime,
        float deltaTime,
        int startRawAmount,
        int startConvertedAmount,
        int expectedRawAmount,
        int expectedConvertedAmount )
    {
        // Setup
        var cfg = ScriptableObject.CreateInstance<ConvertConfig>();
        cfg.ConvertTime = convertTime;
       
        var converted = new Converter(cfg, startRawAmount, startConvertedAmount, true);
        converted.TryStartRecycle();
        converted.Toggle( false );
        converted.TickRecycling( deltaTime );
        
        Assert.AreEqual( expectedRawAmount, converted.RawMaterialsAmount );
        Assert.AreEqual( expectedConvertedAmount, converted.ConvertedMaterialsAmount );   
    }
    
    [TestCase( 1, 0.5f, 10, 0, 10, 0 )]
    [TestCase( 1, 0.8f, 10, 0, 10, 0 )]
    [TestCase( 1, 1.7f, 10, 0, 7, 1 )]
    public void TestConvertCycle( 
        float convertTime,
        float deltaTime,
        int startRawAmount,
        int startConvertedAmount,
        int expectedRawAmount,
        int expectedConvertedAmount )
    {
        // Setup
        var cfg = ScriptableObject.CreateInstance<ConvertConfig>();
        cfg.ConvertTime = convertTime;
       
        var converted = new Converter(cfg, startRawAmount, startConvertedAmount, true);
        converted.TryStartRecycle();
        converted.TickRecycling( deltaTime );
        
        Assert.AreEqual( expectedRawAmount, converted.RawMaterialsAmount );
        Assert.AreEqual( expectedConvertedAmount, converted.ConvertedMaterialsAmount );   
    }
}
