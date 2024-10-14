using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAttack : MonoBehaviour, IPlayerComponent
{
    public List<AttackData> attackDataList; // 에디터에서 할당할 공격 데이터 리스트
    private IAttack attack;
    private List<ISkill> skills = new List<ISkill>();

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
        _animator.OnExcuteAttackAnim += ExcuteAttack;
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
        AttackTarget = _plc._target.Value;
        if (AttackTarget != null) attack.ExecuteAttack(_plc, AttackTarget);

        IsAttack = false;
    }

    public void AttackCooldown()
    {
        if (!CanAttack || IsCooldown) return;
        if (cooldownCoroutine != null) return;

        IsCooldown = true;
        float attackCooldownTime = 1f / _plc.characterStat.AttackSpeed.StatValue;


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
        if(AttackTarget == null) return;
        ISkill useSkill = skills[useIndex];
        useSkill.Execute(_plc, AttackTarget);
    }
}
