using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject AddNewItem;
    [SerializeField]
    private InputField ItemName;
    [SerializeField]
    private InputField ItemPrice;
    [SerializeField]
    private InputField ItemQuantity;


    private InventoryList listOfItems = new InventoryList();
    private int index;

    public void OnAddPress()
    {
        BinaryTree.AddItems(ItemName.text, float.Parse(ItemPrice.text), int.Parse(ItemQuantity.text));
    }

    public void OnRemovePress()
    {
        BinaryTree.RootTree = BinaryTree.RemoveFromTree(BinaryTree.RootTree, ItemName.text);
    }

    public void OnPrintPress()
    {
        index = 1;
        PrintTree(BinaryTree.RootTree);
    }

    private void PrintTree(Node node)
    {
        if (node != null)
        {
            print("node " + index + " item " + node.ItemName);
            print("node " + index + " price: " + node.ItemObject.Price);
            print("node " + index + " quantity: " + node.ItemObject.Quantity);
            listOfItems.ItemsList.Add(node.ItemObject);
            index++;
        }

        if (node.Right != null)
        {
            PrintTree(node.Right);
        }

        if (node.Left != null)
        {
            PrintTree(node.Left);
        }
    }

    public void SerializeTree()
    {
        //listOfItems.ItemsList.Sort();
        FileManager.WriteFile(listOfItems);
    }
}
