using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemsDatabase : MonoBehaviour
{
    public List<Item> ListOfAllItems;

    void Awake()
    {
        GameController.Instance.AllItems = this;
    }

    public Item GetItem(ItemData data)
    {
        return ListOfAllItems.FirstOrDefault(x => x.Data.Name == data.Name);
    }
}
