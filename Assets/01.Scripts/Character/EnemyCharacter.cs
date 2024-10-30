using BehaviorDesigner.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyCharacter : MonoCharacter, IDamageable, ICharacterEvents
{
    private EnemyCharacterDataSO emcData { get; set; }

    #region Event Actions
    public event Action<EnemyCharacterDataSO> OnChangeCharacterData;   
    public event Action OnResetPool;

    public event Action<float> OnHpChange;
    public event Action<int> OnSpiritChange;

    // 전투 단계에서 사용할 이벤트들
    public event Action OnSetDefaultData;
    public event Action OnStartCharacter;
    public event Action OnEndCharacter;
    #endregion

    #region BT Values
    private BehaviorTree _tree;
    [HideInInspector] public SharedTransform _target;
    [HideInInspector] public SharedFloat _range;
    private SharedBool _isSpiritMax;
    #endregion

    protected Dictionary<Type, IEnemyComponent> _components;

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
        base.OnAnimationEnd();
    }

    private void SetDefaultData()
    {
        _isAlive.Value = true;
        _tree.SetVariableValue("isAlive", _isAlive);
        characterStat.SetValues(CharacterProficiency);

        currentHp = characterStat.MaxHp.StatValue;
        characterSpirit.CurrentSpirit = characterSpirit.DefaultSpirit;

        _tree.SetVariableValue("range", characterStat.AttackRange.StatValue);

        OnSetDefaultData?.Invoke();
    }

    protected override void GetDataFromDataBase()
    {
        base.GetDataFromDataBase();
        SetDefaultData();
    }

    public override void SetCharacterData(BaseCharacterDataSO initData)
    {
        base.SetCharacterData(initData);
        emcData = CharacterDataBase as EnemyCharacterDataSO;
        OnChangeCharacterData?.Invoke(emcData);
        GetDataFromDataBase();
    }

    public override void AddFightingSpirit(int AddAmount)
    {
        base.AddFightingSpirit(AddAmount);
    }

    public override void HandleStartBattlePhase(bool Action)
    {
        if (_tree == null)
        {
            Debug.LogError("BT is not assigned");
            return;
        }

        Debug.Log("Acitve HandleStartBattlePhase");

        if (Action == true)
        {
            _tree.enabled = true;
            SetDefaultData();

            OnStartCharacter?.Invoke();
        }
        else if (Action == false)
        {
            _tree.enabled = false;

            OnEndCharacter?.Invoke();
        }
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
            _isAlive = false;
        }
    }

    public void TakeHeal(float Value)
    {
        currentHp += Value;
        currentHp = Mathf.Clamp(currentHp, 0, characterStat.MaxHp.StatValue);
    }

    public void ActiveEnd()
    {
        _tree.enabled = false;
        OnEndCharacter?.Invoke();

        GameManager.Instance.RemoveRemainEnemy(this);
        Debug.Log($"{this.name} is Dead");

        _characterPool.Push(this);
    }

    #endregion

    #region IPoolable Method

    public override void SetUpPool(Pool pool)
    {
        base.SetUpPool(pool);

        emcData = CharacterDataBase as EnemyCharacterDataSO;

        _components = new Dictionary<Type, IEnemyComponent>();
        GetComponentsInChildren<IEnemyComponent>().ToList().ForEach(compo => _components.Add(compo.GetType(), compo));

        _components.Values.ToList().ForEach(compo => compo.Initilize(this));

        _components.Values.ToList().ForEach(compo => compo.AfterInitilize());

        if (_tree == null) _tree = GetComponent<BehaviorTree>();
        _tree.StartWhenEnabled = true;
        _tree.PauseWhenDisabled = true;

        _tree.SetVariableValue("emc", this);
        _tree.SetVariable("isAlive", _isAlive);

        _isAnimationEnd = _tree.GetVariable("isAnimationEnd") as SharedBool;
        _target = _tree.GetVariable("target") as SharedTransform;
        _isSpiritMax = _tree.GetVariable("isSpiritMax") as SharedBool;

        _tree.enabled = false;
    }

    public override void ResetItem()
    {
    }

    #endregion
}
