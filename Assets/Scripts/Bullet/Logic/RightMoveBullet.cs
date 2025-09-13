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
                health.GetHurt(damage);
            }
            pierceCount--;
            if (pierceCount <= 0)
            {
                Recycle();
            }
        }
    }

    protected override void Fly()
    {
        Vector3 dir = Vector3.right;
        float moveDistance = speed * Time.fixedDeltaTime;
        transform.position += dir * moveDistance;
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
