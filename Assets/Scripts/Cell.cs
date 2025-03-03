using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour ,IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    protected Tile currentTile;
    protected GameManager impactRange;
    protected bool isPreview;
    void Start()
    {
        
    }


    void Update()
    {
        
    }
    
    public void SetRoad()
    {

    }
    public void PreviewTile()
    {
       switch(currentTile.effect)
        {
            case Tile.Effect.Adj:
                break;
            case Tile.Effect.R1:
                break;
            case Tile.Effect.R2:
                break;
        }
    }
    public void SetTile()
    {

    }
    public void RemoveTile()
    {

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
