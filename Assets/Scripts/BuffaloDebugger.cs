using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffaloDebugger : MonoBehaviour
{

    [SerializeField]
    public Text colliderText;
    public Text stateText;
    public Text randomText;
    // Start is called before the first frame update
    void Start()
    {
        colliderText.text = "Num avoid colliders: ";
    }

    public void SetStateText(BuffaloidState.State<Buffaloid> state )
    {
        if(state is PackState)
        {
            stateText.text = "State: Pack";
        }
        else if (state is IdleState)
        {
            stateText.text = "State: Idle";
        }
        else if (state is StuckState)
        {
            stateText.text = "State: Stuck";
        }
    }

    public void SetColliderText(int numColliders)
    {
        colliderText.text = "Num avoid colliders: " + numColliders;
    }

    public void SetRandomDebugText(string str) 
    {
        randomText.text = str;
    }

}
