using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider healthSlider;
    public GameObject healthBarSpawn;
    public GameObject healthBarPrefab;
    private void OnEnable()
    {
        if (healthBarSpawn != null)
        {
            healthBarSpawn.gameObject.SetActive(true);
        }
        else return;
    }
    private void OnDisable()
    {
        if (healthBarSpawn != null)
        {
            healthBarSpawn.gameObject.SetActive(false);
        }
        else return;

    }

    public void SpawnHealthBar(Transform healthBarTransform , float maxHealth)
    {
        healthBarSpawn = Instantiate(healthBarPrefab, healthBarTransform);
        healthSlider = healthBarSpawn.GetComponent<Slider>();
        healthBarSpawn.gameObject.SetActive(false);
        healthSlider.maxValue = maxHealth; // Set max value to 1 for percentage

    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }


}
