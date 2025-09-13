using MemoFramework.ObjectPool;
using UnityEngine;

namespace FSM
{
    public enum StateType
    {
        Idle,
        Move,
        Hurt,
        Dead,
        Attack,
        Reloading,
    }
    
    public enum ComplexStateType
    {
        Idle,
        Move,
        Hurt,
        Dead,
        Attack,
        UpdatedAttack,
        Reloading,
    }

    public abstract class State
    {
        public static StateType ConvertToStateType(ComplexStateType complexStateType)
        {
            switch (complexStateType)
            {
                case ComplexStateType.Idle:
                    return StateType.Idle;
                case ComplexStateType.Move:
                    return StateType.Move;
                case ComplexStateType.Hurt:
                    return StateType.Hurt;
                case ComplexStateType.Dead:
                    return StateType.Dead;
                case ComplexStateType.Attack:
                    return StateType.Attack;
                case ComplexStateType.UpdatedAttack:
                    return StateType.Attack;
                case ComplexStateType.Reloading:
                    return StateType.Reloading;
                default:
                    return StateType.Idle;
            }
        }
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
            fsm.anim.Play("Idle");
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
            fsm.anim.Play("Idle");
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
            fsm.rb.velocity = Vector3.zero;
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
        private float timer = 0f;
        public DeadState(FinateStateMachine fsm) : base(fsm)
        {
            Type = StateType.Dead;
        }
        public override void Enter()
        {
            timer = 0f;
            fsm.anim.Play("Dead");
            UnityEngine.Debug.Log("DeadState Enter");
            /* 死亡状态进入逻辑 */
        }
        public override void Execute()
        {
            // 死亡动画播放完毕后等一段时间然后消失
            if (fsm.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                timer += Time.deltaTime;
                if (timer >= 1f)
                {
                    ObjectPoolRegister.Instance._objectPool.Despawn(fsm.data.io);
                }
            }
            
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
            fsm.anim.Play("Attack");
            //重复播放攻击动画or射一次子弹
            UnityEngine.Debug.Log("AttackState In");
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
    
    public class UpdatedAttackState : State
    {
        public UpdatedAttackState(FinateStateMachine fsm) : base(fsm)
        {
            Type = StateType.Attack;
        }
        public override void Enter()
        {
            fsm.anim.Play("Attack");
            fsm.data.AttackLogic.AttackEnter();
        }
        public override void Execute()
        {
            if(fsm.data.AttackLogic.AttackUpdate(Time.deltaTime,fsm.data.shootTrans))
            {
                // --fsm.data.ammo;
                // if (fsm.data.ammo <= 0)
                // {
                //     fsm.EmitSignal(FinateStateMachine.SignalType.Attack2Reloading);
                // }
            }
        }
        public override void Exit()
        {
            fsm.data.AttackLogic.AttackExit();   
        }
    }
    
    public class ReloadingState : State
    {
        private float timer = 0f;
        public ReloadingState(FinateStateMachine fsm) : base(fsm)
        {
            Type = StateType.Reloading;
        }
        public override void Enter()
        {
            fsm.anim.Play("Reload");
            timer = 0f;
            UnityEngine.Debug.Log("ReloadingState In");
        }
        public override void Execute()
        {
            
        }
        public override void Exit()
        {
            UnityEngine.Debug.Log("ReloadingState Exit");
            /* 装弹状态退出逻辑 */
        }
    }
}