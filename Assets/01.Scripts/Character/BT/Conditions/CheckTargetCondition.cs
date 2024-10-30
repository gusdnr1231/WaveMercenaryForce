using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom")]
public class CheckTargetCondition : Conditional
{
    public SharedTransform Target;

    /// <summary>
    /// 타겟을 감지했는지 확인
    /// 타겟을 감지 했다면 성공 / 타겟을 감지하지 못했다면 실패
    /// </summary>
    public override TaskStatus OnUpdate()
    {
        if (Target.Value == null) return TaskStatus.Failure;
        if (Target.Value.TryGetComponent(out MonoCharacter _character) == false) return TaskStatus.Failure;
        if (_character._isAlive.Value == false) return TaskStatus.Failure;

        return TaskStatus.Success;
    }
}
