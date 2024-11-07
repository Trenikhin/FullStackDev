using System.Collections;
using Game.Scripts;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ConvertTests
{
    ObjConverter CreateConverter() => ConfigProvider.Instance.ConvertConfig.GetConverter();
    ObjType CreateWood() => ConfigProvider.Instance.Wood.GetObj();
    ObjType CreatePlank() => ConfigProvider.Instance.Plank.GetObj();
    
    
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
    public IEnumerator TestConverterCapacity()
    { 
        var converter = CreateConverter();
        var amount = 5;
        
        var rawMaterials = new ObjStack
        (
            CreateWood(), amount
        );
        
        converter.PushRaw( rawMaterials, out var outOfCapacity );
        
        yield return new WaitForSeconds ((float)converter.ConvertTime.TotalSeconds + + 0.1f) ;

        Assert.IsTrue( outOfCapacity == 0 );
    }
}
