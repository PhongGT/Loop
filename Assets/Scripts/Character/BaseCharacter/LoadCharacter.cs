using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadCharacter : MonoBehaviour
{
    
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Transform healthBar;
    [SerializeField] private Transform healthPosition;
    Slider healthSlider;
    protected GameObject statusBar;
    [SerializeField] protected GameObject healthBarPrefab;
    public bool isLoaded = false;
    protected Battle battle;

    public void Load(Character character)
    {
        battle = GetComponent<Battle>();
        battle.currentChar = character;
        battle.currentChar.animator = GetComponent<Animator>();
        spriteRenderer.sprite = character.icon;
        this.name = character.name + " (Clone)";
        SpawnHealthBar();
        isLoaded = true;   
        
    }

    protected void SpawnHealthBar()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        healthBar = Instantiate(healthBarPrefab, canvas.transform).transform;
        healthSlider = healthBar.gameObject.GetComponent<Slider>();

    }
    private void LateUpdate()
    {
        if (healthBar != null)
        {
            healthBar.position = healthPosition.position;
        }
    }
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthSlider != null)
        {
            float healthPercent = currentHealth / maxHealth;
            healthSlider.value = healthPercent;
        }
    }
    public void UpdateEnemys(List<Character> targets)
    {
        battle.currentChar.targets = targets;
        
    }

    public Character ReturnCharacter => battle.currentChar;

    
}
