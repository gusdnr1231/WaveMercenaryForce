using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom")]
public class InDistanceCondition : Conditional
{
    public SharedTransform Target;
    public SharedFloat Range;

    public override TaskStatus OnUpdate()
    {
        Vector3 position = Target.Value.position;

        float distance = Vector3.Distance(position, transform.position);
        if (distance <= Range.Value) return TaskStatus.Success;
        else return TaskStatus.Failure;
    }
}
