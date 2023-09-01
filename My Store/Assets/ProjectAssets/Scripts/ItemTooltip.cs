using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemTooltip : MonoBehaviour
{
    public TMP_Text Name;
    public TMP_Text Description;
    public TMP_Text Price;
    public GameObject PriceLabel;

    public void InitTooltip(ItemData itemData, PanelType tooltipType, Vector2 position)
    {
        Name.text = itemData.Name;
        Description.text = itemData.Description;
        PriceLabel.SetActive(true);

        if (tooltipType == PanelType.Buy)
        {
            Price.text = itemData.BuyPrice.ToString();
        }
        else if (tooltipType == PanelType.Sell)
        {
            Price.text = itemData.SellPrice.ToString();
        }
        else if (tooltipType == PanelType.Inventory)
        {
            PriceLabel.SetActive(false);
            Price.text = "";
        }

        transform.position = position;
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
