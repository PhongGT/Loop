using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;   

public class OnDropItem : MonoBehaviour, IDropHandler
{

    public enum ItemType
    {
        Sword,
        Bow,
        Staff,
        Armor,
        Book
    }
    public ItemType itemType;
    public Item itemEqiuped;
    public Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        if( eventData.pointerDrag != null) 
        {
            Item item = eventData.pointerDrag.GetComponent<ItemUI>().item;
            if(item.baseItem.itemName.ToString() == itemType.ToString())
            {
                Debug.Log("a");
                BattleManager.instance.player.EquipItem(item, itemEqiuped, itemType.ToString());
                itemEqiuped = item;
                image.sprite = item.GetSprite();
                Color color = image.color;
                color.a = 1f;
                image.color = color;
            }
            

        }
    }


}
