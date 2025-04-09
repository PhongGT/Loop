using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_In_Map : MonoBehaviour
{
    static int roadIndex = 0;
    public enum PlayerState
    {
        Idle,
        Move
    }
    public PlayerState playerState;
    private void Start()
    {
        playerState = PlayerState.Move;
        Actions.StartBattle += ChangeState;
        Actions.EndBattle += ChangeState;
    }
    private void OnEnable()
    {
        gameObject.transform.position = Map.instance.roadCells[roadIndex].transform.position;

    }
    private void OnDisable()
    {
        Actions.StartBattle -= ChangeState;
        Actions.EndBattle -= ChangeState;
    }
    public void Update()
    {
        Move();
    }

    public void Move()
    {
       
        if (playerState == PlayerState.Move)
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, Map.instance.roadCells[roadIndex +1].transform.position, 0.02f);
        if (gameObject.transform.position == Map.instance.roadCells[roadIndex + 1].transform.position)
        {
            roadIndex++;
            if (roadIndex == Map.instance.roadCells.Count-1)
            {
                roadIndex = 0;
            }
        }
    } 

    public void ChangeState()
    {
        if (playerState == PlayerState.Idle)
        {   
            Debug.Log("ChangeState");  
            playerState = PlayerState.Move;
        }
        else
        {
            playerState = PlayerState.Idle;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Campfire")
        {
            BattleManager.instance.loopCount++;
        }
    }



}
