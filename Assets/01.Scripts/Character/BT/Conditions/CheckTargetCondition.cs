using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom")]
public class CheckTargetCondition : Conditional
{
    public SharedTransform Target;

    /// <summary>
    /// Ÿ���� �����ߴ��� Ȯ��
    /// Ÿ���� ���� �ߴٸ� ���� / Ÿ���� �������� ���ߴٸ� ����
    /// </summary>
    public override TaskStatus OnUpdate()
    {
        if (Target.Value == null) return TaskStatus.Failure;
        if (Target.Value.TryGetComponent(out MonoCharacter _character) == false) return TaskStatus.Failure;
        if (_character._isAlive.Value == false) return TaskStatus.Failure;

        return TaskStatus.Success;
    }
}
