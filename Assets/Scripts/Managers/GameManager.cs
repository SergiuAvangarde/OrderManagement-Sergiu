using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public UIManager UIManagerComponent;
    public ItemsManager ItemsManagerComponent;

    //public List<Clients> ClientsList = new List<Clients>();
    public List<CartItem> ShopingCartList = new List<CartItem>();
    public Queue<CartItem> ShopingCartPool = new Queue<CartItem>();
    public GameObject CartItem;
    public GameObject AddNewItem;
    public Transform ShoppingCartContents;
    public double TotalPrice { get; set; }

    public ItemsBinaryTree ItemsTreeRoot = new ItemsBinaryTree();
    public OrdersBinaryTree OrdersTreeRoot = new OrdersBinaryTree();

    private string[] nodesData;
    private int ShoppingCartPoolSize = 10;

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

        FileManager.LoadItemsFromFile();
        FileManager.LoadClients();
        UIManagerComponent.Index = 0;

        PrintTree(ItemsTreeRoot.RootTree.Left);

        UIManagerComponent.RefreshNodesList(ItemsTreeRoot.RootTree.Left);
        UIManagerComponent.RefreshClientsDropdown(OrdersTreeRoot.RootTree.Left);
    }

    private void Start()
    {
        for (int i = 0; i <= ShoppingCartPoolSize; i++)
        {
            GameObject listItem = Instantiate(CartItem, ShoppingCartContents);
            CartItem itemValues = listItem.GetComponent<CartItem>();
            ShopingCartPool.Enqueue(itemValues);
        }
    }

    public void RefreshNodes()
    {
        FileManager.DeleteTreeFiles();
        FileManager.SaveItemsToFile(ItemsTreeRoot.RootTree.Left);
        FileManager.SaveClients(OrdersTreeRoot.RootTree.Left);
        UIManagerComponent.Index = 0;
        UIManagerComponent.RefreshNodesList(ItemsTreeRoot.RootTree.Left);
        UIManagerComponent.RefreshClientsDropdown(OrdersTreeRoot.RootTree.Left);
        UIManagerComponent.CalculateTotalPrice();
    }

    public void PrintTree(ItemNode node)
    {
        if(node != null)
        {
            Debug.Log(node.ItemName);

            if(node.Right != null)
            {
                Debug.Log("right " + node.Right.ItemName);
                PrintTree(node.Right);
            }

            if (node.Left != null)
            {
                Debug.Log("left " + node.Left.ItemName);
                PrintTree(node.Left);
            }

        }
    }
}
