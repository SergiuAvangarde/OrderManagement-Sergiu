using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    private static readonly string binerytreefile = "BinaryTreeData.csv";
    private static readonly string clientsfile = "ClientsData.csv";

    public static void SaveNodesToFile(Node node)
    {
        if (node != null)
        {
            string filePath = Path.Combine(Application.persistentDataPath, binerytreefile);
            string row = node.ItemName + ',' + node.Price.ToString() + ',' + node.Stock.ToString() + ',' + node.Discount.ToString() + Environment.NewLine;
            File.AppendAllText(filePath, row);
        }

        if (node.Left != null)
        {
            SaveNodesToFile(node.Left);
        }

        if (node.Right != null)
        {
            SaveNodesToFile(node.Right);
        }
    }

    public static void LoadNodesFromFile()
    {
        string filePath = Path.Combine(Application.persistentDataPath, binerytreefile);
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
                    Node newNode = new Node(nodeName, float.Parse(nodePrice), int.Parse(nodeStock), float.Parse(nodeDiscount));
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

    public static void DeleteTreeFile()
    {
        string filePath = Path.Combine(Application.persistentDataPath, binerytreefile);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public static void SaveClients(List<Clients> cList)
    {
        string filePath = Path.Combine(Application.persistentDataPath, clientsfile);

        foreach (var client in cList)
        {
            string row = client.ClientName + Environment.NewLine;
            File.AppendAllText(filePath, row);
        }
    }

    public static List<Clients> LoadClients()
    {
        string filePath = Path.Combine(Application.persistentDataPath, clientsfile);
        List<Clients> cList = new List<Clients>();

        if (File.Exists(filePath))
        {
            string[] Clients = File.ReadAllLines(filePath);

            foreach (var client in Clients)
            {
                Clients loadedClient = new Clients(client);
                cList.Add(loadedClient);
            }
        }
        return cList;
    }
}
