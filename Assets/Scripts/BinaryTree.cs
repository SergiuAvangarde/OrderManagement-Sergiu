using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryTree : MonoBehaviour
{
    public static Node RootTree = null;

    public static void AddItems(string name, float price, int quantity)
    {
        InventoryItem Object = new InventoryItem();
        Object.ItemName = name;
        Object.Price = price;
        Object.Quantity = quantity;
        Object.OnSale = false;

        Node ItemNode = new Node(name, Object);
        RootTree = AddToTree(RootTree, ItemNode);
    }

    public static Node SearchTree(Node parentNode, string searchName)
    {
        if (parentNode != null)
        {
            if (parentNode.ItemName == searchName)
            {
                return parentNode;
            }
            else
            {
                int value = searchName.CompareTo(parentNode.ItemName);
                if (value > 0)
                {
                    if (parentNode.Right != null)
                    {
                        return SearchTree(parentNode.Right, searchName);
                    }
                    else
                    {
                        //Node ItemNode = new Node();
                        //ItemNode.ItemName = searchName;
                        //AddToTree(parentNode.Right, ItemNode);
                        return SearchTree(parentNode.Right, searchName);
                    }
                }
                else
                {
                    if (parentNode.Left != null)
                    {
                        return SearchTree(parentNode.Left, searchName);
                    }
                    else
                    {
                        //Node ItemNode = new Node();
                        //ItemNode.ItemName = searchName;
                        //AddToTree(parentNode.Left, ItemNode);
                        return SearchTree(parentNode.Left, searchName);
                    }
                }
            }
        }
        else
        {
            //Node ItemNode = new Node();
            //ItemNode.ItemName = searchName;
            //AddToTree(parentNode, ItemNode);
            return SearchTree(parentNode, searchName);
        }
    }

    public static Node AddToTree(Node parentNode, Node newNode)
    {
        if (parentNode == null)
        {
            parentNode = newNode;
        }
        else
        {
            int value = newNode.ItemName.CompareTo(parentNode.ItemName);
            if (value > 0)
            {
                parentNode.Right = AddToTree(parentNode.Right, newNode);
            }
            else if (value < 0)
            {
                parentNode.Left = AddToTree(parentNode.Left, newNode);
            }
            else
            {
                print("Item already in tree");
                parentNode.ItemObject.Quantity += newNode.ItemObject.Quantity;
            }
        }
        return parentNode;
    }

    public static Node RemoveFromTree(Node parentNode, string toRemove)
    {
        if (parentNode != null)
        {
            if (parentNode.ItemName == toRemove)
            {
                if (parentNode.Right != null && parentNode.Left == null)
                {
                    return parentNode.Right;
                }
                else if (parentNode.Right == null && parentNode.Left != null)
                {
                    return parentNode.Left;
                }
                else if (parentNode.Right != null && parentNode.Left != null)
                {
                    return LowerValue(parentNode.Right);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                int value = toRemove.CompareTo(parentNode.ItemName);
                if (value > 0)
                {
                    if (parentNode.Right != null)
                    {
                        return RemoveFromTree(parentNode.Right, toRemove);
                    }
                    else
                    {
                        print("the node does not exist");
                        return parentNode;
                    }
                }
                else
                {
                    if (parentNode.Left != null)
                    {
                        return RemoveFromTree(parentNode.Left, toRemove);
                    }
                    else
                    {
                        print("the node does not exist");
                        return parentNode;
                    }
                }
            }
        }
        else
        {
            print("the node does not exist");
            return null;
        }
    }

    public static Node LowerValue(Node subTree)
    {
        if (subTree.Left == null)
        {
            return subTree;
        }
        else
        {
            return LowerValue(subTree.Left);
        }
    }
}
