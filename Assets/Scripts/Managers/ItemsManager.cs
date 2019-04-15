using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsManager : MonoBehaviour
{
    public InventoryItem SelectedItem { get; set; }
    public InputField EditItemName;
    public InputField EditItemPrice;
    public InputField EditItemStock;
    public InputField EditItemDiscount;

    [SerializeField]
    private InputField clientNameInput;
    [SerializeField]
    private InputField addItemName;
    [SerializeField]
    private InputField addItemPrice;
    [SerializeField]
    private InputField addItemStock;
    [SerializeField]
    private InputField addItemDiscount;

    /// <summary>
    /// add the client from the input fields to the binary tree
    /// </summary>
    public void AddClients()
    {
        OrderNode client = new OrderNode(clientNameInput.text);
        Node<OrderNode> clientNode = new Node<OrderNode>(client);
        GameManager.Instance.OrdersTreeRoot.RootTree.Left = GameManager.Instance.OrdersTreeRoot.AddToTree(GameManager.Instance.OrdersTreeRoot.RootTree.Left, clientNode);
        Dropdown.OptionData newClient = new Dropdown.OptionData();
        newClient.text = client.Name;
        GameManager.Instance.UIManagerComponent.ClientsSelection.options.Add(newClient);
        GameManager.Instance.RefreshNodes();
        GameManager.Instance.UIManagerComponent.ClientsSelection.value = 0;
        clientNameInput.text = "";
    }

    /// <summary>
    /// add a new item to the binary tree
    /// </summary>
    public void AddItems()
    {
        ItemNode nodeItem = new ItemNode(addItemName.text, float.Parse(addItemPrice.text), int.Parse(addItemStock.text), float.Parse(addItemDiscount.text));
        Node<ItemNode> itemNode = new Node<ItemNode>(nodeItem);
        GameManager.Instance.ItemsTreeRoot.RootTree.Left = GameManager.Instance.ItemsTreeRoot.AddToTree(GameManager.Instance.ItemsTreeRoot.RootTree.Left, itemNode);
        GameManager.Instance.RefreshNodes();
        addItemName.text = "";
        addItemPrice.text = "";
        addItemStock.text = "";
        addItemDiscount.text = "";
    }

    /// <summary>
    /// edit the item information from the binary tree, copy the current node, edit the information then replace the node with the new one
    /// </summary>
    public void EditItem()
    {
        //GameManager.Instance.ItemsTreeRoot.EditItem(GameManager.Instance.ItemsTreeRoot.RootTree.Left, SelectedItem.NodeItem.Key, EditItemName.text, float.Parse(EditItemPrice.text), int.Parse(EditItemStock.text), float.Parse(EditItemDiscount.text));
        Node<ItemNode> itemRef = GameManager.Instance.ItemsTreeRoot.SearchTree(SelectedItem.NodeItem.Name);
        itemRef.Key.Name = EditItemName.text;
        itemRef.Key.Price = float.Parse(EditItemPrice.text);
        itemRef.Key.Stock = int.Parse(EditItemStock.text);
        itemRef.Key.Discount = float.Parse(EditItemDiscount.text);
        GameManager.Instance.ItemsTreeRoot.EditNode(GameManager.Instance.ItemsTreeRoot.RootTree.Left, itemRef, SelectedItem.NodeItem.Name);
        GameManager.Instance.RefreshNodes();
        EditItemName.text = "";
        EditItemPrice.text = "";
        EditItemStock.text = "";
        EditItemDiscount.text = "";
    }

    /// <summary>
    /// remoave an item from from the list, and from the binary tree
    /// </summary>
    public void OnRemovePress()
    {
        GameManager.Instance.ItemsTreeRoot.RemoveFromTree(SelectedItem.NodeItem.Name);
        SelectedItem.gameObject.SetActive(false);
        GameManager.Instance.UIManagerComponent.ItemsQueue.Enqueue(SelectedItem);
        GameManager.Instance.UIManagerComponent.UsedItemsList.Remove(SelectedItem);
        GameManager.Instance.RefreshNodes();
    }

    /// <summary>
    /// send the order to the order history
    /// </summary>
    public void SendOrder()
    {
        string name = GameManager.Instance.UIManagerComponent.ClientsSelection.options[GameManager.Instance.UIManagerComponent.ClientsSelection.value].text;
        foreach (CartItem item in GameManager.Instance.ShopingCartList)
        {
            Node<OrderNode> orderRef = GameManager.Instance.OrdersTreeRoot.SearchTree(name);
            orderRef.Key.Name = name;

            bool added = false;
            foreach(var orderItem in orderRef.Key.OrderedItems)
            {
                if(orderItem.NodeItem.Name == item.NodeItem.Name)
                {
                    orderItem.Quantity += item.Quantity;
                    added = true;
                    break;
                }
            }
            if (!added)
            {
                orderRef.Key.OrderedItems.Add(item);
            }
            GameManager.Instance.OrdersTreeRoot.EditNode(GameManager.Instance.OrdersTreeRoot.RootTree.Left, orderRef, name);

            Node<ItemNode> itemRef = GameManager.Instance.ItemsTreeRoot.SearchTree(item.NodeItem.Name);
            itemRef.Key.Name = item.NodeItem.Name;
            itemRef.Key.Price = item.NodeItem.Price;
            itemRef.Key.Stock = item.NodeItem.Stock - item.Quantity;
            itemRef.Key.Discount = item.NodeItem.Discount;
            GameManager.Instance.ItemsTreeRoot.EditNode(GameManager.Instance.ItemsTreeRoot.RootTree.Left, itemRef, item.NodeItem.Name);

            item.gameObject.SetActive(false);
            item.AddedToCart = false;
            GameManager.Instance.ShopingCartPool.Enqueue(item);
        }
        GameManager.Instance.ShopingCartList.Clear();
        GameManager.Instance.TotalPrice = 0;
        GameManager.Instance.RefreshNodes();
    }
}
