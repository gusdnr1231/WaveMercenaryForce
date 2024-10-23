using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom_Enemy")]
public class EnemyAttackCondition : Conditional
{
    public SharedECharacter Enemy;

    private EnemyAttack _attack;

    public override void OnAwake()
    {
        if (Enemy.Value != null)
        {
            _attack = Enemy.Value.GetCompo<EnemyAttack>();
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (_attack == null)
        {
            return TaskStatus.Failure;
        }

        if (_attack.CanAttack == true)
        {
            return TaskStatus.Success;
        }
        else if (_attack.CanAttack == false)
        {
            return TaskStatus.Failure;
        }

        return TaskStatus.Running;
    }
}
