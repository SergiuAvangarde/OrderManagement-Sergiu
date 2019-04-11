using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemNode
{
    public ItemNode Left;
    public ItemNode Right;
    public string ItemName;
    public float Price;
    public int Stock;
    public float Discount;

    public ItemNode()
    {
    
    }

    public ItemNode(string name, float price, int stock)
    {
        ItemName = name;
        Price = price;
        Stock = stock;
        Discount = 0;
    }

    public ItemNode(string name, float price, int stock, float discount)
    {
        ItemName = name;
        Price = price;
        Stock = stock;
        Discount = discount;
    }
}
