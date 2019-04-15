using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this class holds information about inventory items
/// </summary>
[Serializable]
public class ItemNode : NodeKey
{
    public float Price;
    public int Stock;
    public float Discount;

    public ItemNode(string name, float price, int stock) : base(name)
    {
        Name = name;
        Price = price;
        Stock = stock;
        Discount = 0;
    }

    public ItemNode(string name, float price, int stock, float discount) : base(name)
    {
        Name = name;
        Price = price;
        Stock = stock;
        Discount = discount;
    }
}
