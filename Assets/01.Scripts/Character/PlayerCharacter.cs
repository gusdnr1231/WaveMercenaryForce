using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoCharacter, IDamageable
{
    [Space]
    public EnemyCharacter AttackEnemy;

    private void SetDefaultSpirit()
    {
        characterSpirit.CurrentSpirit = characterSpirit.DefaultSpirit;
    }

    #region IDamageable Methods

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

    #endregion
}
