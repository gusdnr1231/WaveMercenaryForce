using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour, IEnemyComponent
{
    [Header("공격 정보 (추후 캐릭터 데이터로 전부 합칠 예정)")]
    public AttackData attackData;
    public AttackData skillData;

    private Transform AttackTarget;

    private EnemyCharacter _emc;
    private EnemyAnimator _animator;

    private Coroutine cooldownCoroutine;

    public bool IsAttack { get; private set; } = false;
    public bool IsCooldown { get; private set; } = false;
    public bool CanAttack => IsAttack == false && IsCooldown == false;

    public void Initilize(EnemyCharacter emc)
    {
        _emc = emc;
        _animator = emc.GetCompo<EnemyAnimator>();
    }

    public void AfterInitilize()
    {
        throw new NotImplementedException();
    }

    #region 기본 공격 부분

    public void ActionAttack() => IsAttack = true;

    private void ExcuteAttack()
    {
        AttackTarget = _emc._target.Value;
        if (AttackTarget != null) attackData.Execute(_emc, AttackTarget);

        IsAttack = false;
    }

    public void AttackCooldown()
    {
        if (!CanAttack || IsCooldown) return;
        if (cooldownCoroutine != null) return;

        IsCooldown = true;
        float attackCooldownTime = 1f / _emc.characterStat.AttackSpeed.StatValue;


        cooldownCoroutine = StartCoroutine(ResetCooldownAfterTime(attackCooldownTime));
    }

    private IEnumerator ResetCooldownAfterTime(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        IsCooldown = false;
    }

    #endregion

    public void UseSkill()
    {
        if (AttackTarget == null) return;
        _emc.characterSpirit.CurrentSpirit = 0;
        skillData.Execute(_emc, AttackTarget);
    }
}
