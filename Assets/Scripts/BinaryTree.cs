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

        if (foundNode != null)
        {
            if (foundNode != parentNode)
            {
                bool deleteRight = false;
                if (foundNode == parentNode.Right)
                {
                    deleteRight = true;
                }

                if (foundNode.Left == null && foundNode.Right == null)
                {
                    // Removing a leaf
                    if (deleteRight)
                    {
                        parentNode.Right = null;
                    }
                    else
                    {
                        parentNode.Left = null;
                    }
                }
                else if (foundNode.Left == null || foundNode.Right == null)
                {
                    // Removing node with single child
                    if (foundNode.Left != null)
                    {
                        if (deleteRight)
                        {
                            parentNode.Right = foundNode.Left;
                        }
                        else
                        {
                            parentNode.Left = foundNode.Left;
                        }
                    }
                    else
                    {
                        if (deleteRight)
                        {
                            parentNode.Right = foundNode.Right;
                        }
                        else
                        {
                            parentNode.Left = foundNode.Right;
                        }
                    }
                }
                else if (foundNode.Left != null && foundNode.Right != null)
                {
                    // Removing node with two children
                    Node replaceingNode;
                    if (deleteRight)
                    {
                        replaceingNode = LowestValueRight(foundNode.Right);
                        //replaceingNode.Left = foundNode.Left;
                        replaceingNode.Right = foundNode.Right;
                        parentNode.Right = replaceingNode;
                    }
                    else
                    {
                        replaceingNode = HighestValueLeft(foundNode.Left);
                        replaceingNode.Left = foundNode.Left;
                        //replaceingNode.Right = foundNode.Right;
                        parentNode.Left = replaceingNode;
                    }
                }
            }
            else
            {
                if (foundNode.Left == null && foundNode.Right == null)
                {
                    parentNode = null;
                }
                else if (foundNode.Left == null || foundNode.Right == null)
                {
                    // Removing node with single child
                    if (foundNode.Left != null)
                    {
                        parentNode.Left = foundNode.Left.Left;
                        parentNode.Right = foundNode.Left.Right;
                        parentNode = foundNode.Left;
                    }
                    else
                    {
                        parentNode.Left = foundNode.Right.Left;
                        parentNode.Right = foundNode.Right.Right;
                        parentNode = foundNode.Right;
                    }
                }
                else if (foundNode.Left != null && foundNode.Right != null)
                {
                    // Removing node with two children
                    Node replaceingNode;
                    replaceingNode = LowestValueRight(foundNode.Right);
                    replaceingNode.Right = foundNode.Right;
                    replaceingNode.Left = foundNode.Left;
                    parentNode = replaceingNode;
                }
            }
        }
        else
        {
            GameManager.Instance.UIManagerComponent.PrintErrorMessage(Constants.ERROR_INVALID_NODE);
        }
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

    public static Node SearchTree(string searchName)
    {
        Node parentNode = RootTree;

        while (parentNode != null)
        {
            int value = searchName.ToLower().CompareTo(parentNode.ItemName.ToLower());

            if (value == 0)
            {
                return parentNode;
            }
            else if (value > 0)
            {
                parentNode = parentNode.Right;
            }
            else if (value < 0)
            {
                parentNode = parentNode.Left;
            }
        }
        return null;
    }

    public static Node LowestValueRight(Node subTree)
    {
        if (subTree.Left == null)
        {
            Node toReturn = subTree;
            subTree = null;
            return toReturn;
        }
        else
        {
            return LowestValueRight(subTree.Left);
        }
    }

    public static Node HighestValueLeft(Node subTree)
    {
        if (subTree.Right == null)
        {
            Node toReturn = subTree;
            subTree = null;
            return toReturn;
        }
        else
        {
            return HighestValueLeft(subTree.Right);
        }
    }
}
