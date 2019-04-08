using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Clients
{
    public string ClientName;
    public List<Orders> OrdersHistory = new List<Orders>();


    public Clients(string name)
    {
        ClientName = name;
    }

    public void AddNewOrder(Orders order)
    {
        OrdersHistory.Add(order);
    }
}

//[Serializable]
//public class ClientList
//{
//    public List<Clients> ClientsList = new List<Clients>();
//}
