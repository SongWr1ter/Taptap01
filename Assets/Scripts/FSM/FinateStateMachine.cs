using System;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace FSM
{
    public class FinateStateMachine : MonoBehaviour
    {
        // 信号类型枚举，格式为 [FromState]2[ToState]
        public enum SignalType
        {
            Idle2Move,      // 从Idle状态转换到Move状态
            Move2Idle,      // 从Move状态转换到Idle状态
            Any2Hurt,       // 从任意状态转换到Hurt状态
            Any2Dead,       // 从任意状态转换到Dead状态
            Idle2Attack,    // 从Idle状态转换到Attack状态
            Move2Attack,    // 从Move状态转换到Attack状态
            Attack2Idle,    // 从Attack状态转换到Idle状态
            Hurt2Idle,       // 从Hurt状态转换到Idle状态
            // 可以根据需要继续扩展
            Attack2Move,
        }
        
        private Dictionary<StateType, State> stateDict;
        private Dictionary<SignalType, System.Action> signalHandlers;
        private SignalType? pendingSignal = null; // 当前待处理的信号
        private bool signalProcessedThisFrame = false; // 标记信号是否在本帧被处理
        private bool blocked = false; // 是否阻止状态转换
        [ShowInInspector, ReadOnly] private StateType currentStateType;
        private State currentState;
        [SerializeField]
        protected FSMConfigSO config;
        [SerializeField]
        protected StateType defaultState = StateType.Idle;
        [HideInInspector]
        public Parameter data;
        public Animator anim;
        public Rigidbody2D rb;
        public void Awake()
        {
            stateDict = new Dictionary<StateType, State>();
            signalHandlers = new Dictionary<SignalType, System.Action>();
            // Unity特化
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            // 初始化状态字典
            RegisterStatesFromConfig();

            // 初始化信号处理器字典
            // signalHandlers = new Dictionary<SignalType, System.Action>
            // {
            //     { SignalType.Idle2Move, () => HandleTransitionSignal(StateType.Idle, StateType.Move) },
            //     { SignalType.Move2Idle, () => HandleTransitionSignal(StateType.Move, StateType.Idle) },
            //     { SignalType.Any2Hurt, () => HandleTransitionSignal(null, StateType.Hurt) },
            //     { SignalType.Any2Dead, () => HandleTransitionSignal(null, StateType.Dead) },
            //     { SignalType.Idle2Attack, () => HandleTransitionSignal(StateType.Idle, StateType.Attack) },
            //     { SignalType.Move2Attack, () => HandleTransitionSignal(StateType.Move, StateType.Attack) },
            //     { SignalType.Attack2Idle, () => HandleTransitionSignal(StateType.Attack, StateType.Idle) },
            //     { SignalType.Hurt2Idle, () => HandleTransitionSignal(StateType.Hurt, StateType.Idle) }
            // };
            RegisterSignalsFromConfig();
        }
        
        private void RegisterStatesFromConfig()
        {
            if (config == null) return;

            foreach (var stateType in config.states)
            {
                var cache = State.ConvertToStateType(stateType);
                if (!stateDict.ContainsKey(cache))
                {
                    switch (stateType)
                    {
                        case ComplexStateType.Idle:
                            stateDict[cache] = new IdleState(this);
                            break;
                        case ComplexStateType.Move:
                            stateDict[cache] = new MoveState(this);
                            break;
                        case ComplexStateType.Hurt:
                            stateDict[cache] = new HurtState(this);
                            break;
                        case ComplexStateType.Dead:
                            stateDict[cache] = new DeadState(this);
                            break;
                        case ComplexStateType.Attack:
                            stateDict[cache] = new AttackState(this);
                            break;
                        case ComplexStateType.UpdatedAttack:
                            stateDict[cache] = new UpdatedAttackState(this);
                            break;
                        // 根据需要添加更多状态
                    }
                }
            }
        }
        private void RegisterSignalsFromConfig()
        {
            if (config == null) return;

            foreach (var signal in config.signals)
            {
                if (!signalHandlers.ContainsKey(signal))
                {
                    var (fromState, toState) = ParseSignal(signal);
                    RegisterTransitionSignal(signal, fromState, toState);
                }
            }
        }
        private void OnEnable()
        {
            ChangeState(defaultState);
        }

        private void OnDisable()
        {
            blocked = true;
            // 清理当前状态
            if (currentState != null)
            {
                currentState.Exit();
                currentState = null;
            }
            // 清理待处理信号
            pendingSignal = null;
            signalProcessedThisFrame = false;
            blocked = false;
        }

        public void Update()
        {
            // 重置信号处理标志
            signalProcessedThisFrame = false;

            // 检查是否有待处理的信号
            if (pendingSignal.HasValue && signalHandlers.ContainsKey(pendingSignal.Value))
            {
                // 调用对应的信号处理函数
                signalHandlers[pendingSignal.Value]?.Invoke();
                
                // 如果信号被处理，清除待处理信号
                if (signalProcessedThisFrame)
                {
                    pendingSignal = null;
                }
            }

            // 执行当前状态的逻辑
            if (currentState != null)
                currentState.Execute();
        }
        /// <summary>
        /// 发射状态转换信号
        /// </summary>
        /// <param name="signal">信号类型</param>
        public void EmitSignal(SignalType signal)
        {
            // 如果当前的信号是Any2类型，检测一下是否重复进入目标状态
            if (signal.ToString().StartsWith("Any2"))
            {
                var (_, toState) = ParseSignal(signal);
                if (currentState != null && currentState.GetStateType() == toState)
                {
                    // Debug.LogWarning($"Already in state {toState}, ignoring signal {signal}.");
                    return;
                }
            }
            pendingSignal = signal;
            signalProcessedThisFrame = false;
        }

        /// <summary>
        /// 处理状态转换信号
        /// </summary>
        /// <param name="requiredFromState">需要的起始状态类型，null表示任意状态</param>
        /// <param name="targetState">目标状态类型</param>
        private void HandleTransitionSignal(StateType? requiredFromState, StateType targetState)
        {
            // 检查是否阻止状态转换
            if (blocked)
            {
                Debug.LogWarning($"State transition blocked. Cannot transition to {targetState}.");
                return;
            }
            // 检查当前状态是否符合要求
            bool canTransition = requiredFromState == null || 
                               (currentState != null && currentState.GetStateType() == requiredFromState.Value);

            if (canTransition)
            {
                // 符合条件，执行状态转换
                ChangeState(targetState);
                signalProcessedThisFrame = true;
            }
            else
            {
                Debug.LogWarning($"Cannot transition from {currentState?.GetStateType()} to {targetState}. " +
                               $"Required from state: {requiredFromState}");
            }
        }

        /// <summary>
        /// 根据信号类型获取起始状态和目标状态
        /// </summary>
        private (StateType? fromState, StateType toState) ParseSignal(SignalType signal)
        {
            string signalStr = signal.ToString();
            
            if (signalStr.StartsWith("Any2"))
            {
                string toStateStr = signalStr.Substring(4);
                if (Enum.TryParse(toStateStr, out StateType toState))
                {
                    return (null, toState);
                }
            }
            else if (signalStr.Contains("2"))
            {
                string[] parts = signalStr.Split(new string[] { "2" }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    if (Enum.TryParse(parts[0], out StateType fromState) && 
                        Enum.TryParse(parts[1], out StateType toState))
                    {
                        return (fromState, toState);
                    }
                }
            }

            Debug.LogError($"Invalid signal format: {signal}");
            return (null, StateType.Idle); // 默认返回Idle状态
        }

        /// <summary>
        /// 动态注册信号处理器,字典操作, 参数是函数
        /// </summary>
        public void RegisterSignalHandler(SignalType signal, System.Action handler)
        {
            signalHandlers[signal] = handler;
        }

        /// <summary>
        /// 动态创建并注册信号处理器,字典操作, 参数是状态类型
        /// </summary>
        public void RegisterTransitionSignal(SignalType signal, StateType? fromState, StateType toState)
        {
            signalHandlers[signal] = () => HandleTransitionSignal(fromState, toState);
        }

        /// <summary>
        /// 设置是否阻止状态转换
        /// </summary>
        public void SetBlocked(bool value)
        {
            blocked = value;
        }

        public void ChangeState(State newState)
        {
            if (currentState != null)
                currentState.Exit();
            currentState = newState;
            if (currentState != null)
            {
                currentState.Enter();
                currentStateType = currentState.GetStateType();
            }
                
        }

        public void ChangeState(StateType type)
        {
            if (stateDict.TryGetValue(type, out var newState))
            {
                ChangeState(newState);
            }
        }
        
        public State GetCurrentState()
        {
            return currentState;
        }

        public State GetState(StateType type)
        {
            stateDict.TryGetValue(type, out var state);
            return state;
        }

        /// <summary>
        /// 检查是否可以发射某个信号（检查当前状态是否符合起始状态要求）
        /// </summary>
        public bool CanEmitSignal(SignalType signal)
        {
            if (signalHandlers.TryGetValue(signal, out var handler))
            {
                var (fromState, _) = ParseSignal(signal);
                return fromState == null || (currentState != null && currentState.GetStateType() == fromState.Value);
            }
            return false;
        }

        /// <summary>
        /// 获取当前待处理的信号
        /// </summary>
        public SignalType? GetPendingSignal()
        {
            return pendingSignal;
        }
    }
}