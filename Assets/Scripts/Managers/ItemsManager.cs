using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsManager : MonoBehaviour
{
    [SerializeField]
    private InputField clientNameInput;
    [SerializeField]
    private InputField itemName;
    [SerializeField]
    private InputField itemPrice;
    [SerializeField]
    private InputField itemStock;

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

    public void OnAddPress()
    {
        AddItems(itemName.text, float.Parse(itemPrice.text), int.Parse(itemStock.text));
        GameManager.Instance.RefreshNodes();
    }

    public void OnRemovePress()
    {
        BinaryTree.RootTree = BinaryTree.RemoveFromTree(BinaryTree.RootTree, itemName.text);
        GameManager.Instance.RefreshNodes();
    }

    public void AddItems(string name, float price, int stock)
    {
        Node ItemNode = new Node(name, price, stock);
        BinaryTree.RootTree = BinaryTree.AddToTree(BinaryTree.RootTree, ItemNode);

        GameManager.Instance.UIManagerComponent.InitializeNode(ItemNode);
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
