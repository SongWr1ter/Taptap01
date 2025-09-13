using System.Collections;
using System.Collections.Generic;
using MemoFramework;
using MemoFramework.ObjectPool;
using UnityEngine;

public class bulletShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public BulletData bulletData;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var bullet = 
                ObjectPoolRegister.Instance._objectPool.Spawn(
                    PoolName.Bullet.ToString(),transform.position,Quaternion.identity,bulletData);
        }
    }
}
