using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuffaloidState;

public class IdleState : State<Buffaloid>
{
    //private static IdleState _instance;
    private float timer;
    private bool idling;
    private Vector2 randDir;

    /*
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
    }*/

    public override void EnterState(Buffaloid _owner)
    {
        //Debug.Log("Entering Idle State");
        timer = Random.Range(3f,8f);
        idling = true;
        _owner.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;

    }

    public override void ExitState(Buffaloid _owner)
    {
        //Debug.Log("Exiting Idle State");

    }

    //called to switch between standing still and moving in random direction
    void idleSwitch()
    {
        timer = Random.Range(8f, 15f);
        if (idling)
        {
            idling = false;
            randDir = Random.insideUnitCircle.normalized;
            //Debug.Log("moving in random direction: " + randDir);
        }
        else
        {
            idling = true;
            //Debug.Log("now idling and decelerating");
        }
    } 



    public override void UpdateState(Buffaloid _owner)
    {
        if (_owner.currentMove != Vector2.zero)
        {
            _owner.stateMachine.ChangeState(new PackState());
        }

        timer -= Time.deltaTime;

        _owner.preyCheck();

        if(timer < 0f)
        {
            idleSwitch();
        }
        //.Log("time idle: " + timer);

        if(idling)
        {
            //Debug.Log("decelerating");

            _owner.moveObject(Vector2.zero, 0f);

        }
        else
        {
            //Debug.Log("moving in rand dir: " + randDir);

            _owner.moveObject(randDir, 0.25f);
        }
    }
}
