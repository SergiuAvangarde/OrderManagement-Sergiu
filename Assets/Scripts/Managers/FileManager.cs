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

    /// <summary>
    /// Save item information from the binary tree to an .csv file
    /// </summary>
    /// <param name="root of the tree"></param>
    public static void SaveItemsToFile(Node<ItemNode> node)
    {
        if (node != null)
        {
            string filePath = Path.Combine(Application.persistentDataPath, itemsTreeFile);
            string row = node.Key.Name + ',' + node.Key.Price.ToString() + ',' + node.Key.Stock.ToString() + ',' + node.Key.Discount.ToString() + Environment.NewLine;
            File.AppendAllText(filePath, row);

            if (node.Left != null)
            {
                SaveItemsToFile(node.Left);
            }

            if (node.Right != null)
            {
                SaveItemsToFile(node.Right);
            }
        }
    }

    /// <summary>
    /// load the information of the items from the .csv files to the binary tree
    /// </summary>
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
                    ItemNode newItem = new ItemNode(nodeName, float.Parse(nodePrice), int.Parse(nodeStock), float.Parse(nodeDiscount));
                    Node<ItemNode> newNode = new Node<ItemNode>(newItem);
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
            var textFile = Resources.Load<TextAsset>("BinaryTreeData");
            File.AppendAllText(filePath, textFile.ToString());
            LoadItemsFromFile();
        }
    }

    /// <summary>
    /// delete all of the .csv files containing information about the binary tree or the clients
    /// </summary>
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

    /// <summary>
    /// save the binary tree cointaining information about the Clients and the orders to a .csv file
    /// </summary>
    /// <param name="root of the tree"></param>
    public static void SaveClients(Node<OrderNode> node)
    {
        if (node != null)
        {
            string filePath = Path.Combine(Application.persistentDataPath, clientsTreeFile);
            string orders = null;
            if (node.Key.OrderedItems != null)
            {
                foreach (var order in node.Key.OrderedItems)
                {
                    orders += '&' + order.NodeItem.Name + ';' + order.Price + ';' + order.Quantity + ';' + order.Discount;
                }
            }
            string row = node.Key.Name + ',' + orders + Environment.NewLine;
            File.AppendAllText(filePath, row);

            if (node.Left != null)
            {
                SaveClients(node.Left);
            }

            if (node.Right != null)
            {
                SaveClients(node.Right);
            }
        }
    }

    /// <summary>
    /// load the client information from the .csv file
    /// </summary>
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
                        if (!string.IsNullOrEmpty(order))
                        {
                            string[] orderInfo = order.Split(';');
                            CartItem item = new CartItem();
                            item.NodeItem = GameManager.Instance.ItemsTreeRoot.SearchTree(orderInfo[0]).Key;
                            item.Price = float.Parse(orderInfo[1]);
                            item.Quantity = int.Parse(orderInfo[2]);
                            item.Discount = int.Parse(orderInfo[3]);
                            OrderedItems.Add(item);
                        }
                    }
                }

                if (GameManager.Instance.ItemsTreeRoot.SearchTree(ClientName) == null)
                {
                    OrderNode newOrder = new OrderNode(ClientName, OrderedItems);
                    Node<OrderNode> newNode = new Node<OrderNode>(newOrder);
                    GameManager.Instance.OrdersTreeRoot.RootTree.Left = GameManager.Instance.OrdersTreeRoot.AddToTree(GameManager.Instance.OrdersTreeRoot.RootTree.Left, newNode);
                }
                else
                {
                    continue;
                }
            }
        }
        else
        {
            var textFile = Resources.Load<TextAsset>("ClientsData");
            File.AppendAllText(filePath, textFile.ToString());
            LoadClients();
        }
    }
}
