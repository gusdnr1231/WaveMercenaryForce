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
    [Tooltip("���� ���۽� �⺻ ����")]
    public int DefaultSpirit;
    [Tooltip("���� �� ����Ǵ� ����")]
    public int CurrentSpirit;
    [Tooltip("ĳ���� �ִ� ����")]
    public int MaxSpirit;
}

public abstract class MonoCharacter : MonoBehaviour
{
    [Header("ĳ���� ����")]
    [Tooltip("ĳ���� �̸�")]
    public string CharacterName;
    [Tooltip("ĳ���� �ɷ�ġ ����")]
    public CharacterStat characterStat;
    [Tooltip("ĳ���� Ÿ��")]
    public CharacterType characterType;
    [Tooltip("ĳ���� ���õ�")]
    [Range(0, 2)]public int CharacterProficiency = 0;
    [Tooltip("ĳ���� ���")]
    public CharacterGrade characterGrade;
    public FightingSpirit characterSpirit;

    [HideInInspector] public SharedBool _isAlive;
    public LocationInfo CharacterLocation;
    
    //���� ü��
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