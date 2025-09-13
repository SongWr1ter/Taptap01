using UnityEngine;

namespace FSM
{
    public enum StateType
    {
        Idle,
        Move,
        Hurt,
        Dead,
        Attack
    }

    public abstract class State
    {
        public StateType Type { get; protected set; }
        protected FinateStateMachine fsm;

        public State(FinateStateMachine fsm)
        {
            this.fsm = fsm;
        }
        public abstract void Enter();
        public abstract void Execute();
        public abstract void Exit();
        public StateType GetStateType() => Type;
    }

    public class IdleState : State
    {
        public IdleState(FinateStateMachine fsm) : base(fsm)
        {
            Type = StateType.Idle;
        }
        public override void Enter()
        {
            UnityEngine.Debug.Log("IdleState Enter");
            /* 待机状态进入逻辑 */
            // 播放待机动画
        }
        public override void Execute()
        {
            UnityEngine.Debug.Log("IdleState Execute");
            /* 待机状态执行逻辑 */
        }
        public override void Exit()
        {
            UnityEngine.Debug.Log("IdleState Exit");
            /* 待机状态退出逻辑 */
        }
    }

    public class MoveState : State
    {
        public MoveState(FinateStateMachine fsm) : base(fsm)
        {
            Type = StateType.Move;
        }
        public override void Enter()
        {
            UnityEngine.Debug.Log("MoveState Enter");
            /* 移动状态进入逻辑 */
        }
        public override void Execute()
        {
            Vector3 dir = fsm.data.direction;
            dir.y = 0;
            dir.Normalize();
            fsm.rb.velocity = dir * (fsm.data.speed * fsm.data.speed_factor);
        }
        public override void Exit()
        {
            UnityEngine.Debug.Log("MoveState Exit");
            /* 移动状态退出逻辑 */
        }
    }

    public class HurtState : State
    {
        private float timer = 0f;
        public HurtState(FinateStateMachine fsm) : base(fsm)
        {
            Type = StateType.Hurt;
        }
        public override void Enter()
        {
            timer = 0f;
        }
        public override void Execute()
        {
            timer += Time.deltaTime;
            if (timer >= fsm.data.hurtDuration)
            {
                fsm.EmitSignal(FinateStateMachine.SignalType.Hurt2Idle);
                // TODO
                // 受伤特效
            }
        }
        public override void Exit()
        {
            UnityEngine.Debug.Log("HurtState Exit");
            /* 受伤状态退出逻辑 */
        }
        
    }

    public class DeadState : State
    {
        public DeadState(FinateStateMachine fsm) : base(fsm)
        {
            Type = StateType.Dead;
        }
        public override void Enter()
        {
            UnityEngine.Debug.Log("DeadState Enter");
            /* 死亡状态进入逻辑 */
        }
        public override void Execute()
        {
            UnityEngine.Debug.Log("DeadState Execute");
            /* 死亡状态执行逻辑 */
        }
        public override void Exit()
        {
            UnityEngine.Debug.Log("DeadState Exit");
            /* 死亡状态退出逻辑 */
        }
    }

    public class AttackState : State
    {
        private float timer = 0f;
        public AttackState(FinateStateMachine fsm) : base(fsm)
        {
            Type = StateType.Attack;
        }
        public override void Enter()
        {
            //重复播放攻击动画or射一次子弹
        }
        public override void Execute()
        {
            timer += Time.deltaTime;
            if (timer >= fsm.data.attackInterval)
            {
                // TODO
                // 攻击特效
                // 射一次  
            }
        }
        public override void Exit()
        {
            UnityEngine.Debug.Log("AttackState Exit");
            /* 攻击状态退出逻辑 */
        }
    }
}