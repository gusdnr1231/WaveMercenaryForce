using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TaskCategory("Custom_Player")]
public class PlayerStopAction : Action
{
    public SharedPCharacter Player;

    private CharacterMovement _movement;

    public override void OnStart()
    {
        if (_movement == null) _movement = Player.Value.GetCompo<CharacterMovement>();
    }

    public override TaskStatus OnUpdate()
    {
        _movement.SetStop(true);
        _movement.SetVelocity(Vector3.zero);
        return TaskStatus.Success;
    }
}
