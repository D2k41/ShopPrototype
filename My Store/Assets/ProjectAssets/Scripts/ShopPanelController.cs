using System.Collections;
using System.Collections.Generic;
using Code.Helpers;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanelController : MonoBehaviour
{
    public Transform MerchantInventoryContent;
    public Transform BackpackContent;
    public Item ItemPrefab;
    public TMP_Text CoinCountText;
    public Button ActionButton;
    public Item SelectedItem;

    public Sprite SelectedItemSprite;
    public Sprite DeselectedItemSprite;

    public void SelectItem(Item item)
    {
        DeselectItems(item.transform.parent);
        SelectedItem = item;
        SelectedItem.GetComponent<Image>().sprite = SelectedItemSprite;
        ActionButton.gameObject.SetActive(true);
    }

    public void DeselectItems(Transform parent)
    {
        foreach (Transform sibling in parent)
        {
            Image imageComponent = sibling.GetComponent<Image>();
            imageComponent.sprite = DeselectedItemSprite;
        }
    }

    public void InitPanel(List<Item> merchantItems, List<Item> playerBackpack, int playerCoins, ShopPanelType panelType)
    {
        if (ActionButton != null)
        {
            ActionButton.onClick.RemoveAllListeners();

            Color buttonColor = Color.clear;

            if (panelType == ShopPanelType.Buy)
            {
                ActionButton.GetComponentInChildren<TMP_Text>().text = "Buy";
                buttonColor = new Color(0, 255, 0, 100);
                ActionButton.GetComponent<Image>().color = buttonColor;
                ActionButton.onClick.AddListener(() =>
                {
                    BuyItem(SelectedItem, GameController.Instance.Merchant.ItemsToSell,
                        GameController.Instance.PlayerInventory.ItemList);
                });
            }
            else
            {
                ActionButton.GetComponentInChildren<TMP_Text>().text = "Sell";
                buttonColor = new Color(255, 0, 0, 100);
                ActionButton.GetComponent<Image>().color = buttonColor;
                ActionButton.onClick.AddListener(() =>
                {
                    SellItem(SelectedItem, GameController.Instance.Merchant.ItemsToSell,
                        GameController.Instance.PlayerInventory.ItemList);
                });
            }
        }
        GameController.Instance.ClearChildren(MerchantInventoryContent);

        foreach (var item in merchantItems)
        {
            GameObject newItem = Instantiate(ItemPrefab.gameObject, MerchantInventoryContent);
            Item newItemComponent = newItem.GetComponent<Item>();
            newItemComponent.SetItem(ObjectCloning.Clone(item.Data), item.Type, item.IconName);
        }

        GameController.Instance.ClearChildren(BackpackContent);

        foreach (var item in playerBackpack)
        {
            GameObject newItem = Instantiate(ItemPrefab.gameObject, BackpackContent);
            Item newItemComponent = newItem.GetComponent<Item>();
            newItemComponent.SetItem(ObjectCloning.Clone(item.Data), item.Type, item.IconName);
        }

        CoinCountText.text = playerCoins.ToString();

        gameObject.SetActive(true);
    }

    private void BuyItem(Item boughtItem, List<Item> merchant, List<Item> player)
    {
        if (boughtItem != null)
        {
            if (GameController.Instance.PlayerStats.Coins >= boughtItem.Data.BuyPrice)
            {
                GameController.Instance.PlayerStats.Coins -= boughtItem.Data.BuyPrice;

                List<Item> newMerchantInventory = merchant;
                newMerchantInventory.Remove(boughtItem);
                GameController.Instance.Merchant.ItemsToSell = newMerchantInventory;

                List<Item> newBackpack = player;
                newBackpack.Add(boughtItem);
                GameController.Instance.PlayerInventory.ItemList = newBackpack;

                InitPanel(newMerchantInventory, newBackpack, GameController.Instance.PlayerStats.Coins, ShopPanelType.Buy);
            }
            else
            {
                Debug.Log("No Coins");
            }
        }
        else
        {
            Debug.Log("No item selected");
        }
    }

    private void SellItem(Item soldItem, List<Item> merchant, List<Item> player)
    {
        if (soldItem != null)
        {
            GameController.Instance.PlayerStats.Coins += soldItem.Data.SellPrice;

            List<Item> newBackpack = player;
            newBackpack.Remove(soldItem);
            GameController.Instance.PlayerInventory.ItemList = newBackpack;

            List<Item> newMerchantInventory = merchant;
            newMerchantInventory.Add(soldItem);
            GameController.Instance.Merchant.ItemsToSell = newMerchantInventory;

            InitPanel(newMerchantInventory, newBackpack, GameController.Instance.PlayerStats.Coins, ShopPanelType.Sell);
        }
        else
        {
            Debug.Log("No item selected");
        }
    }
}
