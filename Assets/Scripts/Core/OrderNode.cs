using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Order type node, it holds the information for a list of ordered items
/// </summary>
public class OrderNode : NodeKey
{
    public List<CartItem> OrderedItems = new List<CartItem>();

    public OrderNode(string name) : base(name)
    {
        Name = name;
    }

    public OrderNode(string name, List<CartItem> orderedItems) : base(name)
    {
        Name = name;
        OrderedItems = orderedItems;
    }
}
