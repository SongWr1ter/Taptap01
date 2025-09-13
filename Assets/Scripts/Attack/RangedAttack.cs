using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AttackConfig/RangedAttack",fileName = "RangedAttack")]

public class RangedAttack : BaseAttack
{
    [Tooltip("和Prefab也就是模型相关")]
    public PoolName bulletType;
    public BulletData bulletData;
    [Tooltip("一秒钟发射几发子弹")]
    public float shotSpeed;

    public float reloadTime;
    public int ammo;
    private float shotInverval;

    private float timer = 0f;
    
    public override void AttackInit(object o = null)
    {
        shotInverval = 1 / shotSpeed;
    }
    
    public override void AttackEnter()
    {
        timer = shotInverval;
    }

    public override void AttackUpdate(float deltaTime)
    {
        throw new System.NotImplementedException();
    }

    public override bool AttackUpdate(float deltaTime,Transform shootTrans)
    {
        timer += deltaTime;
        if (timer >= shotInverval)
        {
            timer = 0f;
            SoundManager.PlayAudioWithLimit(attackSFX);
            // Create Bullet
            ObjectPoolRegister.Instance._objectPool.Spawn(bulletType.ToString(), shootTrans.position,
                shootTrans.rotation, bulletData);
            return true;
        }

        return false;
    }

    public override void AttackExit()
    {
        
    }
}
