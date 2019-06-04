using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuffaloidState;

public class PackState : State<Buffaloid>
{

    private float stuckTimer;
    private bool stuck;
    private float idleTimer;
    private bool idling;
    private float timeStuck;

    public override void EnterState(Buffaloid _owner)
    {
        timeStuck = 0.5f;
        //Debug.Log("Entering Pack State");
        stuckTimer = timeStuck;
        stuck = false;
        idling = false;
        idleTimer = 1f;
        _owner.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public override void ExitState(Buffaloid _owner)
    {
        //Debug.Log("Exiting Pack State");

    }

    // checks if buffaloid is stuck on terrain. If it is stuck for a short period of time, changes to stuck state.
    void stuckCheck(Buffaloid _owner)
    {
        if(stuck)
        {
            stuckTimer -= Time.deltaTime;
            //Debug.Log("stuckTimer: " + stuckTimer);
            if(stuckTimer < 0)
            {
                _owner.stateMachine.ChangeState(new StuckState());
            }
            else if(_owner.getRBSpeed() > 0.1f )
            {
                //.Log("stuck to false");

                stuck = false;
            }
        }
        else
        {
            if(_owner.getRBSpeed() <= 0.05f)
            {
                //Debug.Log("stuck to true");

                stuckTimer = timeStuck;
                stuck = true;
            }
        }
    }

    void idleCheck(Buffaloid _owner)
    {
        if(idling)
        {
            idleTimer -= Time.deltaTime;
            if (idleTimer < 0)
            {
                _owner.stateMachine.ChangeState(new IdleState());
            }
            else if (_owner.currentMove != Vector2.zero)
            {
                idling = false;
            }
        }
        else
        {
            if(_owner.currentMove == Vector2.zero )
            {
                idleTimer = 1f;
                idling = true;
            }
        }
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

 
        idleCheck(_owner);
        stuckCheck(_owner);
        chargeCheck(_owner );
        _owner.preyCheck();

        //Debug.Log("forwardVelocity " + _owner.getRBSpeed());
        //Debug.Log("stuck: " + stuck);

        //Debug.Log("stuck: " + stuck);
        //Debug.Log("speed: " + _owner.getRBSpeed());

        //Debug.Log("stuckTimer: " + stuckTimer);

        //Debug.Log("calling move in PackState: " + _owner.currentMove);

        _owner.moveObject(_owner.currentMove, 0.32f);
    }
}
