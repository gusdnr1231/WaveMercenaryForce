using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom_Enemy")]
public class EMCDeadAction : Action
{
    public SharedECharacter Enemy;
    public AnimatorParamSO DeadAnimParam;

    private EnemyAnimator _animator;

    public override void OnAwake()
    {
        _animator = Enemy.Value.GetCompo<EnemyAnimator>();
    }

    public override void OnStart()
    {
        Enemy.Value.ActiveEnd();
    }
    public override TaskStatus OnUpdate()
    {
        _animator.ClearAllBoolen();
        _animator.SetParam(DeadAnimParam, new ParamValue { boolValue = true });
        return TaskStatus.Success;
    }
}
