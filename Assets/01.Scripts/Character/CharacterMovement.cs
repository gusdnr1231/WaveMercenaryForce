using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour, IEnemyComponent, IPlayerComponent
{
    private MonoCharacter _character;
    private MonoChacarterAnimator _animator;
    public NavMeshAgent CharacterAgent { get; private set; }

    public event Action<Vector3> MoveToDirection;

    public void Initilize(PlayerCharacter plc)
    {
        _character = plc;
        _animator = plc.GetCompo<PlayerAnimator>();

        CharacterAgent = GetComponent<NavMeshAgent>();

        SetStopAgent();
    }

    public void Initilize(EnemyCharacter emc)
    {
        _character = emc;
        _animator = emc.GetCompo<EnemyAnimator>();

        CharacterAgent = GetComponent<NavMeshAgent>();

        SetStopAgent();
    }

    public void AfterInitilize()
    {
        if(_character is ICharacterEvents characterEvts)
        {
            characterEvts.OnSetDefaultData += HandleSetDefaultMoveSpeed;

            characterEvts.OnStartCharacter += HandleStartCharacter;
            characterEvts.OnEndCharacter += HandleEndCharacter;
        }
        else
        {
            Debug.LogError("Character Event is null! Not Initialize ICharacterEvents");
        }
    }

    private void HandleStartCharacter()
    {
        if (CharacterAgent == null) CharacterAgent = GetComponent<NavMeshAgent>();

        CharacterAgent.enabled = true;
        SetStop(false);
    }

    private void HandleEndCharacter()
    {
        if (CharacterAgent == null) CharacterAgent = GetComponent<NavMeshAgent>();

        SetStopAgent();
    }

    private void HandleSetDefaultMoveSpeed()
    {
        CharacterAgent.speed = _character.characterStat.MoveSpeed.StatValue;
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

    private void SetStopAgent()
    {
        SetStop(true);
        SetVelocity(Vector3.zero);
        CharacterAgent.enabled = false;
    }
}
