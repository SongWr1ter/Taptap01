using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
[CreateAssetMenu(fileName = "BattleUnitData", menuName = "BattleUnits/Data")]
public class BattleUnitData : ScriptableObject
{
    public string Name;
    public Sprite sprite;
    public AnimatorOverrideController animator;
    public int MaxHealth;
    public float resistance;
    public float speed;
    public float hurtDuration;
    public float attackInterval;
    public BaseAttack AttackLogic;
    public float Box_Width = 4f;
    public float Box_Height = 4f;
    public BattleUnit.Faction faction;
    public bool rangedWeapon = false;
    public Vector3 shootPosOffset;
    [Header("SFX")] 
    public string hurtSFX;
    public string deadSFX;
}
