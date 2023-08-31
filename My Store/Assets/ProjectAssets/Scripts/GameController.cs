using System.Collections;
using System.Collections.Generic;
using Code.Helpers;
using UnityEngine;

public class GameController : MonoSingleton<GameController>
{
    public UiController UiController;
    public Merchant Merchant;
    public PlayerStats PlayerStats;
    public PlayerInventory PlayerInventory;
    public PlayerMovement PlayerMovement;

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
                if (!PlayerInventory.ShopPrompt.activeSelf)
                {
                    PlayerInventory.ShopPrompt.SetActive(true);
                }
            }

            if (PlayerInventory.ShopPrompt.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    UiController.ShopPanel.InitPanel(Merchant.ItemsToSell, PlayerInventory.ItemList, PlayerStats.Coins, ShopPanelType.Buy);
                    PlayerInventory.ShopPrompt.SetActive(false);
                    Merchant.Interactable = false;

                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    UiController.ShopPanel.InitPanel(Merchant.ItemsToSell, PlayerInventory.ItemList, PlayerStats.Coins, ShopPanelType.Sell);
                    PlayerInventory.ShopPrompt.SetActive(false);
                    Merchant.Interactable = false;
                }
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
