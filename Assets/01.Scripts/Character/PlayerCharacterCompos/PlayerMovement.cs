using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour, IPlayerComponent
{
    private PlayerCharacter _plc;
    private PlayerAnimator _animator;
    public NavMeshAgent CharacterAgent { get; private set; }

    public event Action<Vector3> MoveToDirection;

    public void Initilize(PlayerCharacter plc)
    {
        _plc = plc;
        _animator = plc.GetCompo<PlayerAnimator>();

        CharacterAgent = GetComponent<NavMeshAgent>();

        CharacterAgent.enabled = false;
        SetStop(true);
        SetVelocity(Vector3.zero);
    }

    public void AfterInitilize()
    {
        _plc.OnSetDefaultData += HandleSetDefaultMoveSpeed;

        _plc.OnStartCharacter += HandleStartCharacter;
        _plc.OnEndCharacter += HandleEndCharacter;
    }

    private void HandleStartCharacter()
    {
        if (CharacterAgent == null) CharacterAgent = GetComponent<NavMeshAgent>();

        CharacterAgent.enabled = true;
    }

    private void HandleEndCharacter()
    {
        if (CharacterAgent == null) CharacterAgent = GetComponent<NavMeshAgent>();

        CharacterAgent.enabled = false;
        SetStop(true);
        SetVelocity(Vector3.zero);
    }

    private void HandleSetDefaultMoveSpeed()
    {
        CharacterAgent.speed = _plc.characterStat.MoveSpeed.StatValue;
    }

    public void SetStop(bool isStop) => CharacterAgent.isStopped = isStop;

    public void SetDestination(Vector3 destination)
    {
        if (CharacterAgent.isOnNavMesh)
        {
            CharacterAgent.SetDestination(destination);
            MoveToDirection?.Invoke(destination);
        }
        else
        {
            Debug.LogWarning("NavMeshAgent is not on a valid NavMesh!");
        }
    }

    public void SetSpeed(float speed) => CharacterAgent.speed = speed;
    public void SetVelocity(Vector3 velocity) => CharacterAgent.velocity = velocity;
}
