using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    public GameObject PromptE;
    public bool Interactable = false;
    public List<Item> ItemsToSell;
    private SpriteRenderer spriteRenderer;
    private GameObject player;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            player = collider.gameObject;
            PromptE.SetActive(true);
            Interactable = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            player = null;
            PromptE.SetActive(false);
            GameController.Instance.PlayerInventory.ShopPrompt.SetActive(false);
            Interactable = false;
        }
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            if (player.transform.position.x > transform.position.x && !spriteRenderer.flipX)
            {
                spriteRenderer.flipX = true;
            }
            else if(player.transform.position.x <= transform.position.x)
            {
                spriteRenderer.flipX = false;
            }
        }
    }
}