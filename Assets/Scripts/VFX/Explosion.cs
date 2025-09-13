using System;
using System.Collections;
using System.Collections.Generic;
using MemoFramework.ObjectPool;
using UnityEngine;


public class Explosion : MonoBehaviour,IObject
{
    public string Name { get; set; }
    private float timer = 0f;
    public void OnSpawned(object userData = null)
    {
        timer = 0f;
        GetComponent<Animator>().Play("explosion");
    }

    public void OnDespawned()
    {
        
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= 0.5f)
        {
            timer = 0f;
            ObjectPoolRegister.Instance._objectPool.Despawn(this);
        }
    }
}
