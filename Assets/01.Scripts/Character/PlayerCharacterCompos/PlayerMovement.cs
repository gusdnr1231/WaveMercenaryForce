using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour, IPlayerComponent
{
    private PlayerCharacter _plc;
    public NavMeshAgent CharacterAgent { get; private set; }

    public void Initilize(PlayerCharacter plc)
    {
        _plc = plc;

        CharacterAgent = GetComponent<NavMeshAgent>();
        CharacterAgent.speed = _plc.characterStat.MoveSpeed.StatValue;
    }

    public void AfterInitilize() { }

    public void SetStop(bool isStop) => CharacterAgent.isStopped = isStop;
    public void SetSpeed(float speed) => CharacterAgent.speed = speed;
    public void SetDestination(Vector3 destination) => CharacterAgent.SetDestination(destination);
    public void SetVelocity(Vector3 velocity) => CharacterAgent.velocity = velocity;
}
