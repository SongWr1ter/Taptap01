using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FSM;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class Parameter
{
    public int Health;
    public int MaxHealth;
    public float resistance;
    public float speed;
    public float speed_factor = 1f;
    public Vector3 direction;
    public float hurtDuration;
    public float attackInterval;
    public BaseAttack AttackLogic;
}

public class BattleUnit : MonoBehaviour,IDamagable,ICanPushback
{
    public enum Faction
    {
        Tower,
        Monster,
    }
    #region 数值类变量
    public Parameter data = new Parameter();
    public float Box_Width = 4f;
    public float Box_Height = 4f;
    public Faction faction = Faction.Monster;
    public LayerMask enemyLayerMask = 1;
    public float force;
    public float duration;
    #endregion
    #region 一堆引用
    protected FinateStateMachine fsm;
    // protected Collider2D[] colliderCache = new Collider2D[10];
    #endregion

    protected void Awake()
    {
        fsm = GetComponent<FinateStateMachine>();
        fsm.data = data; // 将数值传递给状态机,无关顺序
        FactionSetup();
    }

    private void OnEnable()
    {
        data.Health = data.MaxHealth;
    }

    public void GetHurt(int damage)
    {
        data.Health -= damage;
        if (data.Health <= 0)
        {
            Die();
        }
        else
        {
            //  TODO
            // 播放受伤特效
        }
    }

    public void Heal(int heal)
    {
        data.Health += heal;
    }

    public void Die()
    {
        fsm.EmitSignal(FinateStateMachine.SignalType.Any2Dead);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Pushback(force);
        }
    }

    public void Pushback(float _force)
    {
        
        data.speed_factor = 0;
        transform.DOMoveX(transform.position.x - _force, duration).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            data.speed_factor = 1f;
        });
    }

    protected void FactionSetup()
    {
        if (faction == Faction.Monster)
        {
            enemyLayerMask = 1 << LayerMask.NameToLayer("Tower");
            // 设置自己的LayerMask
            transform.gameObject.layer = LayerMask.NameToLayer("Monster");
        }else if (faction == Faction.Tower)
        {
            enemyLayerMask = 1 << LayerMask.NameToLayer("Monster");
            transform.gameObject.layer = LayerMask.NameToLayer("Tower");
        }
    }

    protected void FixedUpdate()
    {
        // 状态转换核心部分
        TransitionNet();
    }
    protected virtual void TransitionNet()
    {
        if (fsm.GetCurrentState().GetStateType() == StateType.Move)
        {
            // 检测前方一定距离内是否有敌人
            var col = Physics2D.OverlapBox(transform.position + Vector3.right * Box_Width / 2,
                new Vector2(Box_Width, Box_Height), 0, enemyLayerMask);
            if (col != null)
            {
                // 进入攻击状态
                fsm.EmitSignal(FinateStateMachine.SignalType.Move2Attack);
            }
        }else if (fsm.GetCurrentState().GetStateType() == StateType.Attack)
        {
            // 检测前方一定距离内是否有敌人
            var col = Physics2D.OverlapBox(transform.position + Vector3.right * Box_Width / 2,
                new Vector2(Box_Width, Box_Height), 0, enemyLayerMask);
            if (col == null)
            {
                // 进入攻击状态
                fsm.EmitSignal(FinateStateMachine.SignalType.Attack2Move);
            }
        }
    }

    protected void OnDrawGizmos()
    {
        // 绘制一个矩形区域
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.right * Box_Width / 2, new Vector3(Box_Width, Box_Height, 0) );
        // 绘制一个箭头，箭头从Vector3.right方向开始,旋转angle度
        Gizmos.color = Color.yellow;
        // Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;
        // Gizmos.DrawLine(transform.position, transform.position + dir * 2);
    }
}
