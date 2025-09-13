using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class SignalEmiter : MonoBehaviour
{
    public FinateStateMachine fsm;
    public FinateStateMachine.SignalType signal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            fsm.EmitSignal(signal);
        }
    }
}
