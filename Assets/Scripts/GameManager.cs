using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<InventoryItem> Inventory = new List<InventoryItem>();
    public Node RootTree;

    public void AddItems(string name,float price, int quantity)
    {
        InventoryItem Object = new InventoryItem();
        Object.ItemName = name;
        Object.Price = price;
        Object.Quantity = quantity;
        Object.OnSale = false;
        Inventory.Add(Object);

        Node ItemNode = new Node();
        ItemNode.ItemName = name;
        ItemNode.ItemObject = Object;
        AddToTree(RootTree, ItemNode);
    }

    public Node SearchTree(Node parentNode, Node searchNode)
    {
        if(parentNode != null)
        {
            if(parentNode == searchNode)
            {
                return parentNode;
            }
            else
            {
                int value = searchNode.ItemName.CompareTo(parentNode.ItemName);
                if (value > 0)
                {
                    if (parentNode.Right != null)
                    {
                        return SearchTree(parentNode.Right, searchNode);
                    }
                    else
                    {
                        AddToTree(parentNode, searchNode);
                        return SearchTree(parentNode, searchNode);
                    }
                }
                else
                {
                    if (parentNode.Left != null)
                    {
                        return SearchTree(parentNode.Left, searchNode);
                    }
                    else
                    {
                        AddToTree(parentNode, searchNode);
                        return SearchTree(parentNode, searchNode);
                    }
                }
            }
        }
        else
        {
            AddToTree(parentNode, searchNode);
            return SearchTree(parentNode, searchNode);
        }
    }

    private void AddToTree(Node parentNode, Node newNode)
    {
        if (parentNode == null)
        {
            parentNode = newNode;
        }
        else
        {
            int value = newNode.ItemName.CompareTo(parentNode.ItemName);
            Debug.Log("value of node is: " + value);
            if (value > 0)
            {
                if (parentNode.Right == null)
                {
                    parentNode.Right = newNode;
                }
                else
                {
                    AddToTree(parentNode.Right, newNode);
                }
            }
            else
            {
                if (parentNode.Left == null)
                {
                    parentNode.Left = newNode;
                }
                else
                {
                    AddToTree(parentNode.Left, newNode);
                }
            }
        }
    }
}
