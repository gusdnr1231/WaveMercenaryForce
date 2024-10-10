using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatusCondition : Conditional
{
    public SharedBool StatusValue;
    public bool ContainsValue = true;

    /// <summary>
    /// 감지할 값이 비교할 값과 같은지 상태 확인
    /// </summary>
    /// <param name="StatusValue">감지할 값</param>
    /// <param name="ContainsValue">비교할 값</param>
    public override TaskStatus OnUpdate()
    {
        if(StatusValue.Value == ContainsValue) return TaskStatus.Success;
        else return TaskStatus.Failure;
    }
}
