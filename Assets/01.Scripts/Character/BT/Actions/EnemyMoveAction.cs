using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom_Enemy")]
public class EnemyMoveAction : Action
{
    public SharedECharacter Enemy;
    public SharedTransform TargetTrm;

    public float reCalcPeriod = 0.5f;

    private EnemyMovement _movement;
    private float _lastCalcTime;

    public override void OnAwake()
    {
        _lastCalcTime = Time.time;

        _movement = Enemy.Value.GetCompo<EnemyMovement>();
    }

    public override void OnStart()
    {
        _movement.SetStop(false);
        _movement.SetDestination(TargetTrm.Value.position);
        _movement.SetSpeed(Enemy.Value.characterStat.MoveSpeed.StatValue);
    }

    public override TaskStatus OnUpdate()
    {
        if (TargetTrm.Value == null) return TaskStatus.Failure;

        if (_lastCalcTime + reCalcPeriod < Time.time)
        {
            _movement.SetDestination(TargetTrm.Value.position);
            _lastCalcTime = Time.time;
        }

        return TaskStatus.Running;
    }
}
