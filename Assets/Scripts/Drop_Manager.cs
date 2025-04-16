
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
    

    public void Start()
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

    public void DropOnEnemyDead(int ChanceToDropItem)
    {
        if(R_Helper.CheckRandom(ChanceToDropItem))
        {
              UI_Manager.instance.AddItemToBag(Drop_Item());
        }
        else
        {
            UI_Manager.instance.AddCard(Drop_Tile());
        }

    }
}
