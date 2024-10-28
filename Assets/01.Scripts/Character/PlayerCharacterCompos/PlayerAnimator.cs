using System;
using UnityEngine;

public class PlayerAnimator : MonoChacarterAnimator, IPlayerComponent
{
    private PlayerCharacter _plc;
    private CharacterMovement _movement;
    private PlayerAttack _attack;

    public event Action OnStartAttackAnim;
    public event Action OnExcuteAttackAnim;
    public event Action OnEndAttackAnim;

    public void Initilize(PlayerCharacter plc)
    {
        _plc = plc;
        _character = plc;

        _movement = plc.GetCompo<CharacterMovement>();
        _attack = plc.GetCompo<PlayerAttack>();

        _animator = GetComponent<Animator>();
    }

    public void AfterInitilize()
    {
        _plc.OnChangeCharacterData += HandleChangeAnimController;
    }

    private void HandleChangeAnimController(PlayerCharacterDataSO initData)
    {
        if (_animator != null) _animator.runtimeAnimatorController = initData.CharacterAnimator;
    }

    //애니메이션 이벤트로 실행되는 함수 (공격 시작 타이밍)
    private void StartAttack()
    {
        Debug.Log("Start Attack");
        OnStartAttackAnim?.Invoke();
    }

    //애니메이션 이벤트로 실행되는 함수 (실 공격 타이밍)
    private void ExcuteAttack()
    {
        Debug.Log("Excute Attack");
        OnExcuteAttackAnim?.Invoke();
    }

    //애니메이션 이벤트로 실행되는 함수 (공격 종료 타이밍)
    private void EndAttack()
    {
        Debug.Log("End Attack");
        OnEndAttackAnim?.Invoke();
        AnimationEnd();
    }
}
