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

    public void AddClients()
    {
        OrderNode client = new OrderNode(clientNameInput.text);
        GameManager.Instance.OrdersTreeRoot.RootTree.Left = GameManager.Instance.OrdersTreeRoot.AddToTree(GameManager.Instance.OrdersTreeRoot.RootTree.Left, client);
        Dropdown.OptionData newClient = new Dropdown.OptionData();
        newClient.text = client.ClientName;
        GameManager.Instance.UIManagerComponent.ClientsSelection.options.Add(newClient);
        FileManager.SaveClients(client);
    }

    public void AddItems()
    {
        ItemNode ItemNode = new ItemNode(addItemName.text, float.Parse(addItemPrice.text), int.Parse(addItemStock.text), float.Parse(addItemDiscount.text));
        GameManager.Instance.ItemsTreeRoot.RootTree.Left = GameManager.Instance.ItemsTreeRoot.AddToTree(GameManager.Instance.ItemsTreeRoot.RootTree.Left, ItemNode);
        //GameManager.Instance.UIManagerComponent.InitializeNode(ItemNode);
        GameManager.Instance.RefreshNodes();
    }

    public void EditItem()
    {
        GameManager.Instance.ItemsTreeRoot.EditNode(GameManager.Instance.ItemsTreeRoot.RootTree.Left, SelectedItem.NodeItem.ItemName, EditItemName.text, float.Parse(EditItemPrice.text), int.Parse(EditItemStock.text), float.Parse(EditItemDiscount.text));
        GameManager.Instance.RefreshNodes();
    }

    public void OnRemovePress()
    {
        GameManager.Instance.ItemsTreeRoot.RemoveFromTree(SelectedItem.NodeItem.ItemName);
        SelectedItem.gameObject.SetActive(false);
        GameManager.Instance.UIManagerComponent.ItemsQueue.Enqueue(SelectedItem);
        GameManager.Instance.UIManagerComponent.UsedItemsList.Remove(SelectedItem);
        GameManager.Instance.RefreshNodes();
    }

    public void SendOrder()
    {
        //ResetShoppingCart();
    }
}
