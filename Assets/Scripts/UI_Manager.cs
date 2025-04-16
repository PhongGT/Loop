using ScripableObj.Tile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{

    public static UI_Manager instance;
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
    }


    public void AddItemToBag(BaseItem item)
    {
        // Update the bag UI
        // This function should be called whenever an item is added or removed from the bag
        // You can implement the logic to update the UI here
    }

    public void RemoveItemFromBag(ItemUI itemUI)
    {
        // Update the bag UI
        // This function should be called whenever an item is added or removed from the bag
        // You can implement the logic to update the UI here
    }
    public void ShowItemInfo(Item item)
    {
        // Show item info in the UI
        // You can implement the logic to show item info here
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
}
