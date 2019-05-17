using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuffaloidState;


//State when buffaloid is stuck on an object, and will attempt to get unstuck
public class StuckState : State<Buffaloid>
{
    public float reverseSpeed = 1f;
    public float rotateSpeed = 0.06f;
    public float unstuckTime = 3f;
    private float unstuckTimer;
    float randDir;

    public override void EnterState(Buffaloid _owner)
    {
        unstuckTimer = unstuckTime;
        Debug.Log("Entering Stuck State");
        _owner.gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
        randDir = Random.Range(0, 2) * 2 - 1;
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
       
        _owner.torqueRotate(rotateSpeed, randDir);
        _owner.reverseMove(reverseSpeed);
        Debug.Log("stuck state stuff");

    }
}

