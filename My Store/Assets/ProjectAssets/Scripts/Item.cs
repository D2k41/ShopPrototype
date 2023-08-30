using System;
using System.Collections;
using System.Collections.Generic;
using Code.Helpers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[Serializable]
public class Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData Data;
    public ItemType Type;
    public string IconName;

    public void SetItem(ItemData data, ItemType type, string icon)
    {
        Data = data;
        Type = type;
        IconName = icon;
        transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(IconName);
    }

    public void SelectItem()
    {
        GameController.Instance.UiController.ShopPanel.SelectItem(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }
}
