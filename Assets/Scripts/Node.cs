using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Node
{
    public Node Left;
    public Node Right;
    public InventoryItem ItemObject;
    public string ItemName;

    public Node(string name, InventoryItem item)
    {
        ItemName = name;
        ItemObject = item;
        Left = null;
        Right = null;
        Debug.Log("node created " + ItemName);
    }
}
