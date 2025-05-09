
using System.Collections.Generic;
using ScripableObj.Tile;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
public class Drop_Manager : MonoBehaviour
{
    [SerializeField] private Tile[] listCardTiles;
    [SerializeField] private BaseItem[] listItems;
    // List 
    public static Drop_Manager instance;
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
    public enum itemRarity
    {
        Common,
        Rare,
        Epic,
        Legendary,
    }


    private void Start()
    {
        listCardTiles = Resources.LoadAll<Tile>("ScriptableObj/Tile");
        listItems = Resources.LoadAll<BaseItem>("ScriptableObj/Item");

    }
    private BaseItem Drop_Item()
    {
        int index = Random.Range(0, listItems.Length);
        return listItems[index];
    }

    private Tile Drop_Tile()
    {
        int index = Random.Range(0, listCardTiles.Length);
        return listCardTiles[index];
    }

    private int ItemRarity()
    {
        int index = Random.Range(0, 100);
        if (index < 50)
        {
            return (int)itemRarity.Common;
        }
        else if (index < 75)
        {
            return (int)itemRarity.Rare;
        }
        else if (index < 90)
        {
            return (int)itemRarity.Epic;
        }
        else
        {
            return (int)itemRarity.Legendary;
        }
    }
    [ContextMenu("Increase Fun Level")]
    public void DropOnEnemyDead()
    {
        //if(R_Helper.CheckRandom(ChanceToDropItem))
        //{
              UI_Manager.instance.AddItemToBag(Drop_Item(), ItemRarity());
        //}
        //else
        //{
        //    UI_Manager.instance.AddCard(Drop_Tile());
        //}

    }
}
