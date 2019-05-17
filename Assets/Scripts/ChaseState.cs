using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuffaloidState;

public class ChaseState : State<Buffaloid>
{
    //private static IdleState _instance;
    private float timer;
    private GameObject prey;


    public override void EnterState(Buffaloid _owner)
    {
        Debug.Log("Entering Chase State");
        _owner.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;

    }

    public override void ExitState(Buffaloid _owner)
    {
        Debug.Log("Exiting Chase State");

    }


    public override void UpdateState(Buffaloid _owner)
    {
        Debug.Log("Chase state stuff...");
    }
}
