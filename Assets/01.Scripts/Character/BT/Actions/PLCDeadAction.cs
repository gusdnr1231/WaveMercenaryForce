using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom_Player")]
public class PLCDeadAction : Action
{
    public SharedPCharacter Player;
    public AnimatorParamSO DeadAnimParam;

    private PlayerAnimator _animator;

    public override void OnAwake()
    {
        _animator = Player.Value.GetCompo<PlayerAnimator>();
    }

    public override void OnStart()
    {
        Player.Value.ActiveEnd();
    }
    public override TaskStatus OnUpdate()
    {
        _animator.ClearAllBoolen();
        _animator.SetParam(DeadAnimParam, new ParamValue { boolValue = true });
        return TaskStatus.Success;
    }
}
