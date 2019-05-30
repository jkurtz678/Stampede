using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuffaloidState;

public class ChargeState : State<Buffaloid>
{

    public float minSpeed = 1f;
    public float chargeSpeed = 3.5f;
    private float timer;
    private Vector2 chargeDir;


    public override void EnterState(Buffaloid _owner)
    {
        //Debug.Log("Entering Charge State");
        _owner.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        timer = 1f;
        chargeDir = _owner.currentMove;
        //WWiseBankManager.Charge(_owner.gameObject);
    }

    public override void ExitState(Buffaloid _owner)
    {
        //Debug.Log("Exiting Charge State");

    }

    public void chargeCheck(Buffaloid _owner)
    {
        if( _owner.getRBSpeed() < minSpeed || timer <= 0f )
        {
            _owner.stateMachine.ChangeState(new IdleState());
        }
    }

    public override void UpdateState(Buffaloid _owner)
    {

        chargeCheck(_owner);

        //Debug.Log("Charge dir: " + chargeDir);
        Debug.DrawRay(_owner.transform.position, chargeDir, Color.yellow);

        timer -= Time.deltaTime;
        _owner.moveObject(chargeDir, chargeSpeed);

    }
}
