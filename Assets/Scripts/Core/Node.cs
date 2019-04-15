using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// generic type node, it can be of type ItemNode or OrderNode
/// it has the key value wich is neutral for both types of nodes
/// </summary>
/// <typeparam name="T"></typeparam>
public class Node<T>
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
}
