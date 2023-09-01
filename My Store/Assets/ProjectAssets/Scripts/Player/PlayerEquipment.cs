using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public Item Body;
    public Item Head;

    public Transform ClothingParent;

    public float TotalWeight;
    public float TotalArmor;

    void Awake()
    {
        GameController.Instance.PlayerEquipment = this;
    }

    public void RegulateStats()
    {
        float currentWeight = 0;
        float currentArmor = 0;

        if (Head != null)
        {
            currentWeight += Head.Data.Weight;
            currentArmor += Head.Data.EffectMultiplier;
        }

        if (Body != null)
        {
            currentWeight += Body.Data.Weight;
            currentArmor += Body.Data.EffectMultiplier;
        }

        TotalArmor = currentArmor;
        TotalWeight = currentWeight;
    }

    public void UnequipOthersInGroup(string ItemNameToIgnore, ClothingType group)
    {
        foreach (Transform clothing in ClothingParent)
        {
            Item clothingItem = GameController.Instance.AllItems.ListOfAllItems.FirstOrDefault(x =>
                x.PrefabWearableName.Contains(clothing.name.Remove(clothing.name.Length - 7, 7)));

            if (clothingItem != null && clothingItem.Data.Name != ItemNameToIgnore)
            {
                if (clothingItem.ClothingType == group)
                {
                    Debug.Log($"Destroying: {clothing.name}");
                    Destroy(clothing.gameObject);
                }
            }
        }
    }

    public GameObject GetClothingItem(string itemName)
    {
        GameObject result = null;

        foreach (Transform item in ClothingParent)
        {
            if (item.name.Contains(itemName))
            {
                result = item.gameObject;
            }
        }

        return result;
    }
}
