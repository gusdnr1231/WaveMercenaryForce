using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(Stat AttackType, float Value);
    public void TakeHeal(float Value);
    public void ActiveEnd();
}
