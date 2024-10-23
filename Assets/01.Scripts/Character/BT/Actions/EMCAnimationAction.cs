using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Custom_Enemy")]
public class EMCAnimationAction : Action
{
    public SharedECharacter Enemy;
    public AnimatorParamSO Param;

    private EnemyAnimator _animator;

    public override void OnAwake()
    {
        _animator = Enemy.Value.GetCompo<EnemyAnimator>();
    }

    public override TaskStatus OnUpdate()
    {
        _animator.ClearAllBoolen();
        _animator.SetParam(Param, new ParamValue { boolValue = true });
        return TaskStatus.Success;
    }
}