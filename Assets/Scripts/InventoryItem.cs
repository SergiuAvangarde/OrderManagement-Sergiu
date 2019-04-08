using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public Node NodeItem { get; set; }

    [SerializeField]
    private Text NameField;
    [SerializeField]
    private Text StockField;
    [SerializeField]
    private Text PriceField;

    private void OnEnable()
    {
        NameField.text = NodeItem.ItemName;
        StockField.text = "Stock: " + NodeItem.Stock.ToString();
        PriceField.text = "Price: " + NodeItem.Price.ToString() + "$";
    }

    public void AddToCart()
    {
        foreach (var item in GameManager.Instance.ShopingCartList)
        {
            if (item.NodeItem.ItemName.ToLower().Trim() == NodeItem.ItemName.ToLower().Trim())
            {
                if (item.NodeItem.Stock > item.Quantity)
                {
                    item.Quantity++;
                    GameManager.Instance.TotalPrice += item.NodeItem.Price;
                    GameManager.Instance.UIManagerComponent.CalculateTotalPrice();
                }
                else
                {
                    GameManager.Instance.UIManagerComponent.PrintErrorMessage(Constants.ERROR_STOCK_EXCEDED);
                }
                return;
            }
        }
        try
        {

            CartItem item = GameManager.Instance.ShopingCartPool.Dequeue();
            item.AddedToCart = true;
            item.NodeItem = NodeItem;
            item.Quantity++;
            item.gameObject.SetActive(true);
            GameManager.Instance.ShopingCartList.Add(item);
            GameManager.Instance.TotalPrice += item.NodeItem.Price;
            GameManager.Instance.UIManagerComponent.CalculateTotalPrice();
        }
        catch
        {
            GameObject listItem = Instantiate(GameManager.Instance.CartItem, GameManager.Instance.ShoppingCartContents);
            CartItem itemValues = listItem.GetComponent<CartItem>();
            GameManager.Instance.ShopingCartPool.Enqueue(itemValues);
            CartItem item = GameManager.Instance.ShopingCartPool.Dequeue();
            item.AddedToCart = true;
            item.NodeItem = NodeItem;
            item.Quantity++;
            item.gameObject.SetActive(true);
            GameManager.Instance.ShopingCartList.Add(item);
            GameManager.Instance.TotalPrice += itemValues.NodeItem.Price;
            GameManager.Instance.UIManagerComponent.CalculateTotalPrice();
        }
    }
}
