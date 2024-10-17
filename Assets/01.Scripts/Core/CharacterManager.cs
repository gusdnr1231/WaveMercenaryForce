using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("캐릭터 생성에 필요한 요소들")]
    [SerializeField] private PoolManagerSO PoolManager;
    [SerializeField] private GameEventChannelSO SpawnChannel;

    [Header("생성할 Pool Type")]
    [SerializeField] private PoolTypeSO PLCPoolType;
    [SerializeField] private PoolTypeSO EMCPoolType;

    private void Awake()
    {
        SpawnChannel.AddListener<PlayerCharacterCreate>(HandlePlayerCharacterCreate);
        SpawnChannel.AddListener<EnemyCharacterCreate>(HandleEnemyCharacterCreate);
    }

    private void HandlePlayerCharacterCreate(PlayerCharacterCreate evt)
    {
        var character = PoolManager.Pop(PLCPoolType) as PlayerCharacter;
        character.SetCharacterData(evt.plcData);
        character.SetCharacterPosition(evt.pos);
    }

    private void HandleEnemyCharacterCreate(EnemyCharacterCreate evt)
    {
        var character = PoolManager.Pop(EMCPoolType) as EnemyCharacter;
        character.SetCharacterData(evt.emcData);
        character.SetCharacterPosition(evt.pos);
    }

}
