using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuffaloidState;


//State when buffaloid is stuck on an object, and will attempt to get unstuck
public class StuckState : State<Buffaloid>
{
    private float unstuckTimer;

    public override void EnterState(Buffaloid _owner)
    {
        unstuckTimer = 3f;
        Debug.Log("Entering Stuck State");
    }

    public override void ExitState(Buffaloid _owner)
    {
        Debug.Log("Exiting Stuck State");

    }

    public override void UpdateState(Buffaloid _owner)
    {

        if (unstuckTimer <= 0f)
        {
            _owner.stateMachine.ChangeState(new PackState());
        }
        unstuckTimer -= Time.deltaTime; ;

        //picks 1 or -1 at random to determine direction of turn to get unstuck
        float randDir = Random.Range(0, 2) * 2 - 1;
        _owner.torqueRotate(0.1f, randDir);
        Debug.Log("stuck state stuff");

    }
}

