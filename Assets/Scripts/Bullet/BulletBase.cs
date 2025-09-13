using System.Collections;
using System.Collections.Generic;
using MemoFramework.ObjectPool;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public abstract class BulletBase : MonoBehaviour,IObject
{
    protected int damage;
    protected float speed;
    // protected float lifeTime;
    protected float range;
    protected int pierceCount;
    protected float pushForce;
    protected bool Explosion;
    // 当且仅当Explision是true时，才显示下列属性
    protected float explosionRadius;
    protected int explosionDamage;
    protected LayerMask interactableLayer;
    public virtual void InitData(BulletData bulletData)
    {
        damage = bulletData.damage;
        speed = bulletData.speed;
        // lifeTime = bulletData.lifeTime;
        range = bulletData.range;
        pierceCount = bulletData.pierceCount;
        pushForce = bulletData.pushForce;
        Explosion = bulletData.Explosion;
        explosionRadius = bulletData.explosionRadius;
        explosionDamage = bulletData.explosionDamage;
        interactableLayer = bulletData.interactableLayer;
    }
    
    protected abstract void OnHit(Collider2D col);
    
    protected abstract void Fly();

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        OnHit(collision);
    }

    protected void FixedUpdate()
    {
        Fly();
    }

    protected abstract void Recycle();
    public string Name { get; set; }
    public abstract void OnSpawned(object userData = null);

    public abstract void OnDespawned();
}
