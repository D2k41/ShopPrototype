using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject ShopPrompt;
    public GameObject InventoryPrompt;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Merchant"))
        {
            
        }
        else if (collider.gameObject.CompareTag("Item"))
        {
            
        }
        else if (collider.gameObject.CompareTag("InventoryTutorial"))
        {
            InventoryPrompt.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Merchant"))
        {
            
        }
        else if (collider.gameObject.CompareTag("Item"))
        {

        }
        else if (collider.gameObject.CompareTag("InventoryTutorial"))
        {
            InventoryPrompt.SetActive(false);
        }
    }
}
