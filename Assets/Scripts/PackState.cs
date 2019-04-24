using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuffaloidState;

public class PackState : State<Buffaloid>
{

    private float stuckTimer;
    private bool stuck;
    //private static PackState _instance;


    //private constructor called from buffaloid
    /*private PackState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static PackState Instance
    {
        get
        {
            if (_instance == null)
            {
                new PackState();
            }
            return _instance;
        }
    }*/

    public override void EnterState(Buffaloid _owner)
    {
        Debug.Log("Entering Pack State");
        stuckTimer = 1f;
        stuck = false;
    }

    public override void ExitState(Buffaloid _owner)
    {
        Debug.Log("Exiting Pack State");

    }

    //
    void stuckCheck(Buffaloid _owner)
    {
        if(stuck)
        {
            stuckTimer -= Time.deltaTime;
            if(stuckTimer < 0)
            {
                _owner.stateMachine.ChangeState(new StuckState());
            }
            else if(_owner.getRBSpeed() > 0.01f )
            {
                stuck = false;
            }
        }
        else
        {
            if(_owner.getRBSpeed() <= 0.01f)
            {
                stuckTimer = 3f;
                stuck = true;
            }
        }
    }

    public override void UpdateState(Buffaloid _owner)
    {
        if(_owner.currentMove == Vector2.zero)
        {
            _owner.stateMachine.ChangeState(new IdleState() );
        }

        stuckCheck(_owner);

        Debug.Log("forwardVelocity " + _owner.getRBSpeed());

        //if( _owner.forwardVelocity)

        Debug.Log("calling move in PackState: " + _owner.currentMove);

        _owner.moveObject(_owner.currentMove, 1f);
    }
}
