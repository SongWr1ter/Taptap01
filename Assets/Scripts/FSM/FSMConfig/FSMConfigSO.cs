using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;
[CreateAssetMenu(fileName = "Attack", menuName = "FSM Config")]
public class FSMConfigSO : ScriptableObject
{
    public enum FSMType
    {
        Attack,
        Defend
    }
    public FSMType fsmType;
    public List<StateType> states = new List<StateType>();
    public List<FinateStateMachine.SignalType> signals = new List<FinateStateMachine.SignalType>();
}
