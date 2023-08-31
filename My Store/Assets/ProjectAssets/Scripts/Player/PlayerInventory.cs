using System.Collections;
using System.Collections.Generic;
using Code.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public List<ItemData> ItemList;
    public int ItemCountLimit;

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

    public void InitPanel(List<ItemData> playerBackpack, int playerCoins)
    {
        if (ActionButton != null)
        {
            ActionButton.onClick.RemoveAllListeners();

            Color buttonColor = Color.clear;

            ActionButton.GetComponentInChildren<TMP_Text>().text = "Use";
            buttonColor = new Color(0, 255, 0, 100);
            ActionButton.GetComponent<Image>().color = buttonColor;
            ActionButton.onClick.AddListener(() =>
            {
                if (SelectedItem != null)
                {
                    SelectedItem.EquipItem();
                    DeselectItems(SelectedItem.transform.parent);
                }
            });
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

}
