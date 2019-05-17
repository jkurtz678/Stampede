using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuffaloidState;

public class ChaseState : State<Buffaloid>
{
    public float chaseSpeed = 2.2f;
    private float timer;
    private GameObject prey;

    public ChaseState(GameObject prey)
    {
        this.prey = prey;
    }

    public override void EnterState(Buffaloid _owner)
    {
        Debug.Log("Entering Chase State");
        _owner.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;

    }

    public override void ExitState(Buffaloid _owner)
    {
        Debug.Log("Exiting Chase State");

    }

    void chargeCheck(Buffaloid _owner)
    {
        if (_owner.getRBSpeed() > _owner.chargeSpeed)
        {
            _owner.stateMachine.ChangeState(new ChargeState());
        }
    }



    public override void UpdateState(Buffaloid _owner)
    {
        Debug.Log("Chase state stuff...");
        var heading = prey.transform.position - _owner.transform.position;
        _owner.currentMove = heading;
        _owner.moveObject(heading, chaseSpeed);
        chargeCheck(_owner);
    }
}
