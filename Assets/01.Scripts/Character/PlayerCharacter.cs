using BehaviorDesigner.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCharacter : MonoCharacter, IDamageable
{
    [Space]
    [Header("플레이어 캐릭터 요소")]
    [Tooltip("캐릭터 이명")]
    public string Nickname;
    public EnemyCharacter AttackEnemy;

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

    private GameManager mng_Game;

    protected Dictionary<Type, IPlayerComponent> _components;

    private void Awake()
    {
        _components = new Dictionary<Type, IPlayerComponent>();
        GetComponentsInChildren<IPlayerComponent>().ToList().ForEach(compo => _components.Add(compo.GetType(), compo));

        _components.Values.ToList().ForEach(compo => compo.Initilize(this));

        _components.Values.ToList().ForEach(compo => compo.AfterInitilize());

        SetDefaultData();

        _tree = GetComponent<BehaviorTree>();
        _tree.StartWhenEnabled = true;
        _tree.PauseWhenDisabled = true;

        _tree.SetVariableValue("plc", this);
        _tree.SetVariableValue("range", characterStat.MoveSpeed.StatValue);
        _tree.SetVariable("isAlive", _isAlive);

        _target = _tree.GetVariable("target") as SharedTransform;
        _isSpiritMax = _tree.GetVariable("isSpiritMax") as SharedBool;

        _tree.enabled = false;
    }

    private void Start()
    {
        mng_Game = GameManager.Instance.GetInstance();
        mng_Game.OnActionRound += HandleStartRound;
    }

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
        characterStat.SetValues(CharacterProficiency);
        currentHp = characterStat.MaxHp.StatValue;
        characterSpirit.CurrentSpirit = characterSpirit.DefaultSpirit;
    }

    private void HandleStartRound(bool Action)
    {
        if(_tree == null)
        {
            Debug.LogError("BT is not assigned");
            return;
        }

        if (Action == true)
        {
            _tree.enabled = true;
            
            SetDefaultData();
            StartCharacter?.Invoke();
        }
        else if (Action == false)
        {
            _tree.enabled = false;
        }
    }

    #region IDamageable Methods

    public void TakeDamage(Stat AttackType, float Value)
    {
        currentHp -= Value;
        ActionHpEvent();

        if (currentHp <= 0) ActiveDead();
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

    public void ActiveDead()
    {
        _isAlive = false;
        OnDeadEvent?.Invoke();
        Debug.Log($"{this.name} is Dead");
    }

    #endregion
}