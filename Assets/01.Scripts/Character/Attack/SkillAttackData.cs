using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkillAttack", menuName = "Attack/SkillAttack")]
public class SkillAttackData : AttackData
{
    [Header("��ų ���� ����")]
    public List<AttackCoefficientCollection> AttackCollection;
    public GameObject skillEffectPrefab;  // ��ų ����Ʈ ������
    public float skillDuration = 3f;      // ��ų ���� �ð�

    public bool IsSingleTarget;
    [Range(1, 100)]public int MaxTargetCount;

    public override void Execute(MonoCharacter attacker, Transform target)
    {
        //�� ȿ�� �κ�
    }
    
    /// <summary>
    /// �������� �ɷ�ġ�� ����� �������� ��ų�� ������ �����
    /// </summary>
    /// <param name="attacker">������ �ޱ� ���� �����ڸ� �޾ƿ�</param>
    private float CalculateValue(MonoCharacter attacker)
    {
        float totalDamage = 0f;

        AttackCoefficientCollection collectionData;

        for (int count = 0; count < AttackCollection.Count; count++)
        {
            collectionData = AttackCollection[count];
            totalDamage += attacker.characterStat.ReturnStatValue(collectionData.AttackType) * collectionData.DamageCoefficient;
        }

        return totalDamage;
    }
}
