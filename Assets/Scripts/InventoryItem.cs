using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public string ItemName;
    public float Price;
    public int Stock;
    public bool OnSale;

    [SerializeField]
    private Text NameField;
    [SerializeField]
    private Text StockField;
    [SerializeField]
    private Text PriceField;

    //public InventoryItem(string name, float price, int stock)
    //{
    //    ItemName = name;
    //    Price = price;
    //    Stock = stock;
    //}

    private void Start()
    {
        NameField.text = ItemName;
        StockField.text = "Stock: " + Stock.ToString();
        PriceField.text = "Price: " + Price.ToString() + "$";
    }
}
