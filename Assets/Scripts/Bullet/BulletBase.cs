using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    protected float damage;
    protected float speed;
    protected float lifeTime;
    protected float flyDistance;
    protected LayerMask interactableLayer;

    public abstract void InitData(BulletData bulletData);
    
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
}
