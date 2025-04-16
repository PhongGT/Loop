using System.Collections;
using System.Collections.Generic;
using ScripableObj.Tile;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardParentManager : MonoBehaviour
{
    [SerializeField] PlaceCardOnMap[] cards;
    private GridLayoutGroup _gridLayoutGroup;
    CardParentManager Instant;
    private void Awake()
    {
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();

        cards = FindObjectsOfType<PlaceCardOnMap>(true);
  

        if (Instant == null)
        {
            Instant = this;
        }
        else
        {
            Destroy(this);
        }
      
    }
    void Start()
    {
        


    }

    public void LoadCard(Tile tile) // Input Card
    {
        foreach (var c in cards)
        {
            if (!c.gameObject.activeInHierarchy)
            {
                c.SetCard(tile);
                c.gameObject.SetActive(true);
                return; 
            }
            
               
        }
    }
    

}
