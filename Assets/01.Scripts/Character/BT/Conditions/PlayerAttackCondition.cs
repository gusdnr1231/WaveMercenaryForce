using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom")]
public class PlayerAttackCondition : Conditional
{
    public SharedPCharacter Player;

    private PlayerAttack _attack;

    public override void OnAwake()
    {
        if (Player.Value != null)
        {
            _attack = Player.Value.GetCompo<PlayerAttack>();
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (_attack == null)
        {
            return TaskStatus.Failure;
        }

        if (_attack.CanAttack)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
