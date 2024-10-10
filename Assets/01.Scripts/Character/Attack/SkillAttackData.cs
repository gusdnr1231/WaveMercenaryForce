using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkillAttack", menuName = "Attack/SkillAttack")]
public class SkillAttackData : AttackData
{
    [Header("스킬 공격 정보")]
    public List<AttackCoefficientCollection> AttackCollection;
    public GameObject skillEffectPrefab;  // 스킬 이펙트 프리팹
    public float skillDuration = 3f;      // 스킬 지속 시간

    public bool IsSingleTarget;
    [Range(1, 100)]public int MaxTargetCount;

    public override void Execute(MonoCharacter attacker, Transform target)
    {
        //실 효과 부분
    }
    
    /// <summary>
    /// 공격자의 능력치와 계수를 바탕으로 스킬의 위력을 계산함
    /// </summary>
    /// <param name="attacker">스텟을 받기 위해 공격자를 받아옴</param>
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
