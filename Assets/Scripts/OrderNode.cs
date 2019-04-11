using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderNode
{
    public OrderNode Left;
    public OrderNode Right;
    public string ClientName;
    public List<CartItem> OrderedItems = new List<CartItem>();

    public OrderNode()
    {

    }

    public OrderNode(string clientName)
    {
        ClientName = clientName;
    }

    public OrderNode(string name, List<CartItem> orderedItems)
    {
        ClientName = name;
        OrderedItems = orderedItems;
    }
}
