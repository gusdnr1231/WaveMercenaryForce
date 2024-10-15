using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, IPlayerComponent
{
    public AttackData attack;
    public List<AttackData> skills = new List<AttackData>();

    private Transform AttackTarget;

    private PlayerCharacter _plc;
    private PlayerAnimator _animator;

    private Coroutine cooldownCoroutine;
   
    public bool IsAttack { get; private set; } = false;
    public bool IsCooldown { get; private set; } = false;
    public bool CanAttack => IsAttack == false && IsCooldown == false;

    public void Initilize(PlayerCharacter plc)
    {
        _plc = plc;
        _animator = plc.GetCompo<PlayerAnimator>();
    }

    public void AfterInitilize()
    {
        _animator.OnStartAttackAnim += ActionAttack;
        _animator.OnExcuteAttackAnim += ExcuteAttack;
        _animator.OnEndAttackAnim += EndAttack;
    }

    #region 기본 공격 부분

    private void ActionAttack() => IsAttack = true;
    
    private void ExcuteAttack()
    {
        AttackTarget = _plc._target.Value;
        if (AttackTarget != null && _plc != null) attack.Execute(_plc, AttackTarget);
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
        float attackCooldownTime = 1f / _plc.characterStat.AttackSpeed.StatValue;

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

    public void UseSkill(int useIndex)
    {
        if(AttackTarget == null) return;
        AttackData useSkill = skills[useIndex];
        useSkill.Execute(_plc, AttackTarget);
    }
}
