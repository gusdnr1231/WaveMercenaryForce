using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom_Player")]
public class PlayerMoveAction : Action
{
    public SharedPCharacter Player;
    public SharedTransform TargetTrm;

    public float reCalcPeriod = 0.5f;

    private CharacterMovement _movement;
    private float _lastCalcTime;

    public override void OnAwake()
    {
        _lastCalcTime = Time.time;

        _movement = Player.Value.GetCompo<CharacterMovement>();
    }

    public override void OnStart()
    {
        _movement.SetStop(false);
        _movement.SetSpeed(Player.Value.characterStat.MoveSpeed.StatValue);
        _movement.SetDestination(TargetTrm.Value.position);
    }

    public override TaskStatus OnUpdate()
    {
        if(TargetTrm.Value == null) return TaskStatus.Failure;

        if (_lastCalcTime + reCalcPeriod < Time.time)
        {
            _movement.SetDestination(TargetTrm.Value.position);
            _lastCalcTime = Time.time;
        }

        return TaskStatus.Running;
    }
}
