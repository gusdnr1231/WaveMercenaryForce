using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AttackCoefficientCollection
{
    public Stat AttackType;
    public float DamageCoefficient;
}

public interface ISkill
{
    string SkillName { get; }
    AttackCoefficientCollection AttackCollection { get; }
    float Range { get; }
    float Damage { get; }

    void Execute(MonoCharacter attacker, Transform target);
}

