using System.Collections;
using System.Collections.Generic;
using Game.Scripts;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class ConvertTests
{
    ObjConverter CreateConverter() => new ObjConverter( ConfigProvider.Instance.ConvertConfig, new TimeHelper() );
    ObjType CreateWood() => ConfigProvider.Instance.Wood.GetObj();
    ObjType CreatePlank() => ConfigProvider.Instance.Plank.GetObj();
    float _convertCycle => ConfigProvider.Instance.ConvertConfig.ConvertTime + 0.1f * ConfigProvider.Instance.ConvertConfig.ConvertTime;
    
    
    [Test]
    public void ToggleOnTest()
    {
        var converter = CreateConverter();
        
        converter.Toggle(  false );
        
        Assert.IsFalse( converter.IsOn );
    }
    
    [Test]
    public void ToggleOffTest()
    {
        var converter = CreateConverter();
        
        converter.Toggle(  true );
        
        Assert.IsTrue( converter.IsOn );
    }

    [UnityTest]
    public IEnumerator TestConverterCycles()
    {
        yield return TestConverterCycles1(5);
        yield return TestConverterCycles1(10);
        yield return TestConverterCycles1(15);
    }
    
    IEnumerator TestConverterCycles1(int amount)
    {
        ObjConverter converter = CreateConverter();
        var rawMaterials = new ObjStack(CreateWood(), amount);
        converter.Toggle(  false );
        int capacity = converter.RawCapacity - converter.RawMaterialsAmount;
        int expectedExtra = Mathf.Max( amount - capacity, 0 );
        int expectedRawBeforeConversion = converter.RawMaterialsAmount + amount - expectedExtra;
        int expectedConvertedBeforeConversion = 0;
        
        converter.Push( rawMaterials, out var outOfCapacity );
        
        Assert.IsTrue( outOfCapacity == expectedExtra );
        Assert.IsTrue( converter.RawCapacity == expectedRawBeforeConversion );
        Assert.IsTrue( converter.ConvertedMaterialsAmount == expectedConvertedBeforeConversion );
        converter.Toggle(  true );
        yield return new WaitForSeconds( _convertCycle );
        converter.Toggle(  false );
        var cycle1ExpectedRaw = Mathf.Max( expectedRawBeforeConversion - converter.CycleInput, 0 );
        var cycle1ExpectedConverted = Mathf.Max(expectedRawBeforeConversion + converter.CycleOutput, 0);
        
        Assert.IsTrue( converter.RawCapacity == cycle1ExpectedRaw );
        Assert.IsTrue( converter.ConvertedMaterialsAmount == cycle1ExpectedConverted );
        converter.Toggle(  true );
        yield return new WaitForSeconds( _convertCycle );
        
        var cycle2ExpectedRaw = Mathf.Max( cycle1ExpectedRaw - converter.CycleInput, 0 );
        var cycle2ExpectedConverted = Mathf.Max(cycle1ExpectedConverted + converter.CycleOutput, 0);
        
        Assert.IsTrue( converter.RawCapacity == cycle2ExpectedRaw );
        Assert.IsTrue( converter.ConvertedMaterialsAmount == cycle2ExpectedConverted );
    }
    
    IEnumerator TestConverterCycles2(int amount)
    {
        ObjConverter converter = CreateConverter();
        var rawMaterials = new ObjStack(CreateWood(), amount);
        
        converter.Toggle(  false );
        
        int capacity = converter.RawCapacity - converter.RawMaterialsAmount;
        int expectedExtra = Mathf.Max( amount - capacity, 0 );
        int expectedRawBeforeConversion = converter.RawMaterialsAmount + amount - expectedExtra;
        int expectedConvertedBeforeConversion = 0;
        
        converter.Push( rawMaterials, out var outOfCapacity );
        
        Assert.IsTrue( outOfCapacity == expectedExtra );
        Assert.IsTrue( converter.RawCapacity == expectedRawBeforeConversion );
        Assert.IsTrue( converter.ConvertedMaterialsAmount == expectedConvertedBeforeConversion );
        
        converter.Toggle(  true );
        yield return new WaitForSeconds( _convertCycle );
        
        var cycle1ExpectedRaw = Mathf.Max( expectedRawBeforeConversion - converter.CycleInput, 0 );
        var cycle1ExpectedConverted = Mathf.Max(expectedRawBeforeConversion + converter.CycleOutput, 0);
        
        var output = converter.Pull( 1 );
        
        Assert.IsTrue( output.Amount == 1 );
        Assert.IsTrue( output.ObjType == CreatePlank() );
        Assert.IsTrue( converter.RawCapacity == cycle1ExpectedRaw );
        Assert.IsTrue( converter.ConvertedMaterialsAmount == cycle1ExpectedConverted );
    }
}
