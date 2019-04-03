using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<Clients> ClientsList = new List<Clients>();
    public GameObject AddNewItem;

    [SerializeField]
    private GameObject item;
    [SerializeField]
    private Transform itemsParent;

    [SerializeField]
    private InputField itemName;
    [SerializeField]
    private InputField itemPrice;
    [SerializeField]
    private InputField itemStock;

    public void AddClients()
    {

    }

    public void OnAddPress()
    {
        AddItems(itemName.text, float.Parse(itemPrice.text), int.Parse(itemStock.text));
    }

    public void OnRemovePress()
    {
        BinaryTree.RootTree = BinaryTree.RemoveFromTree(BinaryTree.RootTree, itemName.text);
    }

    public void OnPrintPress()
    {
        PrintTree(BinaryTree.RootTree);
    }

    public static void AddItems(string name, float price, int stock)
    {
        //InventoryItem Object = new InventoryItem(name, price, stock);
        //Object.OnSale = false;
        Node ItemNode = new Node(name, price, stock);
        BinaryTree.RootTree = BinaryTree.AddToTree(BinaryTree.RootTree, ItemNode);
    }

    private void PrintTree(Node node)
    {
        if (node != null)
        {
            GameObject listItem = Instantiate(item, itemsParent);
            listItem.GetComponent<Toggle>().group = itemsParent.GetComponent<ToggleGroup>();
            InventoryItem itemComponent = listItem.GetComponent<InventoryItem>();
            itemComponent.ItemName = node.ItemName;
            itemComponent.Price = node.Price;
            itemComponent.Stock = node.Stock;
        }

        if (node.Left != null)
        {
            PrintTree(node.Left);
        }

        if (node.Right != null)
        {
            PrintTree(node.Right);
        }
    }
}
