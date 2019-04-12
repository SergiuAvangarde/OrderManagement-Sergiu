using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Queue<InventoryItem> ItemsQueue = new Queue<InventoryItem>();
    public List<InventoryItem> UsedItemsList = new List<InventoryItem>();
    public List<CartItem> UsedOrderList = new List<CartItem>();
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
    private InputField editClientNameInput;
    [SerializeField]
    private Text totalPriceField;
    [SerializeField]
    private Text ordersTotalPriceField;
    [SerializeField]
    private Text ErrorMessage;
    [SerializeField]
    private InputField searchInput;

    private int PooledItemsNmber = 20;

    private void Start()
    {
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
        ResetOrdersHistory();
        ShowOrdersHistory();
        GameManager.Instance.ShopingCartList.Clear();
    }

    public void EditClient()
    {
        string CurrentName = ClientsSelection.options[ClientsSelection.value].text;
        GameManager.Instance.OrdersTreeRoot.EditClient(GameManager.Instance.OrdersTreeRoot.RootTree.Left, CurrentName, editClientNameInput.text);
        Dropdown.OptionData newClient = new Dropdown.OptionData();
        newClient.text = editClientNameInput.text;
        ClientsSelection.options.Remove(ClientsSelection.options[ClientsSelection.value]);
        ClientsSelection.options.Add(newClient);
        GameManager.Instance.RefreshNodes();
        editClientNameInput.text = "";
    }

    public void RemoveClient()
    {
        GameManager.Instance.OrdersTreeRoot.RemoveFromTree(ClientsSelection.options[ClientsSelection.value].text);
        ClientsSelection.options.Remove(ClientsSelection.options[ClientsSelection.value]);
        ClientsSelection.value = 0;
    }

    public void ResetPriceField()
    {
        GameManager.Instance.TotalPrice = 0;
        GameManager.Instance.OrdersTotalPrice = 0;
        GameManager.Instance.UIManagerComponent.CalculateTotalPrice();
    }

    public void ShowOrdersHistory()
    {
        List<CartItem> selectedClientOrders = GameManager.Instance.OrdersTreeRoot.SearchTree(ClientsSelection.options[ClientsSelection.value].text).OrderedItems;

        foreach (var item in selectedClientOrders)
        {
            try
            {
                CartItem orderItem = GameManager.Instance.OrderHistoryPool.Dequeue();
                orderItem.NodeItem = item.NodeItem;
                orderItem.Price = item.Price;
                orderItem.Quantity = item.Quantity;
                orderItem.Discount = item.Discount;
                GameManager.Instance.OrdersTotalPrice += item.Price * item.Quantity;
                orderItem.AddedToCart = true;
                orderItem.gameObject.SetActive(true);
                UsedOrderList.Add(orderItem);
            }
            catch
            {
                GameObject orderItem = Instantiate(GameManager.Instance.OrderItem, GameManager.Instance.OrderHistoryContents);
                CartItem cartComponent = orderItem.GetComponent<CartItem>();
                GameManager.Instance.OrderHistoryPool.Enqueue(cartComponent);
                CartItem cartItem = GameManager.Instance.OrderHistoryPool.Dequeue();
                cartItem.NodeItem = item.NodeItem;
                cartItem.Price = item.Price;
                cartItem.Quantity = item.Quantity;
                cartItem.Discount = item.Discount;
                GameManager.Instance.OrdersTotalPrice += item.Price * item.Quantity;
                cartItem.AddedToCart = true;
                cartItem.gameObject.SetActive(true);
                UsedOrderList.Add(cartItem);
            }
        }
        CalculateTotalPrice();
    }

    public void ResetOrdersHistory()
    {
        foreach (var item in UsedOrderList)
        {
            item.AddedToCart = false;
            item.gameObject.SetActive(false);
            GameManager.Instance.OrderHistoryPool.Enqueue(item);
        }
    }

    public void CalculateTotalPrice()
    {
        totalPriceField.text = "Total: " + string.Format("{0:0.00}", GameManager.Instance.TotalPrice) + "$";
        ordersTotalPriceField.text = "Total: " + string.Format("{0:0.00}", GameManager.Instance.OrdersTotalPrice) + "$";
    }

    public void PrintErrorMessage(string message)
    {
        ErrorMessage.text = message;
    }


    public void InitializeNode(ItemNode node)
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

    public void RefreshNodesList(ItemNode node)
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

    public void RefreshClientsDropdown(OrderNode node)
    {
        if (node != null)
        {
            Dropdown.OptionData newClient = new Dropdown.OptionData();
            newClient.text = node.ClientName;
            ClientsSelection.options.Add(newClient);
            ClientsSelection.value = 0;

            if (node.Right != null)
            {
                RefreshClientsDropdown(node.Right);
            }
            if (node.Left != null)
            {
                RefreshClientsDropdown(node.Left);
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
