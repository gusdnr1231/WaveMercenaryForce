using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour, IEnemyComponent
{
    private readonly int _idleHash = Animator.StringToHash("Idle");
    private readonly int _moveHash = Animator.StringToHash("Move");
    private readonly int _attackTriggerHash = Animator.StringToHash("Attack");
    private readonly int _deadTriggerHash = Animator.StringToHash("Dead");

    private EnemyCharacter _emc;
    private EnemyMovement _movement;
    private Animator _animator;
    public Animator Animator => _animator;

    public event Action OnStartAttackAnim;
    public event Action OnExcuteAttackAnim;

    public void Initilize(EnemyCharacter emc)
    {
        _emc = emc;
        _movement = emc.GetCompo<EnemyMovement>();

        _animator = GetComponent<Animator>();
    }

    public void AfterInitilize()
    {
        _emc.OnDeadEvent += HandleDeadEvent;
        _movement.IsStopCharacter += HandleMovementEvent;
    }

    private void HandleMovementEvent(bool isStop)
    {
        _animator.SetBool(_idleHash, isStop);
        _animator.SetBool(_moveHash, !isStop);
    }

    public void ActionAttack()
    {
        _animator.SetTrigger(_attackTriggerHash);
    }

    private void HandleDeadEvent()
    {
        _animator.SetTrigger(_deadTriggerHash);
    }

    private void AnimationEnd()
    {
        _emc.OnAnimationEnd();
    }

    private void ExcuteAttack()
    {
        OnExcuteAttackAnim?.Invoke();
    }
}
