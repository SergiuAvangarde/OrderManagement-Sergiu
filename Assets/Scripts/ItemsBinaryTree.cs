using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsBinaryTree
{
    public ItemNode RootTree = new ItemNode("", 0, 0);

    public ItemNode AddToTree(ItemNode parentNode, ItemNode newNode)
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

    public void RemoveFromTree(string nameToRemove)
    {
        ItemNode parentNode = RootTree;
        ItemNode currentNode = RootTree.Left;
        ItemNode foundNode = null;

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
                ItemNode replaceingNode;
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
            GameManager.Instance.UIManagerComponent.PrintErrorMessage(Constants.ERROR_INVALID_NODE);
        }
    }

    public void EditItem(ItemNode parentNode, string searchName, string name, float price, int stock, float discount)
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
                        EditItem(parentNode.Right, searchName, name, price, stock, discount);
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
                        EditItem(parentNode.Left, searchName, name, price, stock, discount);
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

    public ItemNode SearchTree(string searchName)
    {
        ItemNode parentNode = RootTree.Left;

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

    public ItemNode LowestValueRight(ItemNode subTree)
    {
        if (subTree == null)
        {
            Debug.Log("Tried to search for\"null\" in tree");
            return null;
        }
        ItemNode lowestValue = subTree;
        ItemNode parent = null;

        while (lowestValue.Left != null)
        {
            parent = lowestValue;
            lowestValue = lowestValue.Left;
        }

        if (parent == null)
        {
            parent = GetParent(lowestValue);
            parent.Right = lowestValue.Right;
        }
        else
        {
            parent.Left = lowestValue.Right;
        }
        return lowestValue;
    }

    public ItemNode HighestValueLeft(ItemNode subTree)
    {
        if (subTree == null)
        {
            Debug.Log("Tried to search for\"null\" in tree");
            return null;
        }
        ItemNode highestValue = subTree;
        ItemNode parent = null;

        while (highestValue.Right != null)
        {
            parent = highestValue;
            highestValue = highestValue.Right;
        }

        if (parent == null)
        {
            parent = GetParent(highestValue);
            parent.Left = highestValue.Left;
        }
        else
        {
            parent.Right = highestValue.Left;
        }
        return highestValue;
    }

    private ItemNode GetParent(ItemNode child)
    {
        ItemNode curr = RootTree;
        ItemNode next = RootTree.Left;
        if (RootTree.Left == null)
        {
            return null;
        }

        while (next.ItemName.ToLower().CompareTo(child.ItemName.ToLower()) != 0)
        {
            curr = next;
            if (next.ItemName.ToLower().CompareTo(child.ItemName.ToLower()) > 0)
            {
                next = next.Left;
            }
            else if (next.ItemName.ToLower().CompareTo(child.ItemName.ToLower()) < 0)
            {
                next = next.Right;
            }
        }
        return curr;
    }
}
