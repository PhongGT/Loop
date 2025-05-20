using ScripableObj.Tile;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{

    public static UI_Manager instance;
    
    Item item;
    [Header("Item Info")]
    protected int newestItemIndex;
    public List<ItemUI> itemUIs;

    [Header("Card Info")]
    public List<GameObject> cardUIs;
    public TMP_Text header;
    public TMP_Text description;

    [Header("Player Info")] 
    protected string playerHeader;
    protected string playerDescription;

    [Header("Btn")]
    public Button mainMenuBtn;
    public Button exitGameBtn;
    public Button returnToPlayBtn;

    [Header("Gameobj")]
    [SerializeField] protected GameObject settingPanel;


    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        newestItemIndex = 0;
        
    }


    
    

    public void AddItemToBag(BaseItem baseItem, int rarity)
    {   
        item = new Item();  
        item.baseItem = baseItem;
        item.itemRarity = rarity;
        item.GenStat(1);        
        SortItemInBag();
        newestItemIndex = Mathf.Clamp(newestItemIndex + 1, 0, 8);
        

        return;
    }

    public void RemoveItemFromBag(ItemUI itemUI)
    {
        itemUI.DeleteItem();
        

    }
    protected void SortItemInBag()
    {
        Debug.Log(newestItemIndex);
        for (int i = newestItemIndex; i >= 0; i--)
        {
            Debug.Log(i);
            if (i != 0)
            {
                Item tempItem = itemUIs[i - 1].curItem;
                itemUIs[i].DeleteItem();
                itemUIs[i].SetItem( tempItem);
            }
            else
            {
                break;
            }
        }
        Debug.Log("Set item Zero");
        itemUIs[0].DeleteItem();
        itemUIs[0].SetItem( item);
    }
    public void ShowItemInfo(Item item)
    {
        if(item == null)
        {
            Debug.LogError("Item is null");
            return;
        }
        header.text = item.baseItem.itemName.ToString();
        
        string newDescription = "";
        newDescription += item.mainStat.statName + ": " + item.mainStat.statValue + "\n";
        foreach (var stat in item.subStats)
        {
            newDescription += stat.statName + ": " + stat.statValue + "\n";
        }
        description.text = newDescription;
        // You can implement the logic to show item info here
    }
    public void ShowTileInfo(Tile tile)
    {
        header.text = tile.type.ToString();
        description.text = tile.tileDescription + "\n" + tile.tileEffectDescription;
    }    

    public void DisableInfo()
    {
        header.text = playerHeader;
        description.text = playerHeader;  
    }
    public void UpdatePlayerInfo()
    {
        if(BattleManager.instance.player == null)
        {
            Debug.LogError("Player is null");
            return;
        }
        playerHeader = "Player Info";
        playerDescription = BattleManager.instance.player.DisplayStats();

    }
    public void AddCard(Tile tile)
    {
        // Add card to the UI
        // You can implement the logic to add card to the UI here
    }
    public void MainMenuBtn()
    {
        // Load the main menu scene
        // You can implement the logic to load the main menu scene here
    }
    public void ExitGame()
    {
        // Exit the game
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
    public void CloseSettingBtn()
    {
        // Return to the play scene
        // You can implement the logic to return to the play scene here
        settingPanel.SetActive(false);
        //Actions.StartTimer();
    }
    public void SettingBtn()
    {
        settingPanel.SetActive(true);
        Actions.PauseTimer();
    }

    private void OnEnable()
    {
        mainMenuBtn.onClick.AddListener(MainMenuBtn);
        exitGameBtn.onClick.AddListener(ExitGame);
        returnToPlayBtn.onClick.AddListener(CloseSettingBtn);
        Actions.StartTimer += UpdatePlayerInfo;
    }
}
