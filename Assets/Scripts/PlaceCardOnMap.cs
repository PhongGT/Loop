using System.Collections;
using System.Collections.Generic;
using ScripableObj.Tile;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using static UnityEditor.Progress;
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
            cardUI.SetActive(false);
            //Set preview card
            
        }


    }
    public override void OnEndDrag (PointerEventData eventData)
    {

        if (target != null)
        {
            Cell cellTarget = target.GetComponent<Cell>();
            if (cellTarget.currentTile.IsValid(currentTile))
            {
                this.gameObject.SetActive(false);
                cellTarget.UpdateCell(currentTile);
            }

        }
        else
            {
                cardUI.SetActive(true);
                target = null;
                
            }
        
        base.OnEndDrag(eventData);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        UI_Manager.instance.ShowTileInfo(currentTile);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        UI_Manager.instance.DisableInfo();
    }
    public void SetCard(Tile tile)
    {
        currentTile = tile;
    }
}
