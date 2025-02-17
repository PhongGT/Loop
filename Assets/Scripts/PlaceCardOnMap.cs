using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PlaceCardOnMap : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
   [SerializeField] private RectTransform m_RectTransform;
   [SerializeField] private Canvas canvas;
    GameObject target;
    Vector2 basePos;
    
    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
       
        
    }
    private void Start()
    {
        basePos = m_RectTransform.anchoredPosition;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Drag");
    }

    public void OnDrag(PointerEventData eventData)
    {
     
        Debug.Log("InDrag");
        m_RectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor ;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDrag");


        if(target == null)
        {
            m_RectTransform.anchoredPosition = basePos;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Click");
    }


}
