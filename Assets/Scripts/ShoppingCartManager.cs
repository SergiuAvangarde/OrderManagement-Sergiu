using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingCartManager : MonoBehaviour
{
    public List<InventoryItem> ShopingCartList = new List<InventoryItem>();
    public GameObject CartItem;
    public Transform ShoppingCartContents;

    public Text totalPrice;

    public void ResetShoppingCart()
    {

    }

    public void SendOrder()
    {

    }
}
