using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] protected Image imageS_B_S;
    [SerializeField] protected Image imageB_A;

    public Item item;
    public Item item1;

    private void Awake()
    {
        imageS_B_S = transform.GetChild(0).GetComponent<Image>();
        imageB_A = transform.GetChild(1).GetComponent<Image>();
        if(item1.baseItem != null)
        {
            item = item1;
            item.itemRarity = 2;
            item.GenStat(1);
            SetItem(item);
        }
    }
    public void SetItem(Item item)
    {
        this.item = item;
        if (item.baseItem.itemName == BaseItem.ItemName.Sword || item.baseItem.itemName == BaseItem.ItemName.Staff || item.baseItem.itemName == BaseItem.ItemName.Bow)
        {
            imageS_B_S.sprite = item.GetSprite();
            imageS_B_S.gameObject.SetActive(true);
        }
        else
        {
            imageB_A.sprite = item.GetSprite();
            imageB_A.gameObject.SetActive(true);
        }
            
    }
    public void DeleteItem()
    {
        imageB_A.gameObject.SetActive(false);
        imageS_B_S.gameObject.SetActive(false);
    }
}
