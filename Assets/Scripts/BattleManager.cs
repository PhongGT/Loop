using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    protected List<Character> enemys;
    protected List<Character> player_allys;
    public float dayTime = 10f;
    public int loopCount = 1;
    public bool isNewDay = false;
    public Player player;
    

    public static BattleManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartBattle()
    {
        //StartCoroutine(StartBattleCoroutine());
    }
    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }
    public Character ReturnRandomTarget(Character[] targets)
    {
        int index = Random.Range(0, targets.Length);
        return targets[index];
    }
}
