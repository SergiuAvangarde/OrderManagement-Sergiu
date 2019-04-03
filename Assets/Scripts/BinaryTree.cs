using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryTree : MonoBehaviour
{
    public static Node RootTree = null;

    public static Node AddToTree(Node parentNode, Node newNode)
    {
        if (parentNode == null)
        {
            parentNode = newNode;
        }
        else
        {
            int value = newNode.ItemName.ToLower().CompareTo(parentNode.ItemName.ToLower());
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
                parentNode.Stock += newNode.Stock;
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
                    return LowestValue(parentNode.Right);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                int value = toRemove.ToLower().CompareTo(parentNode.ItemName.ToLower());
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

    public static Node SearchTree(Node parentNode, string searchName)
    {
        if (parentNode != null)
        {
            if (parentNode.ItemName.ToLower() == searchName.ToLower())
            {
                return parentNode;
            }
            else
            {
                int value = searchName.ToLower().CompareTo(parentNode.ItemName.ToLower());
                if (value > 0)
                {
                    if (parentNode.Right != null)
                    {
                        return SearchTree(parentNode.Right, searchName);
                    }
                }
                else
                {
                    if (parentNode.Left != null)
                    {
                        return SearchTree(parentNode.Left, searchName);
                    }
                }
            }
        }
        return null;
    }

    public static Node LowestValue(Node subTree)
    {
        if (subTree.Left == null)
        {
            return subTree;
        }
        else
        {
            return LowestValue(subTree.Left);
        }
    }
}
