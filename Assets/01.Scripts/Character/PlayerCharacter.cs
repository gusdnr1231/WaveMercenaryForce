using BehaviorDesigner.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerCharacter : MonoCharacter, IDamageable
{
    [Space]
    [Header("플레이어 캐릭터 요소")]
    [Tooltip("캐릭터 이명")]
    public string Nickname;
    public EnemyCharacter AttackEnemy;

    #region Event Actions
    public event Action OnDeadEvent;

    #endregion

    #region BT Values
    private BehaviorTree _tree;
    public SharedTransform _target;
    public SharedFloat _range;
    #endregion

    private float currentHp;

    protected Dictionary<Type, IPlayerComponent> _components;

    private void Awake()
    {
        _components = new Dictionary<Type, IPlayerComponent>();
        GetComponentsInChildren<IPlayerComponent>().ToList().ForEach(compo => _components.Add(compo.GetType(), compo));

        _components.Values.ToList().ForEach(compo => compo.Initilize(this));

        _components.Values.ToList().ForEach(compo => compo.AfterInitilize());

        SetDefaultData();

        _tree = GetComponent<BehaviorTree>();
        _tree.SetVariableValue("plc", this);
        _tree.SetVariableValue("range", characterStat.MoveSpeed.StatValue);

        _target = _tree.GetVariable("target") as SharedTransform;
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
        IsAlive = true;
        characterStat.SetValues(CharacterProficiency);
        currentHp = characterStat.MaxHp.StatValue;
        characterSpirit.CurrentSpirit = characterSpirit.DefaultSpirit;
    }

    #region IDamageable Methods

    public void TakeDamage(float Value)
    {
        currentHp -= Value;
        currentHp = Mathf.Clamp(currentHp, 0, characterStat.MaxHp.StatValue);

        if(currentHp <= 0) ActiveDead();
    }

    public void TakeHeal(float Value)
    {
        currentHp += Value;
        currentHp = Mathf.Clamp(currentHp, 0, characterStat.MaxHp.StatValue);
    }

    public void ActiveDead()
    {
        IsAlive = false;
        OnDeadEvent?.Invoke();
        Debug.Log($"{this.name} is Dead");
    }

    #endregion
}