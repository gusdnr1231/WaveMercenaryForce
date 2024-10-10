using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAction : Action
{
    public SharedPCharacter Player;

    private PlayerAttack _attack;

    public override void OnAwake()
    {
        _attack = Player.Value.GetCompo<PlayerAttack>();
    }

    public override TaskStatus OnUpdate()
    {
        _attack.UseAttack();

        return TaskStatus.Success;
    }
}
