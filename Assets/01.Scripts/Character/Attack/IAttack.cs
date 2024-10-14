using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    string AttackName { get; }
    float DamageCoefficient { get; }
    bool IsAvailable { get; }

    void ExecuteAttack(MonoCharacter attacker, Transform target);
}
