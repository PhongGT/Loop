using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
public class PlaceCardOnMap : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
   [SerializeField] private RectTransform m_RectTransform;
   [SerializeField] private Canvas canvas;
   [SerializeField] Vector3 basePos;
   [SerializeField] private GameObject target;
   [SerializeField] Tile currentTile;
    [SerializeField] GameObject cardUI;
    


    [Header("CardUI")]

    protected TMP_Text cardName;
    protected Image image;
    

    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
        Debug.Log(m_RectTransform.position.x + "" + m_RectTransform.position.y  );

      
    }
    private void Start()
    {
        basePos = m_RectTransform.position;
    }
    
        
    public void OnBeginDrag(PointerEventData eventData)
    {
        
        Debug.Log("Drag");
    }
    

    public void OnDrag(PointerEventData eventData)
    {
        m_RectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        RaycastHit2D ray = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
        if (ray.collider != null)
        {
            target = ray.collider.gameObject;
            Debug.Log("Hit");
            //Set preview card
            cardUI.SetActive(false);


        }
        else
        {
            
        }
           
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDrag");


        if(target == null)
        {
            m_RectTransform.position = basePos;
        }
        else
        {

            return;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Click");
    }

    public void SetCard(Tile tile)
    {
        currentTile = tile;
    }


    // Hover UI
    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
