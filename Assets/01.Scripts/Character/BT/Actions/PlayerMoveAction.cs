using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerMoveAction : Action
{
    public SharedPCharacter Player;
    public SharedTransform TargetTrm;

    public float reCalcPeriod = 0.5f;

    private PlayerMovement _movement;
    private float _lastCalcTime;

    public override void OnAwake()
    {
        _lastCalcTime = Time.time;

        _movement = Player.Value.GetCompo<PlayerMovement>();

        _movement.SetSpeed(Player.Value.characterStat.MoveSpeed.StatValue);
    }

    public override void OnStart()
    {
        _movement.SetDestination(TargetTrm.Value.position);
        _movement.SetStop(false);
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
