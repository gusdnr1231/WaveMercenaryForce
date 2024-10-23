using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour, IEnemyComponent
{
    private EnemyCharacter _emc;
    private EnemyAnimator _animator;
    public NavMeshAgent CharacterAgent { get; private set; }

    public event Action<Vector3> MoveToDirection;

    public void Initilize(EnemyCharacter emc)
    {
        _emc = emc;
        _animator = emc.GetCompo<EnemyAnimator>();

        CharacterAgent = GetComponent<NavMeshAgent>();
    }

    public void AfterInitilize()
    {
        _emc.OnSetDefaultData += HandleSetDefaultMoveSpeed;
    }

    private void HandleSetDefaultMoveSpeed()
    {
        CharacterAgent.speed = _emc.characterStat.MoveSpeed.StatValue;
    }

    public void SetStop(bool isStop) => CharacterAgent.isStopped = isStop;

    public void SetDestination(Vector3 destination)
    {
        CharacterAgent.SetDestination(destination);
        MoveToDirection?.Invoke(destination);
    }

    public void SetSpeed(float speed) => CharacterAgent.speed = speed;
    public void SetVelocity(Vector3 velocity) => CharacterAgent.velocity = velocity;
}
