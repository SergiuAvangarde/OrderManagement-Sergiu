using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orders : MonoBehaviour
{
    public List<CartItem> OrderedItems = new List<CartItem>();

    public Orders(List<CartItem> itemsList)
    {
        itemsList = OrderedItems;
    }
}
