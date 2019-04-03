using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orders : MonoBehaviour
{
    public Clients Client;
    public List<InventoryItem> OrderedItems = new List<InventoryItem>();
    public float TotalPrice;
    

    public Orders(Clients client, List<InventoryItem> itemsList)
    {
        Client = client;
        itemsList = OrderedItems;
    }
}
