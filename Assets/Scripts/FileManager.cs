﻿using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    private static readonly string binerytreefile = "BinaryTreeData.csv";
    private static readonly string clientsfile = "ClientsData.json";

    //public static void SaveNodeToFile(string name, float price, int stock)
    //{
    //    string filePath = Path.Combine(Application.persistentDataPath, binerytreefile);
    //    string row = name + ',' + price.ToString() + ',' + stock.ToString() + Environment.NewLine;
    //    File.AppendAllText(filePath, row);
    //}

    public static void SaveNodesToFile(Node node)
    {
        if (node != null)
        {
            string filePath = Path.Combine(Application.persistentDataPath, binerytreefile);
            string row = node.ItemName + ',' + node.Price.ToString() + ',' + node.Stock.ToString() + Environment.NewLine;
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

                if (BinaryTree.SearchTree(BinaryTree.RootTree, nodeName) == null)
                {
                    Node newNode = new Node(nodeName, float.Parse(nodePrice), int.Parse(nodeStock));
                    BinaryTree.RootTree = BinaryTree.AddToTree(BinaryTree.RootTree, newNode);
                }
                else
                {
                    continue;
                }
            }
        }
        else
        {
            print("the .csv file does not exist");
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

    public static void SaveClients(ClientList cList)
    {
        string contents = JsonUtility.ToJson(cList, true);
        File.WriteAllText(clientsfile, contents);
    }
}