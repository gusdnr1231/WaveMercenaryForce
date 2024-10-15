using BehaviorDesigner.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ParamValue
{
    public bool boolValue;
    public float floatValue;
    public int intValue;
}

public abstract class MonoChacarterAnimator : MonoBehaviour
{
    [SerializeField] private AnimatorParamListSO _paramList;

    protected MonoCharacter _character;
    protected Animator _animator;
    public Animator Animator => _animator;

    protected virtual void AnimationEnd()
    {
        _character.OnAnimationEnd();
    }

    public virtual void ClearAllBoolen() => _paramList.ClearAllBoolen(_animator);

    public virtual void SetParam(AnimatorParamSO param, ParamValue value)
    {
        switch (param.paramType)
        {
            case ParamType.Boolen:
                _animator.SetBool(param.hashValue, value.boolValue); break;
            case ParamType.Float:
                _animator.SetFloat(param.hashValue, value.floatValue); break;
            case ParamType.Trigger:
                _animator.SetTrigger(param.hashValue); break;
            case ParamType.Integer:
                _animator.SetInteger(param.hashValue, value.intValue); break;
        }
    }
}