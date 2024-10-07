using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(float Value);
    public void TakeHeal(float Value);
    public void ActiveDead();
}
