﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public UIManager UIManagerComponent;
    public ItemsManager ItemsManagerComponent;

    //public ClientList ClientsList = new ClientList();
    public List<Clients> ClientsList = new List<Clients>();
    public List<CartItem> ShopingCartList = new List<CartItem>();
    public Queue<CartItem> ShopingCartPool = new Queue<CartItem>();
    public GameObject CartItem;
    public GameObject AddNewItem;
    public Transform ShoppingCartContents;
    public double TotalPrice { get; set; }

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

        FileManager.LoadNodesFromFile();
        ClientsList = FileManager.LoadClients();
        UIManagerComponent.InitializeAllNodes(BinaryTree.RootTree);
    }

    private void Start()
    {
        for(int i=0; i<= ShoppingCartPoolSize; i++)
        {
            GameObject listItem = Instantiate(CartItem, ShoppingCartContents);
            CartItem itemValues = listItem.GetComponent<CartItem>();
            ShopingCartPool.Enqueue(itemValues);
        }
    }

    public void RefreshNodes()
    {
        FileManager.DeleteTreeFile();
        FileManager.SaveNodesToFile(BinaryTree.RootTree);
        UIManagerComponent.InitializeAllNodes(BinaryTree.RootTree);
    }
}