using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType
{
    None = 0,
    Dealer = 1,
    Tanker = 2,
    Supporter = 3,
    Boss = 4
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

public struct CharacterTransform
{
    public Vector2 Position;
}

public abstract class MonoCharacter : MonoBehaviour
{
    [Header("캐릭터 설정")]
    public CharacterType characterType;
    public FightingSpirit characterSpirit;

    [Header("공격 우선도 설정")]
    public List<CharacterType> TypeAttackOrder;

    public CharacterTransform CharacterTrm;

    public CharacterType GetCharacterType() => characterType;
    public bool IsFightSpiritMax() => characterSpirit.CurrentSpirit >= characterSpirit.MaxSpirit;

    public virtual void AddFightingSpirit(int AddAmount)
    {
        characterSpirit.CurrentSpirit += AddAmount;
        characterSpirit.CurrentSpirit = Mathf.Clamp(characterSpirit.CurrentSpirit, 0, characterSpirit.MaxSpirit);
    }

    public virtual void SetCharacterPosition(Vector2 position)
    {
        CharacterTrm.Position = position;
    }
}
