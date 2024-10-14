using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour, IPlayerComponent
{
    private readonly int _idleHash = Animator.StringToHash("Idle");
    private readonly int _moveHash = Animator.StringToHash("Move");
    private readonly int _attackTriggerHash = Animator.StringToHash("Attack");
    private readonly int _deadTriggerHash = Animator.StringToHash("Dead");

    private PlayerCharacter _plc;
    private PlayerMovement _movement;
    private Animator _animator;
    public Animator Animator => _animator;

    public event Action OnStartAttackAnim;
    public event Action OnExcuteAttackAnim;

    public void Initilize(PlayerCharacter plc)
    {
        _plc = plc;
        _movement = plc.GetCompo<PlayerMovement>();

        _animator = GetComponent<Animator>();
    }

    public void AfterInitilize()
    {
        _plc.OnDeadEvent += HandleDeadEvent;
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
        _plc.OnAnimationEnd();
    }

    private void ExcuteAttack()
    {
        OnExcuteAttackAnim?.Invoke();
    }
}
