using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    private static readonly string itemsTreeFile = "BinaryTreeData.csv";
    private static readonly string clientsTreeFile = "ClientsData.csv";

    public static void SaveItemsToFile(ItemNode node)
    {
        if (node != null)
        {
            string filePath = Path.Combine(Application.persistentDataPath, itemsTreeFile);
            string row = node.ItemName + ',' + node.Price.ToString() + ',' + node.Stock.ToString() + ',' + node.Discount.ToString() + Environment.NewLine;
            File.AppendAllText(filePath, row);
        }

        if (node.Left != null)
        {
            SaveItemsToFile(node.Left);
        }

        if (node.Right != null)
        {
            SaveItemsToFile(node.Right);
        }
    }

    public static void LoadItemsFromFile()
    {
        string filePath = Path.Combine(Application.persistentDataPath, itemsTreeFile);
        if (File.Exists(filePath))
        {
            string[] Nodes = File.ReadAllLines(filePath);

            foreach (var nodeData in Nodes)
            {
                string[] node = nodeData.Split(',');
                string nodeName = node[0];
                string nodePrice = node[1];
                string nodeStock = node[2];
                string nodeDiscount = node[3];

                if (GameManager.Instance.ItemsTreeRoot.SearchTree(nodeName) == null)
                {
                    ItemNode newNode = new ItemNode(nodeName, float.Parse(nodePrice), int.Parse(nodeStock), float.Parse(nodeDiscount));
                    GameManager.Instance.ItemsTreeRoot.RootTree.Left = GameManager.Instance.ItemsTreeRoot.AddToTree(GameManager.Instance.ItemsTreeRoot.RootTree.Left, newNode);
                }
                else
                {
                    continue;
                }
            }
        }
        else
        {
            GameManager.Instance.UIManagerComponent.PrintErrorMessage("the .csv file does not exist");
        }
    }

    public static void DeleteTreeFiles()
    {
        string ItemsPath = Path.Combine(Application.persistentDataPath, itemsTreeFile);
        string OrdersPath = Path.Combine(Application.persistentDataPath, clientsTreeFile);

        if (File.Exists(ItemsPath))
        {
            File.Delete(ItemsPath);
        }

        if (File.Exists(OrdersPath))
        {
            File.Delete(OrdersPath);
        }
    }

    public static void SaveClients(OrderNode node)
    {
        if (node != null)
        {
            string filePath = Path.Combine(Application.persistentDataPath, itemsTreeFile);
            string orders = null;
            if (node.OrderedItems != null)
            {
                foreach (var order in node.OrderedItems)
                {
                    orders += '&' + order.NodeItem.ItemName + ';' + order.Price + ';' + order.Quantity;
                }
            }
            string row = node.ClientName + ',' + orders + Environment.NewLine;
            File.AppendAllText(filePath, row);
        }

        if (node.Left != null)
        {
            SaveClients(node.Left);
        }

        if (node.Right != null)
        {
            SaveClients(node.Right);
        }
    }

    public static void LoadClients()
    {
        string filePath = Path.Combine(Application.persistentDataPath, clientsTreeFile);
        if (File.Exists(filePath))
        {
            string[] Nodes = File.ReadAllLines(filePath);

            foreach (var nodeData in Nodes)
            {
                string[] node = nodeData.Split(',');
                string ClientName = node[0];
                string OrdersString = null;
                if (node.Length > 1)
                {
                    OrdersString = node[1];
                }
                List<CartItem> OrderedItems = new List<CartItem>();

                if (!string.IsNullOrEmpty(OrdersString))
                {
                    string[] Orders = OrdersString.Split('&');
                    foreach (var order in Orders)
                    {
                        string[] orderInfo = order.Split(';');
                        CartItem item = new CartItem();
                        item.NodeItem = GameManager.Instance.ItemsTreeRoot.SearchTree(orderInfo[0]);
                        item.Price = float.Parse(orderInfo[1]);
                        item.Quantity = int.Parse(orderInfo[2]);
                        OrderedItems.Add(item);
                    }
                }

                if (GameManager.Instance.ItemsTreeRoot.SearchTree(ClientName) == null)
                {
                    OrderNode newNode = new OrderNode(ClientName, OrderedItems);
                    GameManager.Instance.OrdersTreeRoot.RootTree.Left = GameManager.Instance.OrdersTreeRoot.AddToTree(GameManager.Instance.OrdersTreeRoot.RootTree.Left, newNode);
                }
                else
                {
                    Debug.Log("should update the Client information from node");
                    continue;
                }
            }
        }
        else
        {
            GameManager.Instance.UIManagerComponent.PrintErrorMessage("the .csv file does not exist");
        }
    }
}
