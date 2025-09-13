using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AttackConfig/MeleeAttack",fileName = "MeleeAttack")]

public class MeleeAttack : BaseAttack
{
    //近战攻击：对一定矩形范围内的敌人造成伤害

    public override void AttackInit(object o)
    {
        
    }

    public override void AttackEnter()
    {
        
    }

    public override void AttackUpdate(float deltaTime)
    {
        
    }

    public override bool AttackUpdate(float deltaTime, Transform shootTrans)
    {
        return false;
    }

    public override void AttackExit()
    {
        
    }
}
