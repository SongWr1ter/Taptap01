using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    Melee,
    Ranged,
}
[CreateAssetMenu(menuName = "AttackConfig",fileName = "BaseAttack")]
public abstract class BaseAttack:ScriptableObject
{
    public string attackSFX;
    public abstract void AttackInit(object o = null);
    public abstract void AttackEnter();
    public abstract void AttackUpdate(float deltaTime);
    public abstract bool AttackUpdate(float deltaTime, Transform shootTrans);
    public abstract void AttackExit();
}
