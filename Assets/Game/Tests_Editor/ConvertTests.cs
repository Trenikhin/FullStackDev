using System;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

public class ConvertTests
{
	
	[Test]
	public async Task ConvertTest() 
	{
		Debug.Log("Send");
		
		await Task.Delay( TimeSpan.FromSeconds(1 ) );
		
		Debug.Log("SendComplete");
		
		Assert.IsTrue(true);
		Assert.IsFalse(false);
	}
}