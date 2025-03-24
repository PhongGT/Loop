using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEditor.Experimental.GraphView;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    [SerializeField] protected Tile currentTile;
    [SerializeField] public Tile defaultTile;
    [SerializeField] protected Tile roadDefault;
    [SerializeField] public List<string> effectedBy;
    [SerializeField] public List<Cell> effecting;
    [SerializeField] public List<string> mob;
    [SerializeField] protected GameObject mobPrefab;
    [SerializeField] protected List<GameObject> mobList;
    public SpriteRenderer spriteRenderer;
    protected int mobCount;
    

    protected bool isSpawner;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {

    }
    private void Update()
    {
        //Check if tile can spawn => spawn mob 


    }

    public void PreviewTile()
    {

    }
    public void ShowDescription()
    {

    }
    protected void SpawnMob()
    {
        List<Cell> shuffledList = R_Helper.shuffle(effecting);

        foreach (var cell in shuffledList)
        {
            if (cell.mobCount < 3)
            {
                
                cell.mob.Add(currentTile.nameMob);
                GameObject mob =  Instantiate(mobPrefab, cell.transform.position, Quaternion.identity);
                mobList.Add(mob);
                cell.mobCount++;
                break;
            }
        }




    }
    public void SetTile(Tile tile)
    {
        currentTile = tile;
        GetTileEffectedBy();
    }
    public void AddEffect()
    {
        foreach (var effected in effecting)
        {
            effected.effectedBy.Add(this.currentTile.name);
        }
    }
    public void LoadCell()
    {
        spriteRenderer.sprite = currentTile.sprite;
        mob.Clear();
        mobCount = 0;

    }
    public void RemoveTile()
    {

        foreach (var name in effectedBy)
        {
            if(name == this.currentTile.name)
            {
                effectedBy.Remove(name);
                break;
            }
        }
        this.currentTile = defaultTile;
    }

    protected void GetTileEffectedBy()
    {
        effecting = new List<Cell>();
        if (currentTile.effect == Tile.Effect.None)
        {
            effecting.Add(this);
        }

        if (currentTile.effect == Tile.Effect.UDLR)
        {
            if (CheckValidCell(currentTile.position.x - 1, currentTile.position.y))
            {
                effecting.Add(Map.instance.ReturnCell(ReturnPos(currentTile.position.x - 1, currentTile.position.y)));
            }
            if (CheckValidCell(currentTile.position.x + 1, currentTile.position.y))
            {
                effecting.Add(Map.instance.ReturnCell(ReturnPos(currentTile.position.x + 1, currentTile.position.y)));
            }
            if (CheckValidCell(currentTile.position.x, currentTile.position.y - 1))
            {
                effecting.Add(Map.instance.ReturnCell(ReturnPos(currentTile.position.x, currentTile.position.y - 1)));
            }
            if (CheckValidCell(currentTile.position.x, currentTile.position.y + 1))
            {
                effecting.Add(Map.instance.ReturnCell(ReturnPos(currentTile.position.x, currentTile.position.y + 1)));
            }
        }
        else if (currentTile.effect == Tile.Effect.Ef_3x3)
        {
            for (float i = -1; i <= 1; i++)
            {
                for (float j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }
                    if (!CheckValidCell(currentTile.position.x + i, currentTile.position.y + j))
                    { continue; }

                    effecting.Add(Map.instance.ReturnCell(ReturnPos((i + currentTile.position.x), j + (currentTile.position.y))));

                }
            }
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }


    int ReturnPos(float x, float y)
    {

        if (x == 0 && y == 0)
        {
            return 0;
        }
        else if (x == 0 && y > 0)
        {
            return (int)y;
        }
        else
            return (int)x * 17 + (int)y;

    }

    bool CheckValidCell(float x, float y)
    {
        if (x < 0 || x > 16 || y < 0 || y > 11)
        {
            return false;
        }
        return true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && mobCount !=0)
        {
            BattleManager.instance.StartBattle();
        }
    }
}
