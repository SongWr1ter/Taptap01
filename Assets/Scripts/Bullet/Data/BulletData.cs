using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
[CreateAssetMenu(fileName = "BulletEmptyData", menuName = "BulletData", order = 1)]
public class BulletData : ScriptableObject
{
    public float range;
    public float speed;
    public int damage;
    public int pierceCount;
    public float pushForce;
    public bool Explosion;
    // 当且仅当Explision是true时，才显示下列属性
    [ShowIf("Explosion")]
    public float explosionRadius;
    [ShowIf("Explosion")]
    public float explosionDamage;
    [ShowIf("Explosion")]
    public LayerMask interactableLayer;
}
