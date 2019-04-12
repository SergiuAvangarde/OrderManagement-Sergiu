using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node<T> /*: IComparable<Node<T>> where T : NodeKey*/
{
    public Node<T> Left;
    public Node<T> Right;
    public T Key;

    public Node(T keyName)
    {
        Key = keyName;
    }

    public Node(T key, Node<T> left, Node<T> right)
    {
        Left = left;
        Right = right;
        Key = key;
    }

    //public int CompareTo(Node<T> other)
    //{
    //    if (other == null)
    //        Debug.LogError("[Node.cs] Tried to compare Node with null");
    //
    //    return Key.CompareTo(other.Key);
    //}
}
