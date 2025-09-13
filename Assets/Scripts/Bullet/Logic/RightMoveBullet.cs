using System;
using System.Collections;
using System.Collections.Generic;
using MemoFramework;
using UnityEngine;

public class RightMoveBullet : BulletBase
{
    protected float flyDistance;
    public override void InitData(BulletData bulletData)
    {
        base.InitData(bulletData);
        flyDistance = 0f;
    }

    protected override void OnHit(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & interactableLayer) != 0)
        {
            // 假设被击中的物体有一个Health组件
            IDamagable health = col.GetComponent<IDamagable>();
            if (health != null)
            {
                if (health.GetHurt(damage))
                {
                    ICanPushback pushback = col.GetComponent<ICanPushback>();
                    if (pushback != null)
                    {
                        pushback.Pushback(pushForce);
                    }
                    if (pierceCount-- <= 0)
                    {
                        Recycle();
                    }
                }
            }
            
        }
    }

    protected override void Fly()
    {
        float moveDistance = speed * Time.fixedDeltaTime;
        transform.position += transform.right * moveDistance;
        flyDistance += moveDistance;
        if (flyDistance >= range)
        {
            Recycle();
        }
    }

    protected override void Recycle()
    {
        MemoFrameworkEntry.GetComponent<ObjectPoolComponent>().Despawn(this);
    }

    public override void OnSpawned(object userData = null)
    {
        BulletData data = userData as BulletData;
        if (data != null)
        {
            InitData(data);
        }
    }

    public override void OnDespawned()
    {
        
    }
}
