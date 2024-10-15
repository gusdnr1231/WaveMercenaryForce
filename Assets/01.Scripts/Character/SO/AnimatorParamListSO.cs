using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Animator/ParamList")]
public class AnimatorParamListSO : ScriptableObject
{
    public List<AnimatorParamSO> list = new List<AnimatorParamSO>();

    private List<AnimatorParamSO> _boolenList;

    private void OnEnable()
    {
        _boolenList = list.Where(param => param.paramType == ParamType.Boolen).ToList();
    }

    public void ClearAllBoolen(Animator animator)
    {
        foreach (AnimatorParamSO param in _boolenList)
        {
            animator.SetBool(param.hashValue, false);
        }
    }
}