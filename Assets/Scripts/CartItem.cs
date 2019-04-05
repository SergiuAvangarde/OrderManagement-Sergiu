using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartItem : MonoBehaviour
{
    public string ItemName { get; set; }
    public float Price { get; set; }
    public int Quantity { get; set; } = 0;
    public float SalePrice { get; set; }

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
            NameField.text = ItemName;
            QuantityField.text = "Quantity: " + Quantity.ToString();
            PriceField.text = "Price: " + Price.ToString() + "$";
        }
    }

    public void AdjustQuantity(bool adjustment)
    {
        if (adjustment)
        {
            Quantity++;
            QuantityField.text = "Quantity: " + Quantity.ToString();
        }
        else
        {
            Quantity--;
            QuantityField.text = "Quantity: " + Quantity.ToString();
            if(Quantity == 0)
            {
                gameObject.SetActive(false);
                AddedToCart = false;
            }
        }
    }
}
