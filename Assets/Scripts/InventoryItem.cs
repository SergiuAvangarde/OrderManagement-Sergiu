using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public string ItemName { get; set; }
    public float Price { get; set; }
    public int Stock { get; set; }
    public float SalePrice { get; set; }

    public bool AddedToCart = false;

    [SerializeField]
    private Text NameField;
    [SerializeField]
    private Text StockField;
    [SerializeField]
    private Text PriceField;

    private void Start()
    {
        NameField.text = ItemName;
        StockField.text = "Stock: " + Stock.ToString();
        PriceField.text = "Price: " + Price.ToString() + "$";
    }

    public void AddToCart()
    {
        bool copied = false;
        foreach (CartItem item in GameManager.Instance.ShopingCartList)
        {
            if (item.AddedToCart && item.ItemName.ToLower().Trim() == ItemName.ToLower().Trim())
            {
                item.Quantity += 1;
                copied = true;
                break;
            }
            else if (!item.AddedToCart)
            {
                item.gameObject.SetActive(true);
                item.AddedToCart = true;
                item.ItemName = ItemName;
                item.Price = Price;
                item.Quantity++;
                item.SalePrice = SalePrice;
                copied = true;
                break;
            }
        }

        if (copied == false)
        {
            GameObject listItem = Instantiate(GameManager.Instance.CartItem, GameManager.Instance.ShoppingCartContents);
            CartItem itemValues = listItem.GetComponent<CartItem>();
            GameManager.Instance.ShopingCartList.Add(itemValues);
            itemValues.ItemName = ItemName;
            itemValues.Price = Price;
            itemValues.Quantity++;
            itemValues.SalePrice = SalePrice;
        }
    }
}
