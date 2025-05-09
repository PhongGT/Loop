
using System.Collections.Generic;
using ScripableObj.Tile;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    [SerializeField] public Tile currentTile;
    [SerializeField] public Tile defaultTile;
    [SerializeField] protected Tile roadDefault;
    [SerializeField] public List<string> effectedBy;
    [SerializeField] public List<Cell> effecting;
    [SerializeField] public List<GameObject> mobObject;
    [SerializeField] protected GameObject mobPrefab;
    [SerializeField] protected List<string> mobList;
    private int DayInit;
    public SpriteRenderer spriteRenderer;
    protected int MobCount;
    protected bool IsSpawner;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void PreviewTile()
    {

    }
    public void ShowDescription()
    {

    }



    // ReSharper disable Unity.PerformanceAnalysis
    protected void SpawnMob(int a)
    {
        int Day = a - DayInit;
        if ( Day % currentTile.dayNeedToSpawn == 0 || a == 0)
        {
            if (R_Helper.CheckRandom(currentTile.chanceToSpawn))
            {
                List<Cell> shuffledList = R_Helper.shuffle(effecting);

                foreach (var cell in shuffledList)
                {
                    if (cell.MobCount < 3 || cell.currentTile.type == Tile.Type.Road)
                    {
                        
                        GameObject mob = Instantiate(mobPrefab, cell.transform.position, Quaternion.identity, cell.transform);
                        cell.mobObject.Add(mob);
                        mob.GetComponent<SpriteRenderer>().sprite = currentTile.mobSprite;
                        cell.mobList.Add(currentTile.mobName);
                        cell.MobCount++;
                        break;
                    }
                }
            }

        }
    }
    public void SetTile(Tile tile)
    {

        if(tile.chanceToSpawn != 0 )
        {
            Actions.SpawnMob += SpawnMob;
        }
        currentTile = tile;
        spriteRenderer.sprite = currentTile.sprite;
        DayInit = BattleManager.instance.dayCount;
        GetTileEffectedBy();
        // AddEffect();
    }
    public void AddEffect()
    {
        foreach (var effected in effecting)
        {
            effected.effectedBy.Add(this.currentTile.name);
        }
    }
    public void UpdateCell()
    {
        spriteRenderer.sprite = currentTile.sprite;
    }

    public void UpdateCell(Tile tile)
    {
        currentTile.SetTile(tile);
        
        
    }
    public void RemoveTile()
    {
//Must Fix: Remove Tile and re-apply for all effect Tile
        foreach (string tileName in effectedBy)
        {
            if(tileName == this.currentTile.name)
            {
                effectedBy.Remove(tileName);
                break;
            }
        }
        this.currentTile = defaultTile;
        UpdateCell();
    }
    private void ClearMob()
    {
        foreach (var mob in mobObject)
        {
            Destroy(mob);
        }
        mobList.Clear();
        mobObject.Clear();
        MobCount = 0;
    }

    private void GetTileEffectedBy()
    {
        effecting.Clear();
        if (currentTile.effect == Tile.Effect.None)
        {
            effecting.Add(this);
        }

        if (currentTile.effect == Tile.Effect.Udlr)
        {
            if (CheckValidCell(currentTile.position.x - 1, currentTile.position.y))
            {
                effecting.Add(Map.Instance.ReturnCell((int)currentTile.position.x - 1, (int)currentTile.position.y));
            }
            if (CheckValidCell(currentTile.position.x + 1, currentTile.position.y))
            {
                effecting.Add(Map.Instance.ReturnCell((int)currentTile.position.x + 1, (int)currentTile.position.y));
            }
            if (CheckValidCell(currentTile.position.x, currentTile.position.y - 1))
            {
                effecting.Add(Map.Instance.ReturnCell((int)currentTile.position.x, (int)currentTile.position.y - 1));
            }
            if (CheckValidCell(currentTile.position.x, currentTile.position.y + 1))
            {
                effecting.Add(Map.Instance.ReturnCell((int)currentTile.position.x, (int)currentTile.position.y + 1));
            }
        }
        else if (currentTile.effect == Tile.Effect.Ef3X3)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }
                    if (!CheckValidCell(currentTile.position.x + i, currentTile.position.y + j))
                    { continue; }

                    effecting.Add(Map.Instance.ReturnCell((int)currentTile.position.x + i, (int) currentTile.position.y + j));

                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Show Description

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Hide Description
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
        if (collision.CompareTag("Player") && MobCount !=0 && currentTile.type == Tile.Type.Road)
        {
            BattleManager.instance.PreperBattle(mobList);
            Actions.StartBattle?.Invoke();
            ClearMob();
        }
        if (collision.CompareTag("Player") && currentTile.type == Tile.Type.Campfire)
        {
            BattleManager.instance.player.Heal(0.3f);
        }
    }

    private void OnDisable()
    {
        Actions.SpawnMob -= SpawnMob;
    }

}
