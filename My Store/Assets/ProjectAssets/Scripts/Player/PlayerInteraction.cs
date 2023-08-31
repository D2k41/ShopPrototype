using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject ShopPrompt;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Merchant"))
        {
            
        }
        else if (collider.gameObject.CompareTag("Item"))
        {
            
        }
        else
        {
            Debug.Log($"Entered: {collider.gameObject}");
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
        else
        {
            Debug.Log($"Exited: {collider.gameObject}");
        }
    }
}
