using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
