using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Basic_Drag_Drop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected RectTransform m_RectTransform;
    public Vector3 basePos;
    protected GameObject target;

    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        basePos = m_RectTransform.localPosition;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {

    }


    public virtual void OnDrag(PointerEventData eventData)
    {
        
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (target == null)
        {
            m_RectTransform.localPosition = Vector3.zero;
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    // Hover UI
    public virtual void OnPointerEnter(PointerEventData eventData)
    {

    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {

    }
}
