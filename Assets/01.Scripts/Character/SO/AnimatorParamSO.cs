using UnityEngine;

public enum ParamType
{
    Boolen, Float, Trigger, Integer
}

[CreateAssetMenu(menuName = "SO/Animator/Param")]
public class AnimatorParamSO : ScriptableObject
{
    public ParamType paramType;
    public string paramName;
    public int hashValue;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(paramName) == false)
        {
            hashValue = Animator.StringToHash(paramName);
        }
    }
}