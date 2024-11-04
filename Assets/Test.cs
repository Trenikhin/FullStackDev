using System.Collections;
using System.Collections.Generic;
using Game.Utilities;
using Inventories;
using UnityEngine;


public class Test : MonoBehaviour
{
    
    
    void Start()
    {
        var Inventory = new Inventory(4, 2);
        //var Inventory = new Inventory(2, 2, new KeyValuePair<Item, Vector2Int>(new Item(1, 1), new Vector2Int(1, 1)));

        var i = new Item(1, 1);
        
        Debug.Log($"W: {Inventory.Width}");
        Debug.Log($"H: {Inventory.Height}");
        Debug.Log($"C: {Inventory.Count}");
        
        Inventory.AddItem(i);
        
        Debug.Log($"C: {Inventory.Count}");
        
        //Inventory.Map.ForEach( p => Debug.Log(p) ) ;
    }
}
