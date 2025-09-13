using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AttackConfig/RangedAttack",fileName = "RangedAttack")]

public class RangedAttack : BaseAttack
{
    public string bullet;
    public float range;
    public float damage;
    public float shotSpeed;

    private float shotInverval;

    private float timer = 0f;
    public override void AttackInit()
    {
        timer = 0f;
        shotInverval = 1 / shotSpeed;
    }

    public override void AttackUpdate(float deltaTime)
    {
        timer += deltaTime;
        if (timer >= shotInverval)
        {
            timer = 0f;
            // Create Bullet
            
        }
    }

    public override void AttackExit()
    {
        throw new System.NotImplementedException();
    }
}
