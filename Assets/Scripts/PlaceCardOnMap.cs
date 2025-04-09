using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
public class PlaceCardOnMap : Basic_Drag_Drop
{
   [SerializeField] private Canvas canvas;
   [SerializeField] Tile currentTile;
   [SerializeField] GameObject cardUI;
   
    [Header("CardUI")]
    protected TMP_Text cardName;
    protected Image image;
    

    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();  
        image = cardUI.GetComponent<Image>();
    }
    private void Start()
    {
        basePos = m_RectTransform.position;
    }
    
    public override void OnDrag(PointerEventData eventData)
    {

        m_RectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        RaycastHit2D ray = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
        if (ray.collider != null)
        {
            target = ray.collider.gameObject;
            Debug.Log(target.name);
            cardUI.SetActive(false);
            //Set preview card
            //cardUI.SetActive(false);
        }
        else
        {
            
        }
           
        
    }
    public override void OnEndDrag (PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        if( target!= null)
        {
            target.GetComponent<Cell>().SetTile(currentTile);
            this.gameObject.SetActive(false);
            return;
        }
    }

    public void SetCard(Tile tile)
    {
        currentTile = tile;
    }

}
