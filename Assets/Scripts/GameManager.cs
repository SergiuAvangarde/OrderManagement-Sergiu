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
    private InputField clientName;

    [SerializeField]
    private InputField itemName;
    [SerializeField]
    private InputField itemPrice;
    [SerializeField]
    private InputField itemStock;

    private string[] nodesData;
    private int index = 0;

    private void Awake()
    {
        FileManager.SaveNodesFromFile();
        ShowOnUI(BinaryTree.RootTree);
    }

    public void AddClients()
    {
        Clients client = new Clients(clientName.text);
        ClientsList.Add(client);
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
        SaveNodes(BinaryTree.RootTree);
        ShowOnUI(BinaryTree.RootTree);
    }

    public static void AddItems(string name, float price, int stock)
    {
        //InventoryItem Object = new InventoryItem(name, price, stock);
        //Object.OnSale = false;
        Node ItemNode = new Node(name, price, stock);
        BinaryTree.RootTree = BinaryTree.AddToTree(BinaryTree.RootTree, ItemNode);
    }

    private void ShowOnUI(Node node)
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
            ShowOnUI(node.Left);
        }

        if (node.Right != null)
        {
            ShowOnUI(node.Right);
        }
    }

    private void SaveNodes(Node node)
    {
        if (node != null)
        {
            FileManager.WriteNodeToFile(node.ItemName, node.Price, node.Stock);
        }

        if (node.Left != null)
        {
            SaveNodes(node.Left);
        }

        if (node.Right != null)
        {
            SaveNodes(node.Right);
        }
    }
}
