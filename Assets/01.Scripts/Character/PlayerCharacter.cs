using BehaviorDesigner.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCharacter : MonoCharacter, IDamageable, ICharacterEvents
{
    private PlayerCharacterDataSO plcData { get; set; }
    private LocationInfo DefaultLoaction = new LocationInfo(){ Position = Vector3.zero, Rotation = Vector3.zero};

    #region Event Actions
    public event Action<PlayerCharacterDataSO> OnChangeCharacterData;
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

    private GameManager mng_Game;

    protected Dictionary<Type, IPlayerComponent> _components;

    public T GetCompo<T>() where T : class
    {
        if (_components.TryGetValue(typeof(T), out IPlayerComponent compo))
        {
            return compo as T;
        }

        return default;
    }

    private void SetDefaultData()
    {
        _isAlive.Value = true;
        _tree.SetVariableValue("isAlive", _isAlive);
        characterStat.SetValues(CharacterProficiency);

        currentHp = characterStat.MaxHp.StatValue;
        OnHpChange?.Invoke(currentHp);

        characterSpirit.CurrentSpirit = characterSpirit.DefaultSpirit;
        OnSpiritChange?.Invoke(characterSpirit.CurrentSpirit);

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
        plcData = CharacterDataBase as PlayerCharacterDataSO;
        OnChangeCharacterData?.Invoke(plcData);
        GetDataFromDataBase();
    }

    public override void HandleStartBattlePhase(bool Action)
    {
        if (_tree == null)
        {
            Debug.LogError("BT is not assigned");
            return;
        }

        if (Action == true)
        {
            DefaultLoaction.Position = transform.position;

            SetDefaultData();
            _tree.enabled = true;
            OnStartCharacter?.Invoke();
        }
        else if (Action == false)
        {
            ActiveEnd();
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

        ActionHpEvent();

        Debug.Log($"PlayerCharacter {CharacterDataBase.name} [{currentHp}/{CharacterDataBase.StatusData.MaxHp.StatValue}]");
        // 사망 처리
        if (currentHp <= 0f)
        {
            Debug.LogError("Active Dead_P");
            _isAlive.SetValue(false);
        }
    }

    public void TakeHeal(float Value)
    {
        currentHp += Value;
        ActionHpEvent();
    }

    private void ActionHpEvent()
    {
        currentHp = Mathf.Clamp(currentHp, 0, characterStat.MaxHp.StatValue);
        OnHpChange?.Invoke(currentHp);
    }

    public void ActiveEnd()
    {
        _tree.enabled = false;
        SetCharacterPosition(DefaultLoaction.Position);
        OnEndCharacter?.Invoke();
    }

    #endregion

    #region IPoolable Methods

    public override void SetUpPool(Pool pool)
    {
        base.SetUpPool(pool);

        plcData = CharacterDataBase as PlayerCharacterDataSO;

        _components = new Dictionary<Type, IPlayerComponent>();
        GetComponentsInChildren<IPlayerComponent>().ToList().ForEach(compo => _components.Add(compo.GetType(), compo));

        _components.Values.ToList().ForEach(compo =>
        {
            compo.Initilize(this);
            Debug.Log(compo.ToString());
        });
        
        _components.Values.ToList().ForEach(compo => compo.AfterInitilize());

        if (_tree == null) _tree = GetComponent<BehaviorTree>();
        _tree.StartWhenEnabled = true;
        _tree.PauseWhenDisabled = true;

        _tree.SetVariableValue("plc", this);
        _tree.SetVariable("isAlive", _isAlive);

        _isAnimationEnd = _tree.GetVariable("isAnimationEnd") as SharedBool;
        _target = _tree.GetVariable("target") as SharedTransform;
        _isSpiritMax = _tree.GetVariable("isSpiritMax") as SharedBool;

        _tree.enabled = false;
    }

    public override void ResetItem()
    {
        OnResetPool?.Invoke();
    }

    #endregion
}