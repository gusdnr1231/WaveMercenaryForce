using BehaviorDesigner.Runtime;
using System;
using UnityEngine;

public enum CharacterType
{
    None = 0,
    Dealer = 1,
    Tanker = 2,
    Supporter = 3,
    Boss = 4
}

public enum CharacterGrade
{
    None = 0,
    D = 1,
    C = 2,
    B = 3,
    A = 4,
    S = 5
}

[System.Serializable]
public struct FightingSpirit
{
    [Tooltip("전투 시작시 기본 투지")]
    public int DefaultSpirit;
    [Tooltip("전투 중 변경되는 투지")]
    public int CurrentSpirit;
    [Tooltip("캐릭터 최대 투지")]
    public int MaxSpirit;
}

public abstract class MonoCharacter : MonoBehaviour
{
    [Header("캐릭터 설정")]
    [Tooltip("캐릭터 이름")]
    public string CharacterName;
    [Tooltip("캐릭터 능력치 정보")]
    public CharacterStat characterStat;
    [Tooltip("캐릭터 타입")]
    public CharacterType characterType;
    [Tooltip("캐릭터 숙련도")]
    [Range(0, 2)]public int CharacterProficiency = 0;
    [Tooltip("캐릭터 등급")]
    public CharacterGrade characterGrade;
    public FightingSpirit characterSpirit;

    [HideInInspector] public SharedBool _isAlive;
    public LocationInfo CharacterLocation;
    
    //현재 체력
    protected float currentHp;

    public CharacterType GetCharacterType() => characterType;
    public bool IsFightSpiritMax() => characterSpirit.CurrentSpirit >= characterSpirit.MaxSpirit;

    public virtual void AddFightingSpirit(int AddAmount)
    {
        characterSpirit.CurrentSpirit += AddAmount;
        characterSpirit.CurrentSpirit = Mathf.Clamp(characterSpirit.CurrentSpirit, 0, characterSpirit.MaxSpirit);
    }
    
    public virtual void SetCharacterPosition(Vector3 position)
    {
        CharacterLocation.Position = position;
        CharacterLocation.Rotation = Quaternion.identity;
        transform.position = position;
    }
}

public class SharedPCharacter : SharedVariable<PlayerCharacter>
{
    public static implicit operator SharedPCharacter(PlayerCharacter value)
    {
        return new SharedPCharacter { Value = value };
    }
}

public class SharedECharacter : SharedVariable<EnemyCharacter>
{
    public static implicit operator SharedECharacter(EnemyCharacter value)
    {
        return new SharedECharacter { Value = value };
    }
}