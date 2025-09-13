using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collision2D))]
public class AttackArea : MonoBehaviour
{
    [SerializeField] private LayerMask TargetTagLayerMask;
    [SerializeField] private int damage;
    [SerializeField]private float pushForce = 1f;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & TargetTagLayerMask) != 0 )
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
                }
            }
        }
    }
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
    
    public void SetPushForce(float pushForce)
    {
        this.pushForce = pushForce;
    }
    
    public void SetTargetLayerMask(LayerMask layerMask)
    {
        this.TargetTagLayerMask = layerMask;
    }
}
