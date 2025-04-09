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
    [SerializeField] public List<GameObject> mobObject;
    [SerializeField] protected GameObject mobPrefab;
    [SerializeField] protected List<string> mobList;
    public SpriteRenderer spriteRenderer;
    protected int mobCount;
    

    protected bool isSpawner;

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



    protected void SpawnMob(int a)
    {
        if (a % currentTile.dayNeedToSpawn == 0 || a == 0)
        {
            if (R_Helper.CheckRandom(currentTile.chanceToSpawn))
            {
                List<Cell> shuffledList = R_Helper.shuffle(effecting);

                foreach (var cell in shuffledList)
                {
                    if (cell.mobCount < 3 || cell.currentTile.type == Tile.Type.Road)
                    {
                        Debug.Log("Spawn Mob " + currentTile.mobName);
                        
                        GameObject mob = Instantiate(mobPrefab, cell.transform.position, Quaternion.identity, cell.transform);
                        cell.mobObject.Add(mob);
                        mob.GetComponent<SpriteRenderer>().sprite = currentTile.mobSprite;
                        cell.mobList.Add(currentTile.mobName);
                        cell.mobCount++;
                        break;
                    }
                }
            }

        }
        return;
    }
    public void SetTile(Tile tile)
    {
        currentTile = tile;
        spriteRenderer.sprite = currentTile.sprite;
        if(currentTile.chanceToSpawn != 0)
        {
            Actions.SpawnMob += SpawnMob;
        }
        GetTileEffectedBy();
        AddEffect();
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
        mobObject.Clear();
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
        LoadCell();
    }
    public void ClearMob()
    {
        foreach (var mob in mobObject)
        {
            Destroy(mob);
        }
        mobList.Clear();
        mobObject.Clear();
        mobCount = 0;
    }

    protected void GetTileEffectedBy()
    {
        effecting.Clear();
        if (currentTile.effect == Tile.Effect.None)
        {
            effecting.Add(this);
        }

        if (currentTile.effect == Tile.Effect.UDLR)
        {
            if (CheckValidCell(currentTile.position.x - 1, currentTile.position.y))
            {
                effecting.Add(Map.instance.ReturnCell((int)currentTile.position.x - 1, (int)currentTile.position.y));
            }
            if (CheckValidCell(currentTile.position.x + 1, currentTile.position.y))
            {
                effecting.Add(Map.instance.ReturnCell((int)currentTile.position.x + 1, (int)currentTile.position.y));
            }
            if (CheckValidCell(currentTile.position.x, currentTile.position.y - 1))
            {
                effecting.Add(Map.instance.ReturnCell((int)currentTile.position.x, (int)currentTile.position.y - 1));
            }
            if (CheckValidCell(currentTile.position.x, currentTile.position.y + 1))
            {
                effecting.Add(Map.instance.ReturnCell((int)currentTile.position.x, (int)currentTile.position.y + 1));
            }
        }
        else if (currentTile.effect == Tile.Effect.Ef_3x3)
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

                    effecting.Add(Map.instance.ReturnCell((int)currentTile.position.x + i, (int) currentTile.position.y + j));

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
        if (collision.CompareTag("Player") && mobCount !=0 && currentTile.type == Tile.Type.Road)
        {
            BattleManager.instance.PreperBattle(mobList);
            Actions.StartBattle?.Invoke();
        }
        if (collision.CompareTag("Player") && currentTile.type == Tile.Type.Campfire)
        {
            BattleManager.instance.player.Heal(0.3f);
        }
    }
}
