using BehaviorDesigner.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyCharacter : MonoCharacter, IDamageable
{
    [Space]
    [Header("적 캐릭터 요소")]
    public PlayerCharacter AttackEnemy;

    #region Event Actions
    public event Action<float> OnHpChange;
    public event Action StartCharacter;
    public event Action OnDeadEvent;
    #endregion

    #region BT Values
    private BehaviorTree _tree;
    [HideInInspector] public SharedTransform _target;
    [HideInInspector] public SharedFloat _range;
    private SharedBool _isSpiritMax;
    #endregion

    protected Dictionary<Type, IEnemyComponent> _components;

    private void Awake()
    {
        SetDefaultData();

        _components = new Dictionary<Type, IEnemyComponent>();
        GetComponentsInChildren<IEnemyComponent>().ToList().ForEach(compo => _components.Add(compo.GetType(), compo));

        _components.Values.ToList().ForEach(compo => compo.Initilize(this));

        _components.Values.ToList().ForEach(compo => compo.AfterInitilize());

        _tree = GetComponent<BehaviorTree>();
        _tree.SetVariableValue("emc", this);
        _tree.SetVariableValue("range", characterStat.MoveSpeed.StatValue);

        _isAlive = _tree.GetVariable("isAlive") as SharedBool;
        _target = _tree.GetVariable("target") as SharedTransform;
        _isSpiritMax = _tree.GetVariable("isSpiritMax") as SharedBool;
    }

    public T GetCompo<T>() where T : class
    {
        if (_components.TryGetValue(typeof(T), out IEnemyComponent compo))
        {
            return compo as T;
        }

        return default;
    }

    public override void OnAnimationEnd()
    {
    }

    private void SetDefaultData()
    {
        _isAlive = true;
        characterStat.SetValues(CharacterProficiency);
        currentHp = characterStat.MaxHp.StatValue;
        characterSpirit.CurrentSpirit = characterSpirit.DefaultSpirit;
    }

    public override void AddFightingSpirit(int AddAmount)
    {
        base.AddFightingSpirit(AddAmount);
    }

    #region IDamageable Methods

    public void TakeDamage(Stat AttackType, float Value)
    {
        float reductionAmount = 0f;

        switch (AttackType)
        {
            case Stat.Strength:
                if (characterStat.Defense == null)
                {
                    Debug.LogError("Defense stat is null");
                    return;
                }
                reductionAmount = 0.001f * characterStat.Defense.StatValue;
                break;

            case Stat.Magic:
                if (characterStat.MagicResistance == null)
                {
                    Debug.LogError("MagicResistance stat is null");
                    return;
                }
                reductionAmount = 0.001f * characterStat.MagicResistance.StatValue;
                break;
            default:
                Debug.LogError($"Unknown AttackType: {AttackType}");
                return;
        }

        // 데미지 감소율 계산 클램프 0% ~ 99%
        reductionAmount = Mathf.Clamp(reductionAmount, 0f, 0.99f);

        float damageTaken = Value - (Value * reductionAmount);
        damageTaken = Mathf.Max(damageTaken, 0f);

        currentHp -= damageTaken;
        currentHp = Mathf.Clamp(currentHp, 0f, characterStat.MaxHp.StatValue);

        // 사망 처리
        if (currentHp <= 0)
        {
            ActiveDead();
        }
    }

    public void TakeHeal(float Value)
    {
        currentHp += Value;
        currentHp = Mathf.Clamp(currentHp, 0, characterStat.MaxHp.StatValue);
    }

    public void ActiveDead()
    {
        _isAlive = false;
        OnDeadEvent?.Invoke();
        Debug.Log($"{this.name} is Dead");
    }

    #endregion

    #region IPoolable Method

    public override void SetUpPool(Pool pool)
    {
        base.SetUpPool(pool);
    }

    public override void ResetItem()
    {
    }


    #endregion
}
