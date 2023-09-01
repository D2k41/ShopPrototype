using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemData
{
    public string Name;
    public string Description;
    public float EffectMultiplier; //Armor, backpack capacity, thirst
    public float Weight; //Affects player speed
    public int SellPrice;
    public int BuyPrice;
}
