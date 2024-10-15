using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeAttack", menuName = "Attack/MeleeAttack")]
public class MeleeAttackData : AttackData
{
    [Header("�Ϲ� ���� ����")]
    [Tooltip("���� ��� ����")]
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
            //���� Ǯ�Ŵ����� ����
            Instantiate(effectPrefab, target.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Attack Effect is null");
        }

        Debug.Log($"{attacker.name} performed Melee Attack on {target.name}");
    }

    /// <summary>
    /// �⺻ �ɷ�ġ ��ġ�� �޾ƿ���, �� ��ġ�� ����� ���� ���� ��ȯ (�Ҽ��� 3�ڸ�����)
    /// </summary>
    /// <param name="baseValue">�⺻ �ɷ�ġ ��ġ�� �޾ƿ�</param>
    private float CalculateDamage(float baseValue)
    {
        float damage = 0;
        damage = baseValue * damageCoefficient;
        return (int)Math.Round(damage, 3);
    }
}
