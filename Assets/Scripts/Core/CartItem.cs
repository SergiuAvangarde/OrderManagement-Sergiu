using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CartItem : MonoBehaviour
{
    public ItemNode NodeItem { get; set; }
    public float Price { get; set; }
    public float Discount { get; set; }
    public int Quantity { get; set; } = 0;
    public bool AddedToCart { get; set; } = false;

    [SerializeField]
    private TextMeshProUGUI nameField;
    [SerializeField]
    private TextMeshProUGUI quantityField;
    [SerializeField]
    private TextMeshProUGUI priceField;
    [SerializeField]
    private TextMeshProUGUI oldPriceField;

    /// <summary>
    /// when enabled, the object from the cart or the object from the Orders list, is refreshed with the coresponding information
    /// </summary>
    private void OnEnable()
    {
        if (AddedToCart)
        {
            nameField.text = NodeItem.Name;
            quantityField.text = "Quantity: " + Quantity.ToString();
            if (Discount != 0)
            {
                oldPriceField.text = "Old price: " + NodeItem.Price.ToString() + "$";
                Price -= Discount / 100 * Price;
                priceField.text = "Price: " + Price + "$";
                GameManager.Instance.TotalPrice += Price * Quantity;
                GameManager.Instance.UIManagerComponent.CalculateTotalPrice();
                oldPriceField.gameObject.SetActive(true);
            }
            else
            {
                oldPriceField.gameObject.SetActive(false);
                priceField.text = "Price: " + Price + "$";
                GameManager.Instance.TotalPrice += Price * Quantity;
                GameManager.Instance.UIManagerComponent.CalculateTotalPrice();
            }

        }
    }


    /// <summary>
    /// this function adjusts the quantity of this item in the cart, also if the quantity reaches 0, the object is removed from the cart
    /// </summary>
    /// <param name="if true, increase, if false, decrease"></param>
    public void AdjustQuantity(bool adjustment)
    {
        if (adjustment)
        {
            if (NodeItem.Stock > Quantity)
            {
                Quantity++;
                quantityField.text = "Quantity: " + Quantity.ToString();
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
            quantityField.text = "Quantity: " + Quantity.ToString();
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
