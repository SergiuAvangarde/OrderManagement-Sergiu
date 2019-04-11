using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CartItem : MonoBehaviour
{
    public ItemNode NodeItem { get; set; }
    public float Price { get; set; }
    public int Quantity { get; set; } = 0;

    public bool AddedToCart = false;

    [SerializeField]
    private TextMeshProUGUI NameField;
    [SerializeField]
    private TextMeshProUGUI QuantityField;
    [SerializeField]
    private TextMeshProUGUI PriceField;
    [SerializeField]
    private TextMeshProUGUI OldPriceField;

    private void OnEnable()
    {
        if (AddedToCart)
        {
            NameField.text = NodeItem.ItemName;
            QuantityField.text = "Quantity: " + Quantity.ToString();
            if (NodeItem.Discount != 0)
            {
                OldPriceField.text = "Old price: " + NodeItem.Price.ToString() + "$";
                Price = NodeItem.Price - (NodeItem.Discount / 100 * NodeItem.Price);
                PriceField.text = "Price: " + Price + "$";
                OldPriceField.gameObject.SetActive(true);
            }
            else
            {
                OldPriceField.gameObject.SetActive(false);
                Price = NodeItem.Price;
                PriceField.text = "Price: " + Price + "$";
            }
            GameManager.Instance.UIManagerComponent.CalculateTotalPrice();
        }
    }

    public void AdjustQuantity(bool adjustment)
    {
        if (adjustment)
        {
            if (NodeItem.Stock > Quantity)
            {
                Quantity++;
                QuantityField.text = "Quantity: " + Quantity.ToString();
                GameManager.Instance.TotalPrice += Price;
                GameManager.Instance.UIManagerComponent.CalculateTotalPrice();
            }
            else
            {
                GameManager.Instance.UIManagerComponent.PrintErrorMessage(Constants.ERROR_STOCK_EXCEDED);
            }
        }
        else
        {
            Quantity--;
            QuantityField.text = "Quantity: " + Quantity.ToString();
            GameManager.Instance.TotalPrice -= Price;
            GameManager.Instance.UIManagerComponent.CalculateTotalPrice();
            if (Quantity == 0)
            {
                gameObject.SetActive(false);
                AddedToCart = false;
                GameManager.Instance.ShopingCartPool.Enqueue(this);
            }
        }
    }
}
