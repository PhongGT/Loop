using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isPause;
    [Header("Resource")]
    public int gold;

    


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
      
        
    }


    public void GameOver()
    {
        // Show Game Over Screen
        Debug.Log("Game Over");
    }

    public void ResetSceen()
    {

    }
    public void UpgradeScene()
    {

    }

}
