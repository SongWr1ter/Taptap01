using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collision2D))]
public class AttackArea : MonoBehaviour
{
    public string targetTag;
    private int damage;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            if (TryGetComponent(out IDamagable i))
            {
                i.GetHurt(damage);
            }
        }
    }
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
}
