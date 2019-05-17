using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuffaloidState;

public class ChargeState : State<Buffaloid>
{
    private float timer;
    private Vector2 chargeDir;


    public override void EnterState(Buffaloid _owner)
    {
        Debug.Log("Entering Charge State");
        _owner.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        timer = 4f;
        chargeDir = _owner.currentMove;

    }

    public override void ExitState(Buffaloid _owner)
    {
        Debug.Log("Exiting Charge State");

    }

    public void chargeCheck(Buffaloid _owner)
    {
        if( _owner.getRBSpeed() < 1f)
        {
            _owner.stateMachine.ChangeState(new PackState());
        }
    }

    public override void UpdateState(Buffaloid _owner)
    {

        chargeCheck(_owner);

        Debug.Log("Charge State stuff...");
        Debug.Log("Charge dir: " + chargeDir);
        Debug.DrawRay(_owner.transform.position, chargeDir, Color.yellow);

        timer -= Time.deltaTime;
        _owner.moveObject(chargeDir, 4f);

    }
}
