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
    public GameObject MerchantItemsBlocker;
    public GameObject BackpackItemsBlocker;
    public Item ItemPrefab;
    public TMP_Text CoinCountText;
    public Button ActionButton;
    public ItemData SelectedItem;
    public PanelType ShopType;

    public Sprite SelectedItemSprite;
    public Sprite DeselectedItemSprite;

    public void SelectItem(Item item)
    {
        DeselectItems(item.transform.parent);
        SelectedItem = item.Data;
        item.GetComponent<Image>().sprite = SelectedItemSprite;
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

    public void InitPanel(List<ItemData> merchantItems, List<ItemData> playerBackpack, int playerCoins, PanelType panelType)
    {
        ShopType = panelType;
        MerchantItemsBlocker.SetActive(false);
        BackpackItemsBlocker.SetActive(false);

        if (ActionButton != null)
        {
            ActionButton.onClick.RemoveAllListeners();

            Color buttonColor = Color.clear;

            if (panelType == PanelType.Buy)
            {
                BackpackItemsBlocker.SetActive(true);
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
                MerchantItemsBlocker.SetActive(true);
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
            Item ItemFromData = GameController.Instance.AllItems.GetItem(item);
            if (ItemFromData != null)
            {
                newItemComponent.SetItem(ObjectCloning.Clone(ItemFromData.Data), ItemFromData.Type, ItemFromData.ClothingType, ItemFromData.IconName, ItemFromData.PrefabWearableName);
            }
        }

        GameController.Instance.ClearChildren(BackpackContent);

        foreach (var item in playerBackpack)
        {
            GameObject newItem = Instantiate(ItemPrefab.gameObject, BackpackContent);
            Item newItemComponent = newItem.GetComponent<Item>();
            Item ItemFromData = GameController.Instance.AllItems.GetItem(item);
            if (ItemFromData != null)
            {
                newItemComponent.SetItem(ObjectCloning.Clone(ItemFromData.Data), ItemFromData.Type, ItemFromData.ClothingType, ItemFromData.IconName, ItemFromData.PrefabWearableName);
            }
        }

        CoinCountText.text = playerCoins.ToString();

        gameObject.SetActive(true);
    }

    private void BuyItem(ItemData boughtItem, List<ItemData> merchant, List<ItemData> player)
    {
        ItemData itemToBuy = GameController.Instance.AllItems.GetItem(boughtItem).Data;

        if (itemToBuy != null)
        {
            if (GameController.Instance.PlayerStats.Coins >= itemToBuy.BuyPrice)
            {
                if (GameController.Instance.PlayerInventory.ItemCountLimit < player.Count + 1)
                {
                    return;
                }

                GameController.Instance.PlayerStats.Coins -= itemToBuy.BuyPrice;

                List<ItemData> newMerchantInventory = new List<ItemData>(merchant);
                newMerchantInventory.Remove(itemToBuy);
                GameController.Instance.Merchant.ItemsToSell = new List<ItemData>(newMerchantInventory);

                List<ItemData> newBackpack = new List<ItemData>(player);
                newBackpack.Add(itemToBuy);
                GameController.Instance.PlayerInventory.ItemList = new List<ItemData>(newBackpack);

                Item itemFromData = GameController.Instance.AllItems.GetItem(itemToBuy);
                if (itemFromData != null)
                {
                    itemFromData.EquipItem();
                }

                InitPanel(newMerchantInventory, newBackpack, GameController.Instance.PlayerStats.Coins, PanelType.Buy);
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

    private void SellItem(ItemData soldItem, List<ItemData> merchant, List<ItemData> player)
    {
        Item item = GameController.Instance.AllItems.GetItem(soldItem);
        ItemData itemToSell = item.Data;

        if (itemToSell != null)
        {
            if (item.Type == ItemType.Clothing)
            {
                string itemObjectName = item.PrefabWearableName.Split("/")[1];
                GameObject clothingItem = GameController.Instance.PlayerEquipment.GetClothingItem(itemObjectName);
                if (clothingItem != null)
                {
                    Destroy(clothingItem);
                }
            }

            GameController.Instance.PlayerStats.Coins += itemToSell.SellPrice;

            List<ItemData> newBackpack = new List<ItemData>(player);
            newBackpack.Remove(itemToSell);
            GameController.Instance.PlayerInventory.ItemList = new List<ItemData>(newBackpack);

            List<ItemData> newMerchantInventory = new List<ItemData>(merchant);
            newMerchantInventory.Add(itemToSell);
            GameController.Instance.Merchant.ItemsToSell = new List<ItemData>(newMerchantInventory);

            InitPanel(newMerchantInventory, newBackpack, GameController.Instance.PlayerStats.Coins, PanelType.Sell);
        }
        else
        {
            Debug.Log("No item selected");
        }
    }
}
