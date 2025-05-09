using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Slider healthSlider;
    public Transform healthBar;
    public GameObject healthBarPrefab;
    [SerializeField] private Transform healthPosition;


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

    public void SpawnHealthBar()
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

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthSlider != null)
        {
            float healthPercent = currentHealth / maxHealth;
            healthSlider.value = healthPercent;
        }
    }


}
