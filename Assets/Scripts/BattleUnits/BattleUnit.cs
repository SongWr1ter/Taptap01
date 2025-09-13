using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FSM;
using MemoFramework.ObjectPool;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class Parameter
{
    public string Name;
    public int Health;
    public int MaxHealth;
    public float resistance;
    public float speed;
    public float speed_factor = 1f;
    public Vector3 direction;
    public float hurtDuration;
    public float attackInterval;
    public BaseAttack AttackLogic;
    [Header("如果是近战则无视这些选项")]
    public Transform shootTrans;
    public int ammo;
    public int maxAmmo;
    public float reloadingTime;
    [HideInInspector]
    public IObject io;
    public string hurtSFX;
    public string deadSFX;
    public string attackSFX;
}

public class BattleUnit : MonoBehaviour,IDamagable,ICanPushback,IObject
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
    private float duration = 0.5f;
    private bool canDamage = true;
    private bool canPushback = true;
    private float skillA_factor = 1f;
    private int face2Rgiht
    {
        get
        {
            if (faction == Faction.Monster) return 1;
            else return -1;
        }
    }
    #endregion
    #region 一堆引用
    protected FinateStateMachine fsm;
    // protected Collider2D[] colliderCache = new Collider2D[10];
    [SerializeField] private BattleUnitData bdata;
    #endregion

    protected void Awake()
    {
        fsm = GetComponent<FinateStateMachine>();
        fsm.data = data; // 将数值传递给状态机,无关顺序
        data.io = this;
    }

    

    public bool GetHurt(int damage)
    {
        if (!canDamage) return false;
        data.Health -= Mathf.FloorToInt(damage * skillA_factor);
        if (data.Health <= 0)
        {
            Die();
        }
        else
        {
            //  TODO
            // 播放受伤特效
            if(data.hurtSFX != "")
                SoundManager.PlayAudio(data.hurtSFX);
        }

        return true;
    }

    public void Heal(int heal)
    {
        data.Health += heal;
    }

    public void Die()
    {
        canDamage = false;
        fsm.EmitSignal(FinateStateMachine.SignalType.Any2Dead);
        if(data.deadSFX != "")
            SoundManager.PlayAudio(data.deadSFX);
    }

    private void Update()
    {
        
    }

    public void Pushback(float _force)
    {
        if (!canPushback || Mathf.Approximately(0f,_force)) return;
        // canDamage = false;
        canPushback = false;
        data.speed_factor = 0;
        transform.DOMoveX(transform.position.x - (face2Rgiht * _force), duration).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            data.speed_factor = 1f;
            // canDamage = true;
            canPushback = true;
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
            var col = Physics2D.OverlapBox(transform.position + Vector3.right * (face2Rgiht * Box_Width) / 2,
                new Vector2(Box_Width, Box_Height), 0, enemyLayerMask);
            if (col != null)
            {
                // 进入攻击状态
                fsm.EmitSignal(FinateStateMachine.SignalType.Move2Attack);
            }
        }else if (fsm.GetCurrentState().GetStateType() == StateType.Attack)
        {
            // 检测前方一定距离内是否有敌人
            var col = Physics2D.OverlapBox(transform.position + Vector3.right * (face2Rgiht * Box_Width) / 2,
                new Vector2(Box_Width, Box_Height), 0, enemyLayerMask);
            if (col == null)
            {
                // 离开攻击状态
                fsm.EmitSignal(FinateStateMachine.SignalType.Attack2Move);
            }
        }
    }

    protected void OnDrawGizmos()
    {
        if (faction == Faction.Tower)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + Vector3.left * Box_Width / 2, new Vector3(Box_Width, Box_Height, 0) );
            // 绘制一个箭头，箭头从Vector3.right方向开始,旋转angle度
            Gizmos.color = Color.yellow;
            return;
        }
        else
        {
            // 绘制一个矩形区域
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + Vector3.right * Box_Width / 2, new Vector3(Box_Width, Box_Height, 0) );
            // 绘制一个箭头，箭头从Vector3.right方向开始,旋转angle度
            Gizmos.color = Color.yellow;
        }
        
        // Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;
        // Gizmos.DrawLine(transform.position, transform.position + dir * 2);
    }

    public string Name { get; set; }
    public void OnSpawned(object userData = null)
    {
        if (userData != null)
        {
            BattleUnitData bdata = userData as BattleUnitData;
            data.MaxHealth = bdata.MaxHealth;
            data.resistance = bdata.resistance;
            data.speed = bdata.speed;
            data.hurtDuration = bdata.hurtDuration;
            data.attackInterval = bdata.attackInterval;
            data.AttackLogic = bdata.AttackLogic;
            data.Name = bdata.Name;
            Box_Width = bdata.Box_Width;
            Box_Height = bdata.Box_Height;
            faction = bdata.faction;
            data.hurtSFX = bdata.hurtSFX;
            data.deadSFX = bdata.deadSFX;
            data.attackSFX = bdata.AttackLogic.attackSFX;
            if (data.shootTrans) data.shootTrans.position = transform.position + bdata.shootPosOffset;
            if (bdata.rangedWeapon)
            {
                data.maxAmmo = ((RangedAttack)bdata.AttackLogic).ammo;
                data.ammo = data.maxAmmo;
                data.reloadingTime = ((RangedAttack)bdata.AttackLogic).reloadTime;
            }
            else
            {
                MeleeAttack ma = bdata.AttackLogic as MeleeAttack;
                var at = GetComponentInChildren<AttackArea>();
                at.SetDamage(ma.damage);
                at.SetPushForce(ma.pushForce);
                at.SetTargetLayerMask(ma.TargetTagLayerMask);
            }
            Name = faction.ToString();
            // 设置朝向
            if (faction == Faction.Tower)
            {
                data.direction = Vector3.left;
            }
            else
            {
                data.direction = Vector3.right;
            }
            // 重置血量
            data.Health = data.MaxHealth;
            //
            GetComponent<Animator>().runtimeAnimatorController = bdata.animator;
            GetComponent<SpriteRenderer>().sprite = bdata.sprite;
            data.AttackLogic.AttackInit();
        }
        else
        {
            Debug.LogError("Battle Unit Data Empty");
        }
        FactionSetup();
        canDamage = true;
        canPushback = true;
        skillA_factor = 1f;
        MessageCenter.AddListener(OnSkillA,MESSAGE_TYPE.SkillA);
        MessageCenter.AddListener(OnSkillAEnd,MESSAGE_TYPE.SkillAEnd);
    }

    public bool flag = true;
    private void Start()
    {
        // For Debug Only
        if(flag)
            OnSpawned(bdata);
    }
    public void OnDespawned()
    {
        MessageCenter.RemoveListener(OnSkillA, MESSAGE_TYPE.SkillA);
        MessageCenter.RemoveListener(OnSkillAEnd, MESSAGE_TYPE.SkillAEnd);
    }

    private void OnSkillA(CommonMessage msg)
    {
        if (faction == Faction.Monster)
        {
            skillA_factor = 2f;
        }
    }
    
    private void OnSkillAEnd(CommonMessage msg)
    {
        if (faction == Faction.Monster)
        {
            skillA_factor = 1f;
        }
    }
}
