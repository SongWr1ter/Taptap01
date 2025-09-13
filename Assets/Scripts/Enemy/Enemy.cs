using MemoFramework.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour,IObject
{
    public string Name { get ; set; }
    //public Transform transform => gameObject.transform;

    private float lifeTime = 5f; 
    private float timer;

    public void OnDespawned()
    {
        gameObject.SetActive(false);
    }

    public void OnSpawned(object userData = null)
    {
        timer = 0f;
        gameObject.SetActive(true);

        if (userData is Vector3 pos)
        {
            transform.position = pos; 
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
            ObjectPoolRegister.Instance._objectPool.Despawn(this);
    }
}
