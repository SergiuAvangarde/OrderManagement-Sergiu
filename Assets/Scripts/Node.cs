using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Node
{
    public Node Left;
    public Node Right;
    public string ItemName;
    public float Price;
    public int Stock;
    public float Discount;

    public Node(string name, float price, int stock)
    {
        ItemName = name;
        Price = price;
        Stock = stock;
        Discount = 0;
    }

    public Node(string name, float price, int stock, float discount)
    {
        ItemName = name;
        Price = price;
        Stock = stock;
        Discount = discount;
    }
}
