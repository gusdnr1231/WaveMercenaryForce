using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom")]
public class PlayerAttackAction : Action
{
    public SharedPCharacter Player;

    private PlayerAttack _attack;

    public override void OnAwake()
    {
        _attack = Player.Value.GetCompo<PlayerAttack>();
    }

    public override void OnStart()
    {
        _attack.ActionAttack();
    }

    public override TaskStatus OnUpdate()
    {
        if (_attack.IsAttack == false)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
