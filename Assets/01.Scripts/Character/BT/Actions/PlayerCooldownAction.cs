using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCooldownAction : Action
{
    public SharedPCharacter Player;

    public override void OnStart()
    {
        Player.Value.GetCompo<PlayerAttack>().AttackCooldown();
    }
}
