using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour,IDamagable
{
    public int Health;

    public bool GetHurt(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
            return true;
        }
        return false;
    }

    public void Heal(int heal)
    {
        
    }

    public void Die()
    {
        //Game Over
        MessageCenter.SendMessage(new CommonMessage()
        {
            
        },MESSAGE_TYPE.GAME_OVER);
    }
}
