using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public List<Clients> ClientsList = new List<Clients>();
    public List<CartItem> ShopingCartList = new List<CartItem>();
    public GameObject CartItem;
    public GameObject AddNewItem;
    public Transform ShoppingCartContents;
    public Text totalPrice;


    [SerializeField]
    private GameObject itemPrefab;
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

    [SerializeField]
    private Dropdown ClientsSelection;

    private string[] nodesData;
    private int index = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        FileManager.LoadNodesFromFile();
        ShowOnUI(BinaryTree.RootTree);
    }

    public void AddClients()
    {
        bool exists = false;
        foreach(var clientN in ClientsList)
        {
            if (clientN.ClientName.ToLower().Trim() == clientName.text.ToLower().Trim())
            {
                exists = true;
                print("Client already in list");
            }
        }

        if (!exists)
        {
            Clients client = new Clients(clientName.text);
            ClientsList.Add(client);
            Dropdown.OptionData newClient = new Dropdown.OptionData();
            newClient.text = clientName.text;
            ClientsSelection.options.Add(newClient);
        }
    }

    public void OnAddPress()
    {
        AddItems(itemName.text, float.Parse(itemPrice.text), int.Parse(itemStock.text));
    }

    public void OnRemovePress()
    {
        BinaryTree.RootTree = BinaryTree.RemoveFromTree(BinaryTree.RootTree, itemName.text);
    }

    public void RefreshNodes()
    {
        FileManager.DeleteTreeFile();
        FileManager.SaveNodesToFile(BinaryTree.RootTree);
        ShowOnUI(BinaryTree.RootTree);
    }

    public static void AddItems(string name, float price, int stock)
    {
        Node ItemNode = new Node(name, price, stock);
        BinaryTree.RootTree = BinaryTree.AddToTree(BinaryTree.RootTree, ItemNode);
    }

    //private void SaveNodes(Node node)
    //{
    //    if (node != null)
    //    {
    //        FileManager.SaveNodeToFile(node.ItemName, node.Price, node.Stock);
    //    }
    //
    //    if (node.Left != null)
    //    {
    //        SaveNodes(node.Left);
    //    }
    //
    //    if (node.Right != null)
    //    {
    //        SaveNodes(node.Right);
    //    }
    //}

    private void ShowOnUI(Node node)
    {
        if (node != null)
        {
            GameObject listItem = Instantiate(itemPrefab, itemsParent);
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
}
