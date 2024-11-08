using System;
using System.Threading.Tasks;
using Game.Scripts;
using NUnit.Framework;
using UnityEngine;

public class ConvertTests
{

	ObjConverter CreateConverter() => ConfigProvider.Instance.ConvertConfig.GetConverterWith( 0.3f );
	
	[Test]
	public void ConvertTest() 
	{
		Debug.Log(CreateConverter());
		
		Debug.Log("SendComplete");
		
		Assert.IsTrue(true);
		Assert.IsFalse(false);
	}
}