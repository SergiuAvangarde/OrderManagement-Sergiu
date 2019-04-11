using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdersBinaryTree
{
    public OrderNode RootTree = new OrderNode("");

    public OrderNode AddToTree(OrderNode parentNode, OrderNode newNode)
    {
        if (parentNode == null)
        {
            parentNode = newNode;
        }
        else
        {
            int value = newNode.ClientName.ToLower().CompareTo(parentNode.ClientName.ToLower());
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
                GameManager.Instance.UIManagerComponent.PrintErrorMessage(Constants.ERROR_CLIENT_EXISTS);
            }
        }
        return parentNode;
    }

    public void RemoveFromTree(string nameToRemove)
    {
        OrderNode parentNode = RootTree;
        OrderNode currentNode = RootTree.Left;
        OrderNode foundNode = null;

        while (currentNode != null)
        {
            int value = nameToRemove.ToLower().CompareTo(currentNode.ClientName.ToLower());
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
                OrderNode replaceingNode;
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

    public OrderNode SearchTree(string searchName)
    {
        OrderNode parentNode = RootTree;

        while (parentNode != null)
        {
            int value = searchName.ToLower().CompareTo(parentNode.ClientName.ToLower());

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

    public OrderNode LowestValueRight(OrderNode subTree)
    {
        if (subTree == null)
        {
            Debug.Log("Tried to search for\"null\" in tree");
            return null;
        }
        OrderNode lowestValue = subTree;
        OrderNode parent = null;

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

    public OrderNode HighestValueLeft(OrderNode subTree)
    {
        if (subTree == null)
        {
            Debug.Log("Tried to search for\"null\" in tree");
            return null;
        }
        OrderNode highestValue = subTree;
        OrderNode parent = null;

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

    private OrderNode GetParent(OrderNode child)
    {
        OrderNode curr = RootTree;
        OrderNode next = RootTree.Left;
        if (RootTree.Left == null)
        {
            return null;
        }

        while (next.ClientName.ToLower().CompareTo(child.ClientName.ToLower()) != 0)
        {
            curr = next;
            if (next.ClientName.ToLower().CompareTo(child.ClientName.ToLower()) > 0)
            {
                next = next.Left;
            }
            else if (next.ClientName.ToLower().CompareTo(child.ClientName.ToLower()) < 0)
            {
                next = next.Right;
            }
        }
        return curr;
    }
}
