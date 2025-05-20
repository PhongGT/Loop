using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] protected Image imageS_B_S;
    [SerializeField] protected Image imageB_A;

    public Item curItem;
    

    public bool hasItem;
    private void Awake()
    {
        imageS_B_S = transform.GetChild(0).GetComponent<Image>();
        imageB_A = transform.GetChild(1).GetComponent<Image>();
        hasItem = false;
        
    }
    public void SetItem(Item item)
    {
        this.curItem = new Item(item);
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
        hasItem = true;

    }
    public void DeleteItem()
    {
        imageB_A.gameObject.SetActive(false);
        imageS_B_S.gameObject.SetActive(false);
        hasItem = false;
    }
}
