using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    Melee,
    Ranged,
}

public abstract class BaseAttack
{
    public abstract void Attack();
}
