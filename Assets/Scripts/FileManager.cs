using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    private static readonly string filename = "BinaryTreeData.csv";

    public static void WriteNodeToFile(string name, float price, int stock)
    {
        string filePath = Path.Combine(Application.persistentDataPath, filename);
        string row = name + ',' + price.ToString() + ',' + stock.ToString() + Environment.NewLine;
        File.AppendAllText(filePath, row);
    }

    public static void SaveNodesFromFile()
    {
        string filePath = Path.Combine(Application.persistentDataPath, filename);
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

    public static void SearchInFile(string name)
    {
        string filePath = Path.Combine(Application.persistentDataPath, filename);
        if (File.Exists(filePath))
        {
            string[] Nodes = File.ReadAllLines(filePath);

            foreach (var nodeData in Nodes)
            {
                string[] node = nodeData.Split(',');
                string nodeName = node[0];

                if (BinaryTree.SearchTree(BinaryTree.RootTree, nodeName) != null)
                {
                    //edit node from file
                    print("node is in file");
                }
                else
                {
                    //add node to file
                    print("node not found in file");
                }
            }
        }
        else
        {
            print("the .csv file does not exist");
        }
    }
}
