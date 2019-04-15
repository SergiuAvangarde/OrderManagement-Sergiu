using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    public ItemNode NodeItem { get; set; }

    [SerializeField]
    private TextMeshProUGUI nameField;
    [SerializeField]
    private TextMeshProUGUI stockField;
    [SerializeField]
    private TextMeshProUGUI priceField;
    [SerializeField]
    private TextMeshProUGUI oldPriceField;

    private Toggle toggleButton;

    private void Start()
    {
        toggleButton = GetComponent<Toggle>();
    }

    /// <summary>
    /// Gameobject from Object Pool, when enabled, it sets new values for a different Item
    /// </summary>
    public void OnEnable()
    {
        nameField.text = NodeItem.Name;
        stockField.text = "Stock: " + NodeItem.Stock.ToString();
        if (NodeItem.Discount != 0)
        {
            float price = NodeItem.Price - (NodeItem.Discount/100 * NodeItem.Price);
            priceField.text = "Price: " + price + "$";
            oldPriceField.gameObject.SetActive(true);
            oldPriceField.text = "Old price: " + NodeItem.Price.ToString() + "$";
        }
        else
        {
            oldPriceField.gameObject.SetActive(false);
            priceField.text = "Price: " + NodeItem.Price.ToString() + "$";
        }
    }


    /// <summary>
    /// this function is called when pressing the cart button, tries to enable an object from pool or to instantiate a new one if necessary,
    /// if the item is already in the cart, the quantity is increased
    /// </summary>
    public void AddToCart()
    {
        foreach (CartItem item in GameManager.Instance.ShopingCartList)
        {
            if (item.NodeItem.Name.ToLower().Trim() == NodeItem.Name.ToLower().Trim())
            {
                if (item.NodeItem.Stock > item.Quantity)
                {
                    item.Quantity++;
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
            item.Price = NodeItem.Price;
            item.Discount = NodeItem.Discount;
            item.Quantity++;
            item.gameObject.SetActive(true);
            GameManager.Instance.ShopingCartList.Add(item);
        }
        catch
        {
            GameObject listItem = Instantiate(GameManager.Instance.CartItem, GameManager.Instance.ShoppingCartContents);
            CartItem itemValues = listItem.GetComponent<CartItem>();
            GameManager.Instance.ShopingCartPool.Enqueue(itemValues);
            CartItem item = GameManager.Instance.ShopingCartPool.Dequeue();
            item.AddedToCart = true;
            item.NodeItem = NodeItem;
            item.Price = NodeItem.Price;
            item.Discount = NodeItem.Discount;
            item.Quantity++;
            item.gameObject.SetActive(true);
            GameManager.Instance.ShopingCartList.Add(item);
        }
    }

    /// <summary>
    /// when the toggle of one item is on, it's values are passed to manager script
    /// </summary>
    private void SelectItem()
    {
        GameManager.Instance.ItemsManagerComponent.SelectedItem = this;
        GameManager.Instance.ItemsManagerComponent.EditItemName.text = NodeItem.Name;
        GameManager.Instance.ItemsManagerComponent.EditItemPrice.text = NodeItem.Price.ToString();
        GameManager.Instance.ItemsManagerComponent.EditItemStock.text = NodeItem.Stock.ToString();
        GameManager.Instance.ItemsManagerComponent.EditItemDiscount.text = NodeItem.Discount.ToString();
    }

    /// <summary>
    /// Track right mouse click to open options panel
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (toggleButton.isOn)
        {
            SelectItem();
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                GameManager.Instance.UIManagerComponent.OpenOptionsPanel();
            }
        }
        else
        {
            GameManager.Instance.UIManagerComponent.CloseOptionsPanel();
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                SelectItem();
                toggleButton.isOn = true;
                GameManager.Instance.UIManagerComponent.OpenOptionsPanel();
            }
        }
    }
}
