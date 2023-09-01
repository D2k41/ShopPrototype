using System.Collections;
using System.Collections.Generic;
using Code.Helpers;
using UnityEngine;

public class GameController : MonoSingleton<GameController>
{
    public UiController UiController;
    public AudioController AudioController;
    public Merchant Merchant;
    public PlayerStats PlayerStats;
    public PlayerInventory PlayerInventory;
    public PlayerInteraction PlayerInteraction;
    public PlayerMovement PlayerMovement;
    public PlayerEquipment PlayerEquipment;
    public ItemsDatabase AllItems;

    void Awake()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (Merchant.Interactable)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Merchant.PromptE.SetActive(false);
                if (!PlayerInteraction.ShopPrompt.activeSelf)
                {
                    PlayerInteraction.ShopPrompt.SetActive(true);
                }
            }

            if (PlayerInteraction.ShopPrompt.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    UiController.ShopPanel.InitPanel(Merchant.ItemsToSell, PlayerInventory.ItemList, PlayerStats.Coins, PanelType.Buy);
                    PlayerInteraction.ShopPrompt.SetActive(false);
                    Merchant.Interactable = false;

                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    UiController.ShopPanel.InitPanel(Merchant.ItemsToSell, PlayerInventory.ItemList, PlayerStats.Coins, PanelType.Sell);
                    PlayerInteraction.ShopPrompt.SetActive(false);
                    Merchant.Interactable = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!UiController.IsInventoryOpen() && !UiController.IsShopOpen())
            {
                UiController.InventoryPanel.InitPanel(PlayerInventory.ItemList, PlayerStats.Coins);
            }
        }
    }
    public void ClearChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
