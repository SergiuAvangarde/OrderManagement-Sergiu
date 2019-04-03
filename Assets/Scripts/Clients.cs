using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clients : MonoBehaviour
{
    public string ClientName;
    public List<Orders> OrdersHistory = new List<Orders>();


    public Clients(string name)
    {
        ClientName = name;
    }

    public void AddNewOrder(Orders order)
    {
        order.Client = this;
        OrdersHistory.Add(order);
    }
}
