using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardParentManager : MonoBehaviour
{
    [SerializeField] PlaceCardOnMap[] cards;
    HorizontalLayoutGroup horizontal;
    CardParentManager Instant;
    private void Awake()
    {
        horizontal = GetComponent<HorizontalLayoutGroup>();

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
        horizontal.enabled = true;


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
