using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Item_Drag_Drop : Basic_Drag_Drop
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;
    

    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

    }
    private void Start()
    {
        //basePos = m_RectTransform.localPosition;
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        base.OnEndDrag(eventData);

    }
    public override void OnDrag(PointerEventData eventData)
    {
        m_RectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.3f;
    }
    public void LoadItem(Item item)
    {
        //image.color = new Color(1, 1, 1, 0.5f);
    }

    // Hover UI => Show Item Stats
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
    }

}
