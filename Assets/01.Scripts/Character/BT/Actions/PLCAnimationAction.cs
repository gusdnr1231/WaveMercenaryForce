using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TaskCategory("Custom")]
public class PLCAnimationAction : Action
{
    public SharedPCharacter Player;
    public AnimatorParamSO Param;

    private PlayerAnimator _animator;

    public override void OnAwake()
    {
        _animator = Player.Value.GetCompo<PlayerAnimator>();
    }

    public override TaskStatus OnUpdate()
    {
        _animator.ClearAllBoolen();
        _animator.SetParam(Param, new ParamValue{boolValue = true});
        return TaskStatus.Success;
    }
}
