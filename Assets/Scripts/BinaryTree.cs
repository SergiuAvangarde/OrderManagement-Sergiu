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
                GameManager.Instance.UIManagerComponent.PrintErrorMessage(Constants.ERROR_ITEM_EXISTS);
                parentNode.Stock += newNode.Stock;
            }
        }
        return parentNode;
    }

    public static void RemoveFromTree(string nameToRemove)
    {
        Node parentNode = RootTree;
        Node currentNode = RootTree;
        Node foundNode = null;

        while (currentNode != null)
        {
            int value = nameToRemove.ToLower().CompareTo(currentNode.ItemName.ToLower());
            if (value == 0)
            {
                foundNode = currentNode;
                break;
            }
            else if (value < 0)
            {
                parentNode = currentNode;
                currentNode = currentNode.Left;
            }
            else if (value > 0)
            {
                parentNode = currentNode;
                currentNode = currentNode.Right;
            }
        }

        if (foundNode == null)
        {
            GameManager.Instance.UIManagerComponent.PrintErrorMessage(Constants.ERROR_INVALID_NODE);
        }
        else
        {
            if (foundNode.Right != null && foundNode.Left == null)
            {
                foundNode = foundNode.Right;
                foundNode.Right = null;
            }
            else if (foundNode.Right == null && foundNode.Left != null)
            {
                foundNode = foundNode.Left;
                foundNode.Left = null;
            }
            else if (foundNode.Right != null && foundNode.Left != null)
            {
                foundNode = LowestValue(foundNode.Right);
            }
            else
            {
                foundNode = null;
            }
        }

        //if (parentNode != null)
        //{
        //    if (parentNode.ItemName == nameToRemove)
        //    {
        //         if (parentNode.Right != null && parentNode.Left == null)
        //         {
        //             parentNode = parentNode.Right;
        //             parentNode.Right = null;
        //         }
        //         else if (parentNode.Right == null && parentNode.Left != null)
        //         {
        //             parentNode = parentNode.Left;
        //             parentNode.Left = null;
        //         }
        //         else if (parentNode.Right != null && parentNode.Left != null)
        //         {
        //             parentNode = LowestValue(parentNode.Right);
        //         }
        //         else
        //         {
        //             parentNode = null;
        //         }
        //     }
        //    else
        //    {
        //        int value = nameToRemove.ToLower().CompareTo(parentNode.ItemName.ToLower());
        //        if (value > 0)
        //        {
        //            if (parentNode.Right != null)
        //            {
        //                RemoveFromTree(parentNode.Right, nameToRemove);
        //            }
        //            else
        //            {
        //                GameManager.Instance.UIManagerComponent.PrintErrorMessage(Constants.ERROR_INVALID_NODE);
        //            }
        //        }
        //        else
        //        {
        //            if (parentNode.Left != null)
        //            {
        //                RemoveFromTree(parentNode.Left, nameToRemove);
        //            }
        //            else
        //            {
        //                GameManager.Instance.UIManagerComponent.PrintErrorMessage(Constants.ERROR_INVALID_NODE);
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    GameManager.Instance.UIManagerComponent.PrintErrorMessage(Constants.ERROR_INVALID_NODE);
        //}
    }

    public static void EditNode(Node parentNode, string searchName, string name, float price, int stock, float discount)
    {
        if (parentNode != null)
        {
            if (parentNode.ItemName.ToLower() == searchName.ToLower())
            {
                parentNode.ItemName = name;
                parentNode.Price = price;
                parentNode.Stock = stock;
                parentNode.Discount = discount;
            }
            else
            {
                int value = searchName.ToLower().CompareTo(parentNode.ItemName.ToLower());
                if (value > 0)
                {
                    if (parentNode.Right != null)
                    {
                        EditNode(parentNode.Right, searchName, name, price, stock, discount);
                    }
                    else
                    {
                        GameManager.Instance.UIManagerComponent.PrintErrorMessage(Constants.ERROR_INVALID_NODE);
                    }
                }
                else
                {
                    if (parentNode.Left != null)
                    {
                        EditNode(parentNode.Left, searchName, name, price, stock, discount);
                    }
                    else
                    {
                        GameManager.Instance.UIManagerComponent.PrintErrorMessage(Constants.ERROR_INVALID_NODE);
                    }
                }
            }
        }
        else
        {
            GameManager.Instance.UIManagerComponent.PrintErrorMessage(Constants.ERROR_INVALID_NODE);
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
            Node toReturn = subTree;
            Debug.Log("make the lowest value null");
            subTree = null;
            return toReturn;
        }
        else
        {
            return LowestValue(subTree.Left);
        }
    }
}
