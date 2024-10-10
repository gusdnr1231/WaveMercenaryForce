using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackMethod
{
    Basic = 0,
    Combo = 1,
}

public interface IAttack
{
    string AttackName { get; }
    AttackMethod AttackType { get; }
    float DamageCoefficient { get; }
    bool IsAvailable { get; }

    void ExecuteAttack(MonoCharacter attacker, Transform target);
}
