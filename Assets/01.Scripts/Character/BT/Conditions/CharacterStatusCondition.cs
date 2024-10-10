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
    /// ������ ���� ���� ���� ������ ���� Ȯ��
    /// </summary>
    /// <param name="StatusValue">������ ��</param>
    /// <param name="ContainsValue">���� ��</param>
    public override TaskStatus OnUpdate()
    {
        if(StatusValue.Value == ContainsValue) return TaskStatus.Success;
        else return TaskStatus.Failure;
    }
}
