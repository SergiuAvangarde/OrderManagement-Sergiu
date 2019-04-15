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

    /// <summary>
    /// initialize the object pool for the inventory items list
    /// </summary>
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

    /// <summary>
    /// refresh the shopping cart information and order history
    /// </summary>
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
        editClientNameInput.text = ClientsSelection.options[ClientsSelection.value].text;
    }

    /// <summary>
    /// edit a client name from the binary tree
    /// </summary>
    public void EditClient()
    {
        string CurrentName = ClientsSelection.options[ClientsSelection.value].text;
        Node<OrderNode> editedNode = GameManager.Instance.OrdersTreeRoot.SearchTree(CurrentName);
        editedNode.Key.Name = editClientNameInput.text;
        GameManager.Instance.OrdersTreeRoot.EditNode(GameManager.Instance.OrdersTreeRoot.RootTree.Left, editedNode, CurrentName);
        Dropdown.OptionData newClient = new Dropdown.OptionData();
        newClient.text = editClientNameInput.text;
        ClientsSelection.options.Remove(ClientsSelection.options[ClientsSelection.value]);
        ClientsSelection.options.Add(newClient);
        GameManager.Instance.RefreshNodes();
        editClientNameInput.text = "";
    }

    /// <summary>
    /// delete a client entirelly
    /// </summary>
    public void RemoveClient()
    {
        GameManager.Instance.OrdersTreeRoot.RemoveFromTree(ClientsSelection.options[ClientsSelection.value].text);
        ClientsSelection.options.Remove(ClientsSelection.options[ClientsSelection.value]);
        ClientsSelection.value = 0;
    }

    /// <summary>
    /// reset the total price field from the shopping cart to 0 when the lists are empty
    /// </summary>
    public void ResetPriceField()
    {
        GameManager.Instance.TotalPrice = 0;
        GameManager.Instance.OrdersTotalPrice = 0;
        GameManager.Instance.UIManagerComponent.CalculateTotalPrice();
    }

    /// <summary>
    /// on the orders history panel, populate the objects from the pool with information about past orders
    /// </summary>
    public void ShowOrdersHistory()
    {
        List<CartItem> selectedClientOrders = GameManager.Instance.OrdersTreeRoot.SearchTree(ClientsSelection.options[ClientsSelection.value].text).Key.OrderedItems;

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

    /// <summary>
    /// reset the order history items, and put them back in the queue for the pool
    /// </summary>
    public void ResetOrdersHistory()
    {
        foreach (var item in UsedOrderList)
        {
            item.AddedToCart = false;
            item.gameObject.SetActive(false);
            GameManager.Instance.OrderHistoryPool.Enqueue(item);
        }
    }

    /// <summary>
    /// set the total price of the items in the shopping cart and orders history to the coresponding UI panel
    /// </summary>
    public void CalculateTotalPrice()
    {
        totalPriceField.text = "Total: " + string.Format("{0:0.00}", GameManager.Instance.TotalPrice) + "$";
        ordersTotalPriceField.text = "Total: " + string.Format("{0:0.00}", GameManager.Instance.OrdersTotalPrice) + "$";
    }

    /// <summary>
    /// function to print an error message to the UI
    /// </summary>
    /// <param name="message"></param>
    public void PrintErrorMessage(string message)
    {
        ErrorMessage.text = message;
    }

    /// <summary>
    /// populate an inventory item from the pool with information from a node, if the pool is empty instantiate a new item.
    /// </summary>
    /// <param name="node"></param>
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

    /// <summary>
    /// refresh the informations of the invenroty items from the list
    /// </summary>
    /// <param name="node"></param>
    public void RefreshNodesList(Node<ItemNode> node)
    {
        if (node != null)
        {
            if (Index <= UsedItemsList.Count - 1)
            {
                UsedItemsList[Index].NodeItem = (ItemNode)(NodeKey)node.Key;
                UsedItemsList[Index].OnEnable();
                Index++;
            }
            else
            {
                InitializeNode((ItemNode)(NodeKey)node.Key);
                Index++;
            }

            if (node.Left != null && Index <= UsedItemsList.Count - 1)
            {
                RefreshNodesList(node.Left);
            }
            else if (node.Left != null && Index > UsedItemsList.Count - 1)
            {
                InitializeNode((ItemNode)(NodeKey)node.Key);
                RefreshNodesList(node.Left);
            }

            if (node.Right != null && Index <= UsedItemsList.Count - 1)
            {
                RefreshNodesList(node.Right);
            }
            else if (node.Right != null && Index > UsedItemsList.Count - 1)
            {
                InitializeNode((ItemNode)(NodeKey)node.Key);
                RefreshNodesList(node.Right);
            }
        }
    }

    /// <summary>
    /// refresh the information about the clients from the dropdown menu
    /// </summary>
    /// <param name="node"></param>
    public void RefreshClientsDropdown(Node<OrderNode> node)
    {
        if (node != null)
        {
            Dropdown.OptionData newClient = new Dropdown.OptionData();
            OrderNode nodeName = (OrderNode)(NodeKey)node.Key;
            newClient.text = nodeName.Name;
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

    /// <summary>
    /// function for searching of a speciffic item in the list to add to the cart
    /// </summary>
    public void SearchItems()
    {
        if (!string.IsNullOrEmpty(searchInput.text))
        {
            foreach (var item in UsedItemsList)
            {
                string itemName = item.NodeItem.Name.Trim().ToLower();
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
    /// Open options panel; set position to mouse click position
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
