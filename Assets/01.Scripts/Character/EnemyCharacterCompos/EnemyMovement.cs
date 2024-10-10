using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour, IEnemyComponent
{
    private EnemyCharacter _emc;
    public NavMeshAgent CharacterAgent { get; private set; }

    public void Initilize(EnemyCharacter emc)
    {
        _emc = emc;

        CharacterAgent = GetComponent<NavMeshAgent>();
        CharacterAgent.speed = _emc.characterStat.MoveSpeed.StatValue;
    }

    public void AfterInitilize() { }

    public void SetStop(bool isStop) => CharacterAgent.isStopped = isStop;
    public void SetSpeed(float speed) => CharacterAgent.speed = speed;
    public void SetDestination(Vector3 destination) => CharacterAgent.SetDestination(destination);
    public void SetVelocity(Vector3 velocity) => CharacterAgent.velocity = velocity;
}
