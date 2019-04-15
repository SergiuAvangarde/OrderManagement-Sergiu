using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// key type value for binary tree nodes, this value is necessary to compare nodes betwen them
/// </summary>
public class NodeKey : IComparable<NodeKey>, IEqualityComparer<NodeKey>
{
    public string Name;

    protected NodeKey(string name)
    {
        Name = name;
    }

    public int CompareTo(NodeKey other)
    {
        return Name.CompareTo(other.Name);
    }

    public bool Equals(NodeKey x, NodeKey y)
    {
        return x.Name.ToLower() == y.Name.ToLower();
    }

    public int GetHashCode(NodeKey other)
	{
		return 1;
	}
}
