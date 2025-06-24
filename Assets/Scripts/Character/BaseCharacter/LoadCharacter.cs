using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadCharacter : MonoBehaviour
{

    
    protected GameObject statusBar;
    [SerializeField] protected Transform healthBarTF;
    public bool isLoaded = false;
    protected Battle battle;

    // Load HealthBar
    public void Load(Character character)
    {   
        GameObject a =  Instantiate(character.baseStats.charPrefab, this.transform.position, Quaternion.identity, this.transform);
        a.name = character.baseStats.charPrefab.name;   
        battle = GetComponentInChildren<Battle>();
        battle.currentChar =  character;
        battle.healthBarTF = healthBarTF;
        battle.canAttack = true;
        isLoaded = true;   
        
    }
    public void Clear()
    {
        isLoaded = false;
        this.gameObject.SetActive(false);
    }    




    public Character ReturnCharacter => battle.currentChar;



}
