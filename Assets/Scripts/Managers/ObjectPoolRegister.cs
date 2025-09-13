using System.Collections;
using System.Collections.Generic;
using MemoFramework;
using UnityEngine;
public enum PoolName
{
    Bullet,
    Character,
    Enemy,
    Effect,
    Item,
}
public class ObjectPoolRegister : SingleTon<ObjectPoolRegister>
{
    public ObjectPoolComponent _objectPool;
    // Start is called before the first frame update
    void Start()
    {
        _objectPool = MemoFrameworkEntry.GetComponent<ObjectPoolComponent>();
        // 注册所有对象池
        foreach (PoolName poolName in System.Enum.GetValues(typeof(PoolName)))
        {
            string str = poolName.ToString();
            _objectPool.CreateObjectPool(str,Resources.Load<GameObject>($"AutoLoadPrefabs/{str}"));
        }
    }
}
