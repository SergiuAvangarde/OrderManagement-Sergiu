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

    public List<CartItem> ShopingCartList = new List<CartItem>();
    public Queue<CartItem> ShopingCartPool = new Queue<CartItem>();
    public Queue<CartItem> OrderHistoryPool = new Queue<CartItem>();
    public GameObject CartItem;
    public GameObject OrderItem;
    public Transform ShoppingCartContents;
    public Transform OrderHistoryContents;
    public double TotalPrice { get; set; }
    public double OrdersTotalPrice { get; set; }

    public BinaryTree<ItemNode> ItemsTreeRoot = new BinaryTree<ItemNode>();
    public BinaryTree<OrderNode> OrdersTreeRoot = new BinaryTree<OrderNode>();

    private int poolSize = 10;

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

        UIManagerComponent.RefreshNodesList(ItemsTreeRoot.RootTree.Left);
        UIManagerComponent.RefreshClientsDropdown(OrdersTreeRoot.RootTree.Left);
    }

    private void Start()
    {
        for (int i = 0; i <= poolSize; i++)
        {
            GameObject listItem = Instantiate(CartItem, ShoppingCartContents);
            GameObject orderListItem = Instantiate(OrderItem, OrderHistoryContents);
            CartItem itemValues = listItem.GetComponent<CartItem>();
            CartItem orderValues = orderListItem.GetComponent<CartItem>();
            ShopingCartPool.Enqueue(itemValues);
            OrderHistoryPool.Enqueue(orderValues);
        }
    }

    public void RefreshNodes()
    {
        UIManagerComponent.Index = 0;
        UIManagerComponent.RefreshNodesList(ItemsTreeRoot.RootTree.Left);
        UIManagerComponent.ClientsSelection.ClearOptions();
        UIManagerComponent.RefreshClientsDropdown(OrdersTreeRoot.RootTree.Left);
        UIManagerComponent.ClientsSelection.value = 0;
        UIManagerComponent.CalculateTotalPrice();
    }

    private void OnApplicationQuit()
    {
        FileManager.DeleteTreeFiles();
        FileManager.SaveItemsToFile(ItemsTreeRoot.RootTree.Left);
        FileManager.SaveClients(OrdersTreeRoot.RootTree.Left);
    }
}
