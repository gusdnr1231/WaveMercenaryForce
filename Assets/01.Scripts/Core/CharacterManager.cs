using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : Manager<CharacterManager>
{
    [Header("ĳ���� ������ �ʿ��� ��ҵ�")]
    [SerializeField] private PoolManagerSO PoolManager;
    [SerializeField] private GameEventChannelSO SpawnChannel;

    [Header("������ ĳ���� Pool Type")]
    [SerializeField] private PoolTypeSO PLCPoolType;
    [SerializeField] private PoolTypeSO EMCPoolType;

    public event Action<PlayerCharacter> CreatePlayerCharacter;
    public event Action<EnemyCharacter> CreateEnemyCharacter;

    [Header("ĳ���� ��⼮ ������")]
    public int MaxSeatCount = 10;
    [field:SerializeField] public int CurrentReaminSeatCount { get; private set; }
    [field:SerializeField] private List<Transform> SeatTrms; 
    public bool CanAddCharacter => CurrentReaminSeatCount > 0;
    public Dictionary<int, PlayerCharacter> SeatPair = new Dictionary<int, PlayerCharacter>();

    protected override void Awake()
    {
        base.Awake();

        CurrentReaminSeatCount = MaxSeatCount;
        SeatPair = new Dictionary<int, PlayerCharacter>();

        for (int count = 0; count < MaxSeatCount; count++)
        {
            SeatPair.Add(count, null);
        }

        SpawnChannel.AddListener<PlayerCharacterCreate>(HandlePlayerCharacterCreate);
        SpawnChannel.AddListener<EnemyCharacterCreate>(HandleEnemyCharacterCreate);
    }

    private void HandlePlayerCharacterCreate(PlayerCharacterCreate evt)
    {
        var character = PoolManager.Pop(PLCPoolType) as PlayerCharacter;
        character.SetCharacterData(evt.plcData);
        character.SetCharacterPosition(evt.pos);

        AddCharacterToSeat(character);
    }

    private void HandleEnemyCharacterCreate(EnemyCharacterCreate evt)
    {
        var character = PoolManager.Pop(EMCPoolType) as EnemyCharacter;
        character.SetCharacterData(evt.emcData);
        character.SetCharacterPosition(evt.pos);

        CreateEnemyCharacter?.Invoke(character);
    }

    public bool AddCharacterToSeat(PlayerCharacter addCharacter)
    {
        if (CurrentReaminSeatCount - 1 < 0) return false;

        for (int count = 0; count < MaxSeatCount; count++)
        {
            if (SeatPair[count] == null)
            {
                SeatPair[count] = addCharacter;
                addCharacter.SetCharacterPosition(SeatTrms[count].position);
                CurrentReaminSeatCount--;
                Debug.Log($"{count} : {addCharacter.CharacterDataBase.name}");
                return true;
            }
        }

        return false;
    }

    public bool RemoveCharacterToSeat(int count)
    {
        if (count > MaxSeatCount || count < 0 || SeatPair[count] == null) return false;

        SeatPair[count] = null;
        CurrentReaminSeatCount++;

        return true;
    }
}
