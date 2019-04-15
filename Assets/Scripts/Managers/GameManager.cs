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

    public BinaryTree<ItemNode> ItemsTreeRoot = new BinaryTree<ItemNode>();
    public BinaryTree<OrderNode> OrdersTreeRoot = new BinaryTree<OrderNode>();
    public List<CartItem> ShopingCartList = new List<CartItem>();
    public Queue<CartItem> ShopingCartPool = new Queue<CartItem>();
    public Queue<CartItem> OrderHistoryPool = new Queue<CartItem>();
    public GameObject CartItem;
    public GameObject OrderItem;
    public Transform ShoppingCartContents;
    public Transform OrderHistoryContents;
    public double TotalPrice { get; set; }
    public double OrdersTotalPrice { get; set; }

    private int poolSize = 20;

    /// <summary>
    /// load the informations from the .csv files, and refresh the UI to show the new information loaded
    /// </summary>
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

    /// <summary>
    /// initialize the object pool of Cart items for the shopping cart and order history
    /// </summary>
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

    /// <summary>
    /// reload all of the items from the list and the clients
    /// </summary>
    public void RefreshNodes()
    {
        UIManagerComponent.Index = 0;
        UIManagerComponent.RefreshNodesList(ItemsTreeRoot.RootTree.Left);
        UIManagerComponent.ClientsSelection.ClearOptions();
        UIManagerComponent.RefreshClientsDropdown(OrdersTreeRoot.RootTree.Left);
        UIManagerComponent.ClientsSelection.value = 0;
        UIManagerComponent.CalculateTotalPrice();
    }

    /// <summary>
    /// when the aplication closes, the information of the binary trees is saved to the .csv files
    /// </summary>
    private void OnApplicationQuit()
    {
        FileManager.DeleteTreeFiles();
        FileManager.SaveItemsToFile(ItemsTreeRoot.RootTree.Left);
        FileManager.SaveClients(OrdersTreeRoot.RootTree.Left);
    }
}
