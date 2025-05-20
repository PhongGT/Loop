using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProgressBar : MonoBehaviour
{
    public int maxValue;
    public int currentValue;

    void GetCurrentFill()
    {
        float fillAmount = (float)currentValue / maxValue;
        GetComponent<Image>().fillAmount = fillAmount;
    }
    private void Update()
    {
        GetCurrentFill();
    }
}
