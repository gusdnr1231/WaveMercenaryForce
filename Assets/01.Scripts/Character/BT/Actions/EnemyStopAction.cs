using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom_Enemy")]
public class EnemyStopAction : Action
{
    public SharedECharacter Enemy;

    private CharacterMovement _movement;

    public override void OnStart()
    {
        if (_movement == null) _movement = Enemy.Value.GetCompo<CharacterMovement>();
    }

    public override TaskStatus OnUpdate()
    {
        _movement.SetStop(true);
        _movement.SetVelocity(Vector3.zero);
        return TaskStatus.Success;
    }
}
