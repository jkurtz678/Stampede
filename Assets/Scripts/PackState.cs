using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuffaloidState;

public class PackState : State<Buffaloid>
{
    private static PackState _instance;


    //private constructor called from buffaloid
    private PackState()
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
    }

    public override void EnterState(Buffaloid _owner)
    {
        Debug.Log("Entering Pack State");
    }

    public override void ExitState(Buffaloid _owner)
    {
        Debug.Log("Exiting Pack State");

    }

    public override void UpdateState(Buffaloid _owner)
    {
        if(_owner.switchState)
        {
            _owner.stateMachine.ChangeState(IdleState.Instance);
            _owner.switchState = false;

        }

        Debug.Log("calling move in PackState: " + _owner.currentMove);

        _owner.moveObject(_owner.currentMove, 1f);
    }
}
