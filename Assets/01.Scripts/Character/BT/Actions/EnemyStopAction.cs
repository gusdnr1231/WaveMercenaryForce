using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom_Enemy")]
public class EnemyStopAction : Action
{
    public SharedECharacter Enemy;

    private EnemyMovement _movement;

    public override void OnStart()
    {
        if (_movement == null) _movement = Enemy.Value.GetCompo<EnemyMovement>();
    }

    public override TaskStatus OnUpdate()
    {
        _movement.SetStop(true);
        _movement.SetVelocity(Vector3.zero);
        return TaskStatus.Success;
    }
}
