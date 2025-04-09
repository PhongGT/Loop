using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadCharacter : MonoBehaviour
{
    [SerializeField] protected Transform healthBar;
    [SerializeField] private Transform healthPosition;
    Slider healthSlider;
    protected GameObject statusBar;
    [SerializeField] protected GameObject healthBarPrefab;
    public bool isLoaded = false;
    protected Battle battle;

    // Load HealthBar
    public void Load(Character character)
    {
        GameObject a =  Instantiate(character.baseStats.charPrefab, this.transform.position, Quaternion.identity, this.transform);
        a.name = character.baseStats.charPrefab.name;   
        battle = GetComponentInChildren<Battle>();
        battle.currentChar =  character;
        battle.currentChar.animator = GetComponentInChildren<Animator>();
        SpawnHealthBar();
        isLoaded = true;   
        
    }

    protected void SpawnHealthBar()
    {
        Canvas[] canvas = FindObjectsOfType<Canvas>();
        foreach (var item in canvas)
        {
            if (item.renderMode == RenderMode.WorldSpace)
            {
                canvas = new Canvas[1];
                canvas[0] = item;
                break;
            }
        }
        healthBar = Instantiate(healthBarPrefab, canvas[0].transform).transform;
        healthSlider = healthBar.gameObject.GetComponent<Slider>();
        healthBar.gameObject.SetActive(false);

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

    public Character ReturnCharacter => battle.currentChar;

    private void OnEnable()
    {
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(true);
        }
        else return;
    }
    private void OnDisable()
    {
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(false);
        }
        else return;

    }

}
