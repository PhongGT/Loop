using ScripableObj.Tile;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{

    public static UI_Manager instance;
    public List<ItemUI> itemUIs;
    public Item item = new Item();
    [Header("Item Info")]
    [SerializeField] protected int newestItemIndex;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        newestItemIndex = 0;

    }



    

    public void AddItemToBag(BaseItem baseItem, int rarity)
    {
        foreach (ItemUI itemUI in itemUIs)
        {
            if (!itemUI.hasItem)
            {
                
                item.baseItem = baseItem;
                item.itemRarity = rarity;
                item.GenStat(1);
                itemUI.SetItem(item);
                return;
            }
        }
        RemoveItemFromBag(itemUIs[newestItemIndex]);
        newestItemIndex++;
        if(newestItemIndex >= itemUIs.Count)
        {
            newestItemIndex = 0;
        }

    }

    public void RemoveItemFromBag(ItemUI itemUI)
    {
        itemUI.DeleteItem();
        SortItemInBag();

    }
    public void SortItemInBag()
    {         // Sort the items in the bag
        // You can implement the logic to sort the items here
    }
    public void ShowItemInfo(Item item)
    {
        // Show item info in the UI
        // You can implement the logic to show item info here
    }
    public void ShowTileInfo(Tile tile)
    {

    }    

    public void DisablInfo()
    {
        // Disable item info in the UI
        // You can implement the logic to disable item info here
    }
    public void AddCard(Tile tile)
    {
        // Add card to the UI
        // You can implement the logic to add card to the UI here
    }
    public void Pause()
    {
        // Pause the game
        // You can implement the logic to pause the game here
    }
}
