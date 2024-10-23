using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour, IEnemyComponent
{
    [Header("공격 정보")]
    private AttackData attackData;
    private AttackData skillData;

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
        _emc.OnChangeCharacterData += HandleChangeAttackData;

        _animator.OnStartAttackAnim += ActionAttack;
        _animator.OnExcuteAttackAnim += ExcuteAttack;
        _animator.OnEndAttackAnim += EndAttack;
    }

    private void HandleChangeAttackData(EnemyCharacterDataSO data)
    {
        attackData = data.CharacterAttack;
        skillData = data.CharacterSkill;
    }

    #region 기본 공격 부분

    private void ActionAttack() => IsAttack = true;

    private void ExcuteAttack()
    {
        AttackTarget = _emc._target.Value;
        if (AttackTarget != null && _emc != null) attackData.Execute(_emc, AttackTarget);
        _emc.AddFightingSpirit(attackData.addSpirit);
    }

    private void EndAttack()
    {
        IsAttack = false;
        AttackCooldown();
    }

    private void AttackCooldown()
    {
        if (cooldownCoroutine != null) return;

        Debug.Log("Start Attack Cooldown");

        IsCooldown = true;
        float attackCooldownTime = 1f / _emc.characterStat.AttackSpeed.StatValue;

        if (cooldownCoroutine != null) StopCoroutine(cooldownCoroutine);
        cooldownCoroutine = StartCoroutine(ResetCooldownAfterTime(attackCooldownTime));
    }

    private IEnumerator ResetCooldownAfterTime(float cooldownTime)
    {
        Debug.Log("Start Cooldown Coroutine");
        yield return new WaitForSeconds(cooldownTime);
        IsCooldown = false;
        cooldownCoroutine = null;
        Debug.Log("End Cooldown Coroutine");
    }

    #endregion

    public void UseSkill()
    {
        if (AttackTarget == null) return;
        _emc.characterSpirit.CurrentSpirit = 0;
        skillData.Execute(_emc, AttackTarget);
    }
}
