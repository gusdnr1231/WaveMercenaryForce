using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour, IEnemyComponent
{
    [Header("공격 정보 (추후 캐릭터 데이터로 전부 합칠 예정)")]
    public List<AttackData> attackDataList; // 에디터에서 할당할 공격 데이터 리스트
    private IAttack attack;
    private List<ISkill> skills = new List<ISkill>();

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

    public void ActionAttack()
    {
        if (!CanAttack) return;

        IsAttack = true;
        _animator.ActionAttack();
    }

    private void ExcuteAttack()
    {
        AttackTarget = _emc._target.Value;
        if (AttackTarget != null) attack.ExecuteAttack(_emc, AttackTarget);

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

    public void UseSkill(int useIndex)
    {
        if (AttackTarget == null) return;
        ISkill useSkill = skills[useIndex];
        useSkill.Execute(_emc, AttackTarget);
    }
}
