using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stat
{
    MaxHp = 0,
    Strength = 6,
    AttackSpeed = 7,
    MoveSpeed = 8,
    End = 9
}

[Serializable]
public class StatData
{
    public float baseValue;
    public Stat statType;

    private readonly List<StatModifier> statModifiers = new List<StatModifier>();

    public StatData(float BaseValue, Stat InitStatType)
    {
        baseValue = BaseValue;
        statType = InitStatType;
        statModifiers = new List<StatModifier>();
    }

    private bool isDirty = true;
    private float statValue;
    public float StatValue
    {
        get
        {
            if (isDirty)
            //���� ��ȭ�ߴٸ�?
            {
                statValue = CalculateFinalValue();
                //�ٽ� ����� ���� �Ҵ�
                isDirty = false;
                //��ȭ���� ���� ���·� ����
            }

            return statValue;
            //����� �� ��ȯ
        }
    }

    public void AddModifier(StatModifier modifier)
    {
        isDirty = true;
        statModifiers.Add(modifier);

        Debug.Log("Show Stat [" + statType + "]\nBase : " + baseValue + " Value: " + StatValue);
    }

    public bool RemoveModifier(StatModifier modifier)
    {
        if (statModifiers.Remove(modifier))
        {
            isDirty = true;
            return true;
        }

        Debug.LogError("Error: Modifier is not removed : " + statType + "\nBase : " + baseValue + " Value: " + StatValue);
        return false;
    }

    public bool RemoveAllModifierInSource(object source)
    {
        bool didRemove = false;

        for (int count = statModifiers.Count - 1; count >= 0; count--)
        {
            if (statModifiers[count]._Source == source)
            {
                isDirty = true;
                didRemove = true;
                statModifiers.RemoveAt(count);
            }
        }

        Debug.Log("Show Stat [" + statType + "]\nBase : " + baseValue + " Value: " + StatValue);
        return didRemove;
    }

    public float CalculateFinalValue()
    {
        float finalValue = baseValue;
        float percentSum = 1f;
        // �⺻ �� ����

        Debug.Log(statModifiers.Count);
        if (statModifiers.Count > 0)
        {
            StatModifier modifier;
            // �ݺ� ���� ����

            for (int count = 0; count < statModifiers.Count; count++)
            // ���� StatModifier�� ����ŭ �ݺ� 
            {
                modifier = statModifiers[count];
                if (modifier._IsPercent == true)
                    percentSum += 0.01f * modifier._Value;
                // �ۼ�Ʈ ��ġ�� �´ٸ� ������� ��ȯ�� �ۼ�Ʈ ���� �߰�
                else if (modifier._IsPercent == false)
                    finalValue += modifier._Value;
                // �ۼ�Ʈ ��ġ�� �ƴ϶�� �ٷ� finalValue�� ���ϱ�
            }
        }

        if (percentSum > 0f) return (float)Math.Round(finalValue * percentSum, 3);
        // �⺻ �� + ���� ��ġ ���� ������ �� �Ҽ��� 3�ڸ����� ó��
        else return (float)Math.Round(finalValue, 3);
        // �⺻ �� �Ҽ��� 3�ڸ����� ó��
    }
}

public class StatModifier
{
    public readonly float _Value;
    public readonly bool _IsPercent;
    public readonly object _Source;

    public StatModifier(float Value, bool IsPercent, object Source)
    {
        _Value = Value;
        _IsPercent = IsPercent;
        _Source = Source;

    }
}

public class CharacterStat : ScriptableObject
{
    public string CharacterName;

}
