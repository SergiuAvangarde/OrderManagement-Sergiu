using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Queue<InventoryItem> ItemsList = new Queue<InventoryItem>();

    [SerializeField]
    private GameObject itemPrefab;
    [SerializeField]
    private Transform itemsParent;
    [SerializeField]
    private Dropdown ClientsSelection;
    [SerializeField]
    private Text totalPriceField;
    [SerializeField]
    private Text ErrorMessage;

    private int PooledItemsNmber = 20;

    private void Start()
    {
        foreach (var client in GameManager.Instance.ClientsList)
        {
            Dropdown.OptionData newClient = new Dropdown.OptionData();
            newClient.text = client.ClientName;
            ClientsSelection.options.Add(newClient);
        }
        CalculateTotalPrice();

        for(int i = 0; i <= PooledItemsNmber; i++)
        {
            GameObject listItem = Instantiate(itemPrefab, itemsParent);
            listItem.GetComponent<Toggle>().group = itemsParent.GetComponent<ToggleGroup>();
            InventoryItem itemComponent = listItem.GetComponent<InventoryItem>();
            ItemsList.Enqueue(itemComponent);
        }
    }

    public void CalculateTotalPrice()
    {
        totalPriceField.text = "Total: " + GameManager.Instance.TotalPrice + "$";
    }

    public void InitializeNode(Node node)
    {
        try
        {
            InventoryItem item = ItemsList.Dequeue();
            item.NodeItem = node;
            item.gameObject.SetActive(true);
        }
        catch
        {
            GameObject listItem = Instantiate(itemPrefab, itemsParent);
            listItem.GetComponent<Toggle>().group = itemsParent.GetComponent<ToggleGroup>();
            InventoryItem itemComponent = listItem.GetComponent<InventoryItem>();
            ItemsList.Enqueue(itemComponent);
            InventoryItem item = ItemsList.Dequeue();
            item.NodeItem = node;
            item.gameObject.SetActive(true);
        }
    }

    public void PrintErrorMessage(string message)
    {
        ErrorMessage.text = message;
    }

    public void InitializeAllNodes(Node node)
    {
        if (node != null)
        {
            try
            {
                InventoryItem item = ItemsList.Dequeue();
                item.NodeItem = node;
                item.gameObject.SetActive(true);
            }
            catch
            {
                GameObject listItem = Instantiate(itemPrefab, itemsParent);
                listItem.GetComponent<Toggle>().group = itemsParent.GetComponent<ToggleGroup>();
                InventoryItem itemComponent = listItem.GetComponent<InventoryItem>();
                ItemsList.Enqueue(itemComponent);
                InventoryItem item = ItemsList.Dequeue();
                item.NodeItem = node;
                item.gameObject.SetActive(true);
            }
        }

        if (node.Left != null)
        {
            InitializeAllNodes(node.Left);
        }

        if (node.Right != null)
        {
            InitializeAllNodes(node.Right);
        }
    }
}
