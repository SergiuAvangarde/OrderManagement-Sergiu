using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Node
{
    public Node Left;
    public Node Right;
    //public InventoryItem ItemObject;
    public string ItemName;
    public float Price;
    public int Stock;
    public bool OnSale;

    //public Node(InventoryItem item)
    //{
    //    //ItemName = name;
    //    ItemObject = item;
    //    Left = null;
    //    Right = null;
    //    Debug.Log("node created " + ItemObject.ItemName);
    //}

    public Node(string name, float price, int stock)
    {
        ItemName = name;
        Price = price;
        Stock = stock;
        OnSale = false;
    }
}
