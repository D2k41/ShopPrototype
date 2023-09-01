using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiController : MonoBehaviour
{
    public ShopPanelController ShopPanel;
    public PlayerInventory InventoryPanel;
    public ItemTooltip ItemTooltip;

    public bool IsShopOpen()
    {
        return ShopPanel.gameObject.activeSelf;
    }
    public bool IsInventoryOpen()
    {
        return InventoryPanel.gameObject.activeSelf;
    }
}
