using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryTree<T> where T : NodeKey
{
    public Node<T> RootTree = new Node<T>(null,null,null);

    /// <summary>
    /// recursive search in the binary tree to position a new node in the right place
    /// </summary>
    /// <param name="starting node for the function"></param>
    /// <param name="the node that needs adding"></param>
    /// <returns>the node in a certain position</returns>
    public Node<T> AddToTree(Node<T> parentNode, Node<T> newNode)
    {
        if (parentNode == null)
        {
            parentNode = newNode;
        }
        else
        {
            int value = newNode.Key.Name.ToLower().CompareTo(parentNode.Key.Name.ToLower());
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
                if (parentNode is ItemNode)
                {
                    ItemNode parent = (ItemNode)(NodeKey)parentNode.Key;
                    ItemNode node = (ItemNode)(NodeKey)newNode.Key;
                    parent.Stock += node.Stock;
                }
            }
        }
        return parentNode;
    }

    /// <summary>
    /// search the binary tree for a node and remove it
    /// </summary>
    /// <param name="the key name of the node to be removed"></param>
    public void RemoveFromTree(string nameToRemove)
    {
        Node<T> parentNode = RootTree;
        Node<T> currentNode = RootTree.Left;
        Node<T> foundNode = null;

        while (currentNode != null)
        {
            int value = nameToRemove.ToLower().CompareTo(currentNode.Key.Name.ToLower());
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
                Node<T> replaceingNode;
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

    /// <summary>
    /// recursively search for a node, and replace it with a copy of that node with edited values
    /// </summary>
    /// <param name="the node where to start searching"></param>
    /// <param name="the node to be replaced with"></param>
    /// <param name="the name of the node you want to edit"></param>
    public void EditNode(Node<T> parentNode, Node<T> editNode, string searchName)
    {
        if (parentNode != null)
        {
            if (parentNode.Key.Name.ToLower() == searchName.ToLower())
            {
                editNode.Right = parentNode.Right;
                editNode.Left = parentNode.Left;
                parentNode = editNode;
                return;
            }
            else
            {
                int value = searchName.ToLower().CompareTo(parentNode.Key.Name.ToLower());
                if (value > 0)
                {
                    if (parentNode.Right != null)
                    {
                        EditNode(parentNode.Right, editNode, searchName);
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
                        EditNode(parentNode.Left, editNode, searchName);
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

    /// <summary>
    /// using a while loop, search for a node by name
    /// </summary>
    /// <param name="searchNameof the node to search"></param>
    /// <returns>the node you searched for</returns>
    public Node<T> SearchTree(string searchName)
    {
        Node<T> parentNode = RootTree.Left;

        while (parentNode != null)
        {
            int value = searchName.ToLower().CompareTo(parentNode.Key.Name.ToLower());

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

    /// <summary>
    /// I use this to find the lowest value of the right branch, or the closest value from the left to the parent node
    /// </summary>
    /// <param name="the right node of the branch"></param>
    /// <returns>the node with the lowest value from the right or null if the funtion does not find it</returns>
    public Node<T> LowestValueRight(Node<T> subTree)
    {
        if (subTree == null)
        {
            Debug.Log("Tried to search for\"null\" in tree");
            return null;
        }
        Node<T> lowestValue = subTree;
        Node<T> parent = null;

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

    /// <summary>
    /// I use this to find the highest value of the left branch, or the closest value from the left to the parent node
    /// </summary>
    /// <param name="the right node of the branch"></param>
    /// <returns>the node with the highest value from the left or null if the funtion does not find it</returns>s
    public Node<T> HighestValueLeft(Node<T> subTree)
    {
        if (subTree == null)
        {
            Debug.Log("Tried to search for\"null\" in tree");
            return null;
        }
        Node<T> highestValue = subTree;
        Node<T> parent = null;

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

    /// <summary>
    /// find the parent of a certain node
    /// </summary>
    /// <param name="the child node you want to find the parent"></param>
    /// <returns></returns>
    private Node<T> GetParent(Node<T> child)
    {
        Node<T> curr = RootTree;
        Node<T> next = RootTree.Left;
        if (RootTree.Left == null)
        {
            return null;
        }

        while (next.Key.CompareTo(child.Key) != 0)
        {
            curr = next;
            if (next.Key.CompareTo(child.Key) > 0)
            {
                next = next.Left;
            }
            else if (next.Key.CompareTo(child.Key) < 0)
            {
                next = next.Right;
            }
        }
        return curr;
    }
}
