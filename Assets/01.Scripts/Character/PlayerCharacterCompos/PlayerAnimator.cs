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

    //�ִϸ��̼� �̺�Ʈ�� ����Ǵ� �Լ� (���� ���� Ÿ�̹�)
    private void StartAttack()
    {
        Debug.Log("Start Attack");
        OnStartAttackAnim?.Invoke();
    }

    //�ִϸ��̼� �̺�Ʈ�� ����Ǵ� �Լ� (�� ���� Ÿ�̹�)
    private void ExcuteAttack()
    {
        Debug.Log("Excute Attack");
        OnExcuteAttackAnim?.Invoke();
    }

    //�ִϸ��̼� �̺�Ʈ�� ����Ǵ� �Լ� (���� ���� Ÿ�̹�)
    private void EndAttack()
    {
        Debug.Log("End Attack");
        OnEndAttackAnim?.Invoke();
        AnimationEnd();
    }
}
