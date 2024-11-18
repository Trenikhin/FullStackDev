using System;
using Game.Configs;
using Game.Obj;
using NUnit.Framework;

public class ConverterTests
{
#region Instantiate
    [TestCase(4, 4, true)]
    [TestCase(3, 1, false)]
    [TestCase(1, 1, false)]
    [TestCase(0, 0, true)]
    public void Instantiate( int startRawAmount, int startConvertedAmount, bool isOn )
    {
        var cfg = GetBaseConfig;
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
        var cfg = GetBaseConfig;
        
        cfg.RawMaterialsCapacity = rawCapacity;
        cfg.ConvertedMaterialsCapacity = convertedCapacity;
        Assert.Catch<ArgumentException>(() => new Converter(cfg, startRawAmount, startConvertedAmount ));
    }
#endregion   
#region AddResources
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
        var cfg = GetBaseConfig;
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
        var converted = new Converter(GetBaseConfig);
        
        Assert.Catch<ArgumentException>(() => converted.AddResources( pushAmount, out _ ));
    }
#endregion
#region States
    [TestCase( true)]
    [TestCase( false )]
    public void Toggle(bool isOn)
    {
        var converted = new Converter(GetBaseConfig);
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
        var cfg = GetBaseConfig;
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
    [TestCase( 1, 1.5f, 10, 0, 7, 1 )]
    public void StopRecycling( 
        float convertTime,
        float deltaTime,
        int startRawAmount,
        int startConvertedAmount,
        int expectedRawAmount,
        int expectedConvertedAmount )
    {
        // Setup
        var cfg = GetBaseConfig;
        cfg.ConvertTime = convertTime;
        
        var converted = new Converter(cfg, startRawAmount, startConvertedAmount, true);
        converted.TryStartRecycle();
        converted.TickRecycling( deltaTime );
        converted.StopRecycling();
        
        Assert.AreEqual( expectedRawAmount, converted.RawMaterialsAmount );
        Assert.AreEqual( expectedConvertedAmount, converted.ConvertedMaterialsAmount );   
    }
    
    [Test]
    public void TryStopWhileNotRunning()
    {
        // Setup
        var cfg = GetBaseConfig;
        
        var converted = new Converter(cfg, 10, 0, true);
        
        Assert.Catch<Exception>(() =>  converted.StopRecycling() );
    }
#endregion
#region Usages

    ConvertConfig GetBaseConfig => new ConvertConfig()
    {
        RawMaterialsCapacity = 12,
        ConvertedMaterialsCapacity = 12,
        InputAmount = 3,
        OutputAmount = 1,
        ConvertTime = 1,
    };
    
#endregion
}
