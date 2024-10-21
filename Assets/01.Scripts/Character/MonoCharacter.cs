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

public abstract class MonoCharacter : MonoBehaviour, IPoolable
{
    [field: SerializeField] public PoolTypeSO PoolType { get; private set; }
    public GameObject GameObject => gameObject;
    protected Pool _characterPool;

    [field:Header("ĳ���� ����")]
    [field:SerializeField]
    public BaseCharacterDataSO CharacterDataBase {  get; private set; }
    [Tooltip("ĳ���� ���õ�")]
    [Range(0, 2)] public int CharacterProficiency = 0;

    [HideInInspector] public FightingSpirit characterSpirit;
    [HideInInspector] public CharacterStat characterStat;
    [HideInInspector] public LocationInfo CharacterLocation;

    [HideInInspector] public SharedBool _isAlive;
    protected SharedBool _isAnimationEnd;
    
    //���� ü��
    protected float currentHp;

    public CharacterType GetCharacterType() => CharacterDataBase.TypeData;
    public bool IsFightSpiritMax() => characterSpirit.CurrentSpirit >= characterSpirit.MaxSpirit;

    public virtual void AddFightingSpirit(int AddAmount)
    {
        characterSpirit.CurrentSpirit += AddAmount;
        characterSpirit.CurrentSpirit = Mathf.Clamp(characterSpirit.CurrentSpirit, 0, characterSpirit.MaxSpirit);
    }
    
    public virtual void SetCharacterPosition(Vector3 position)
    {
        CharacterLocation.Position = position;
        CharacterLocation.Rotation = Vector3.zero;
        transform.position = position;
    }

    public virtual void OnAnimationEnd()
    {
        _isAnimationEnd.Value = true;
    }

    protected virtual void GetDataFromDataBase()
    {
        characterStat = CharacterDataBase.StatusData;
        characterSpirit = CharacterDataBase.CharacterSpirit;
    }

    public virtual void SetCharacterData(BaseCharacterDataSO initData)
    {
        CharacterDataBase = initData;
    }

    #region IPoolable Methods

    public virtual void SetUpPool(Pool pool)
    {
        _characterPool = pool;
    }

    public abstract void ResetItem();

    #endregion
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