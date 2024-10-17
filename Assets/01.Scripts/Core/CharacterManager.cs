using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("ĳ���� ������ �ʿ��� ��ҵ�")]
    [SerializeField] private PoolManagerSO PoolManager;
    [SerializeField] private GameEventChannelSO SpawnChannel;

    [Header("������ Pool Type")]
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
