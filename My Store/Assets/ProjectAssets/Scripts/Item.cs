using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Helpers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[Serializable]
public class Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData Data;
    public ItemType Type;
    public ClothingType ClothingType;
    public string IconName;
    public string PrefabWearableName;

    private GameObject GameObjectWearable;
    private PlayerEquipment playerEquipment;

    void Awake()
    {
        playerEquipment = GameController.Instance.PlayerEquipment;
    }

    public void SetItem(ItemData data, ItemType type, ClothingType clothingType, string icon, string prefabName = "")
    {
        Data = data;
        Type = type;
        ClothingType = clothingType;
        IconName = icon;
        PrefabWearableName = prefabName;
        transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(IconName);
    }

    public void SelectItem()
    {
        if (GameController.Instance.UiController.IsShopOpen())
        {
            GameController.Instance.UiController.ShopPanel.SelectItem(this);
        }
        else if (GameController.Instance.UiController.IsInventoryOpen())
        {
            GameController.Instance.UiController.InventoryPanel.SelectItem(this);
        }
    }

    public void EquipItem()
    {
        playerEquipment = GameController.Instance.PlayerEquipment;

        if (Type == ItemType.Clothing)
        {
            GameController.Instance.PlayerEquipment.UnequipOthersInGroup(ClothingType);

            if (ClothingType == ClothingType.Head)
            {
                if (GameController.Instance.PlayerInventory.ItemList.Count(x => x == Data) > 0)
                {
                    if (playerEquipment.Head == null || playerEquipment.Head != this)
                    {
                        GameObjectWearable = Instantiate(Resources.Load<GameObject>(PrefabWearableName), playerEquipment.ClothingParent);
                        playerEquipment.Head = this;
                        playerEquipment.TotalArmor += Data.EffectMultiplier;
                        playerEquipment.TotalWeight += Data.Weight;
                    }
                    else
                    {
                        Destroy(GameObjectWearable);
                        playerEquipment.Head = null;
                        playerEquipment.TotalArmor -= Data.EffectMultiplier;
                        playerEquipment.TotalWeight -= Data.Weight;
                    }

                    playerEquipment.RegulateStats();
                }
            }
            else if (ClothingType == ClothingType.Body)
            {
                if (GameController.Instance.PlayerInventory.ItemList.Count(x => x.Name == Data.Name) > 0)
                {
                    if (playerEquipment.Body != this)
                    {
                        GameObjectWearable = Instantiate(Resources.Load<GameObject>(PrefabWearableName), playerEquipment.ClothingParent);
                        playerEquipment.Body = this;
                        playerEquipment.TotalArmor += Data.EffectMultiplier;
                        playerEquipment.TotalWeight += Data.Weight;
                    }
                    else
                    {
                        Destroy(GameObjectWearable);
                        playerEquipment.Body = null;
                        playerEquipment.TotalArmor -= Data.EffectMultiplier;
                        playerEquipment.TotalWeight -= Data.Weight;
                    }

                    playerEquipment.RegulateStats();
                }
            }
            else
            {
                Debug.Log("Unknown clothing type");
            }
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        PanelType panelType = PanelType.Inventory;

        if (GameController.Instance.UiController.IsInventoryOpen())
        {
            panelType = PanelType.Inventory;
        }
        else if (GameController.Instance.UiController.IsShopOpen())
        {
            panelType = GameController.Instance.UiController.ShopPanel.ShopType;
        }
        GameController.Instance.UiController.ItemTooltip.InitTooltip(Data, panelType, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameController.Instance.UiController.ItemTooltip.HideTooltip();
    }
}
