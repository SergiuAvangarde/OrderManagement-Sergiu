using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsManager : MonoBehaviour
{
    public InventoryItem SelectedItem;
    public InputField EditItemName;
    public InputField EditItemPrice;
    public InputField EditItemStock;
    public InputField EditItemDiscount;

    [SerializeField]
    private InputField clientNameInput;
    [SerializeField]
    private InputField AddItemName;
    [SerializeField]
    private InputField AddItemPrice;
    [SerializeField]
    private InputField AddItemStock;
    [SerializeField]
    private InputField AddItemDiscount;

    public void AddClients()
    {
        bool exists = false;
        foreach (Clients clientName in GameManager.Instance.ClientsList)
        {
            if (clientName.ClientName.ToLower().Trim() == clientNameInput.text.ToLower().Trim())
            {
                exists = true;
                GameManager.Instance.UIManagerComponent.PrintErrorMessage(Constants.ERROR_CLIENT_EXISTS);
            }
        }

        if (!exists)
        {
            Clients client = new Clients(clientNameInput.text);
            GameManager.Instance.ClientsList.Add(client);
            FileManager.SaveClients(GameManager.Instance.ClientsList);
        }
    }

    public void AddItems()
    {
        Node ItemNode = new Node(AddItemName.text, float.Parse(AddItemPrice.text), int.Parse(AddItemStock.text), float.Parse(AddItemDiscount.text));
        BinaryTree.RootTree = BinaryTree.AddToTree(BinaryTree.RootTree, ItemNode);
        //GameManager.Instance.UIManagerComponent.InitializeNode(ItemNode);
        GameManager.Instance.RefreshNodes();
    }

    public void EditItem()
    {
        BinaryTree.EditNode(BinaryTree.RootTree, SelectedItem.NodeItem.ItemName, EditItemName.text, float.Parse(EditItemPrice.text), int.Parse(EditItemStock.text), float.Parse(EditItemDiscount.text));
        GameManager.Instance.RefreshNodes();
    }

    public void OnRemovePress()
    {
        BinaryTree.RemoveFromTree(SelectedItem.NodeItem.ItemName);
        SelectedItem.gameObject.SetActive(false);
        GameManager.Instance.UIManagerComponent.ItemsQueue.Enqueue(SelectedItem);
        GameManager.Instance.UIManagerComponent.UsedItemsList.Remove(SelectedItem);
        GameManager.Instance.RefreshNodes();
    }

    public void ResetShoppingCart()
    {
        foreach (var item in GameManager.Instance.ShopingCartList)
        {
            GameManager.Instance.UIManagerComponent.CalculateTotalPrice();
            item.AddedToCart = false;
            item.gameObject.SetActive(false);
        }
    }

    public void SendOrder()
    {
        Orders CurentOrder = new Orders(GameManager.Instance.ShopingCartList);
        ResetShoppingCart();
    }
}
