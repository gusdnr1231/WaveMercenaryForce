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
        if (Target.Value != null) return TaskStatus.Success;
        else return TaskStatus.Failure;
    }
}
