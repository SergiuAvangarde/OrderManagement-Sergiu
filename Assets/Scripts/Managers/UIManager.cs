using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Queue<InventoryItem> ItemsQueue = new Queue<InventoryItem>();
    public List<InventoryItem> UsedItemsList = new List<InventoryItem>();
    public int Index { get; set; } = 0;
    public Dropdown ClientsSelection;

    [SerializeField]
    private GameObject optionsPanel;
    [SerializeField]
    private GameObject optionsDropdown;
    [SerializeField]
    private GameObject itemPrefab;
    [SerializeField]
    private Transform itemsParent;
    [SerializeField]
    private Text totalPriceField;
    [SerializeField]
    private Text ErrorMessage;
    [SerializeField]
    private InputField searchInput;

    private int PooledItemsNmber = 20;

    private void Start()
    {
        foreach (Clients client in GameManager.Instance.ClientsList)
        {
            Dropdown.OptionData newClient = new Dropdown.OptionData();
            newClient.text = client.ClientName;
            ClientsSelection.options.Add(newClient);
        }
        ClientsSelection.value = 0;
        CalculateTotalPrice();

        for (int i = 0; i <= PooledItemsNmber; i++)
        {
            GameObject listItem = Instantiate(itemPrefab, itemsParent);
            listItem.GetComponent<Toggle>().group = itemsParent.GetComponent<ToggleGroup>();
            InventoryItem itemComponent = listItem.GetComponent<InventoryItem>();
            ItemsQueue.Enqueue(itemComponent);
        }
    }

    public void SelectClient()
    {
        foreach (CartItem item in GameManager.Instance.ShopingCartList)
        {
            item.gameObject.SetActive(false);
            item.AddedToCart = false;
            GameManager.Instance.ShopingCartPool.Enqueue(item);
        }
        GameManager.Instance.ShopingCartList.Clear();
    }

    public void CalculateTotalPrice()
    {
        totalPriceField.text = "Total: " + string.Format("{0:0.00}", GameManager.Instance.TotalPrice) + "$";
    }

    public void PrintErrorMessage(string message)
    {
        ErrorMessage.text = message;
    }

    public void InitializeNode(Node node)
    {
        try
        {
            InventoryItem item = ItemsQueue.Dequeue();
            UsedItemsList.Add(item);
            item.NodeItem = node;
            item.gameObject.SetActive(true);
        }
        catch
        {
            GameObject listItem = Instantiate(itemPrefab, itemsParent);
            listItem.GetComponent<Toggle>().group = itemsParent.GetComponent<ToggleGroup>();
            InventoryItem itemComponent = listItem.GetComponent<InventoryItem>();
            ItemsQueue.Enqueue(itemComponent);
            InventoryItem item = ItemsQueue.Dequeue();
            UsedItemsList.Add(item);
            item.NodeItem = node;
            item.gameObject.SetActive(true);
        }
    }

    public void RefreshNodesList(Node node)
    {
        if (node != null)
        {
            if (Index <= UsedItemsList.Count - 1)
            {
                UsedItemsList[Index].NodeItem = node;
                UsedItemsList[Index].OnEnable();
                Index++;
            }
            else
            {
                InitializeNode(node);
                Index++;
            }

            if (node.Left != null && Index <= UsedItemsList.Count - 1)
            {
                RefreshNodesList(node.Left);
            }
            else if (node.Left != null && Index > UsedItemsList.Count - 1)
            {
                InitializeNode(node);
                RefreshNodesList(node.Left);
            }

            if (node.Right != null && Index <= UsedItemsList.Count - 1)
            {
                RefreshNodesList(node.Right);
            }
            else if (node.Right != null && Index > UsedItemsList.Count - 1)
            {
                InitializeNode(node);
                RefreshNodesList(node.Right);
            }
        }
    }

    public void SearchItems()
    {
        if (!string.IsNullOrEmpty(searchInput.text))
        {
            foreach (var item in UsedItemsList)
            {
                string itemName = item.NodeItem.ItemName.Trim().ToLower();
                string searchName = searchInput.text.Trim().ToLower();
                if (itemName.Contains(searchName))
                {
                    item.gameObject.SetActive(true);
                }
                else
                {
                    item.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            foreach (var item in UsedItemsList)
            {
                item.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Open options panel; set position to mouse coordinates
    /// </summary>
    /// <param name="elem"></param>
    public void OpenOptionsPanel()
    {
        optionsPanel.SetActive(true);
        SetOptionsAnchor();
        optionsDropdown.transform.position = Input.mousePosition;
    }

    /// <summary>
    /// Close options panel
    /// </summary>
    public void CloseOptionsPanel()
    {
        optionsPanel.SetActive(false);
    }

    /// <summary>
    /// Set dropdown's pivot based on mouse coordinates to avoid showing the panel outside the screen
    /// </summary>
    private void SetOptionsAnchor()
    {
        float xValue = 0;
        float yValue = 0;
        if (Input.mousePosition.x + optionsDropdown.GetComponent<RectTransform>().sizeDelta.x > Screen.width)
        {
            xValue = 1;
        }
        else
        {
            xValue = 0;
        }

        if (Input.mousePosition.y - optionsDropdown.GetComponent<RectTransform>().sizeDelta.y > 0)
        {
            yValue = 1;
        }
        else
        {
            yValue = 0;
        }

        optionsDropdown.GetComponent<RectTransform>().pivot = new Vector2(xValue, yValue);
        optionsDropdown.SetActive(true);
    }


}
