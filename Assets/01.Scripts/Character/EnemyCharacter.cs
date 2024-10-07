using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : MonoCharacter, IDamageable
{
    [Space]
    public PlayerCharacter AttackEnemy;

    public void TakeDamage(float Value)
    {
        Debug.Log($"{this.name} Take Damage");
    }

    public void TakeHeal(float Value)
    {
        Debug.Log($"{this.name} Take Heal");
    }

    public void ActiveDead()
    {
        Debug.Log($"{this.name} is Dead");
    }
}
