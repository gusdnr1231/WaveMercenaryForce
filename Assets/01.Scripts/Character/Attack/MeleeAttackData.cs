using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeAttack", menuName = "Attack/MeleeAttack")]
public class MeleeAttackData : AttackData
{
    [Header("일반 공격 정보")]
    [Tooltip("공격 계수 정보")]
    [Range(0f, 100f)] public float damageCoefficient;

    public override void Execute(MonoCharacter attacker, Transform target)
    {
        if (target.TryGetComponent(out IDamageable attackTarget))
        {
            if (attacker.characterStat != null)
            {
                float baseValue = attacker.characterStat.ReturnStatValue(AttackBy);
                float damage = CalculateDamage(baseValue);
                attackTarget.TakeDamage(AttackBy, damage);
                Debug.Log($"{attacker.name} dealt {damage} damage to {target.name}");
            }
            else
            {
                Debug.LogError("attacker.characterStat is null in MeleeAttackData.Execute.");
            }
        }

        if (effectPrefab != null)
        {
            //추후 풀매니저로 변경
            Instantiate(effectPrefab, target.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Attack Effect is null");
        }

        Debug.Log($"{attacker.name} performed Melee Attack on {target.name}");
    }

    /// <summary>
    /// 기본 능력치 수치를 받아오고, 그 수치에 계수를 곱한 값을 반환 (소수점 3자리까지)
    /// </summary>
    /// <param name="baseValue">기본 능력치 수치를 받아옴</param>
    private float CalculateDamage(float baseValue)
    {
        float damage = 0;
        damage = baseValue * damageCoefficient;
        return (int)Math.Round(damage, 3);
    }
}
