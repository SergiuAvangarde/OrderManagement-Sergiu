using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartItem : MonoBehaviour
{
    public Node NodeItem { get; set; }
    public int Quantity { get; set; } = 0;

    public bool AddedToCart = false;

    [SerializeField]
    private Text NameField;
    [SerializeField]
    private Text QuantityField;
    [SerializeField]
    private Text PriceField;

    private void OnEnable()
    {
        if (AddedToCart)
        {
            NameField.text = NodeItem.ItemName;
            QuantityField.text = "Quantity: " + Quantity.ToString();
            PriceField.text = "Price: " + NodeItem.Price.ToString() + "$";
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
                GameManager.Instance.TotalPrice += NodeItem.Price;
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
            GameManager.Instance.TotalPrice -= NodeItem.Price;
            GameManager.Instance.UIManagerComponent.CalculateTotalPrice();
            if (Quantity == 0)
            {
                gameObject.SetActive(false);
                AddedToCart = false;
            }
        }
    }
}
