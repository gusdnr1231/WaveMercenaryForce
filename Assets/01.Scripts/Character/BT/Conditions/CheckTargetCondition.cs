using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTargetCondition : Conditional
{
    public SharedTransform Target;

    /// <summary>
    /// 타겟을 감지했는지 확인
    /// 타겟을 감지 했다면 성공 / 타겟을 감지하지 못했다면 실패
    /// </summary>
    public override TaskStatus OnUpdate()
    {
        if (Target.Value != null) return TaskStatus.Success;
        else return TaskStatus.Failure;
    }
}
