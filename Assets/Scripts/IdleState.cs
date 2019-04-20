using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuffaloidState;

public class IdleState : State<Buffaloid>
{
    private static IdleState _instance;


    //private constructor called from buffaloid
    private IdleState()
    {
        if(_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static IdleState Instance
    {
        get
        {
            if(_instance == null)
            {
                new IdleState();
            }
            return _instance;
        }
    }

    public override void EnterState(Buffaloid _owner)
    {
        Debug.Log("Entering Idle State");
    }

    public override void ExitState(Buffaloid _owner)
    {
        Debug.Log("Exiting Idle State");

    }

    public override void UpdateState(Buffaloid _owner)
    {
        if (_owner.switchState)
        {
            _owner.stateMachine.ChangeState(PackState.Instance);
            _owner.switchState = false;
        }

        Debug.Log("Some idle behavior");

    }
}
