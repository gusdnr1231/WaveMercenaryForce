using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum Stat
{
    MaxHp = 0,
    Strength = 1,
    Magic = 2,
    Defense = 3,
    MagicResistance = 4,
    AttackRange = 5,
    AttackSpeed = 6,
    MoveSpeed = 7,
    End = 8
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

[Serializable]
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

[Serializable]
public struct ProficiencyValues
{
    public float MaxHp;
    public float Strength;
    public float Magic;
    public float Defense;
    public float MagicResistance;
    public float AttackRange;
    public float AttackSpeed;
    public float MoveSpeed;
}

[CreateAssetMenu(menuName = "Character/Stat", fileName = "New Character Stat", order = 3)]
public class CharacterStat : ScriptableObject
{
    public List<ProficiencyValues> CharacterStatValues;

    [HideInInspector] public StatData MaxHp;
    [HideInInspector] public StatData Strength;
    [HideInInspector] public StatData Magic;
    [HideInInspector] public StatData Defense;
    [HideInInspector] public StatData MagicResistance;
    [HideInInspector] public StatData AttackRange;
    [HideInInspector] public StatData AttackSpeed;
    [HideInInspector] public StatData MoveSpeed;

    //���� ĳ������ �⺻ �ɷ�ġ�� ���� ���õ��� �°� �����Ѵ�.
    public void SetValues(int Proficiency)
    {
        ProficiencyValues AfterStatValue = CharacterStatValues[Proficiency];

        MaxHp = new StatData(AfterStatValue.MaxHp, Stat.MaxHp);
        Strength = new StatData(AfterStatValue.Strength, Stat.Strength);
        Magic = new StatData(AfterStatValue.Magic, Stat.Magic);
        Defense = new StatData(AfterStatValue.Defense, Stat.Defense);
        MagicResistance = new StatData(AfterStatValue.MagicResistance, Stat.MagicResistance);
        AttackRange = new StatData(AfterStatValue.AttackRange, Stat.AttackRange);
        AttackSpeed = new StatData(AfterStatValue.AttackSpeed, Stat.AttackSpeed);
        MoveSpeed = new StatData(AfterStatValue.MoveSpeed, Stat.MoveSpeed);
    }

    public float ReturnStatValue(Stat stat)
    {
        switch (stat)
        {
            case Stat.MaxHp:
                return MaxHp.StatValue;
            case Stat.Strength:
                return Strength.StatValue;
            case Stat.Magic:
                return Magic.StatValue;
            case Stat.Defense:
                return Defense.StatValue;
            case Stat.MagicResistance:
                return MagicResistance.StatValue;
            case Stat.AttackRange:
                return AttackRange.StatValue;
            case Stat.AttackSpeed:
                return AttackSpeed.StatValue;
            case Stat.MoveSpeed:
                return MoveSpeed.StatValue;
            default:
                Debug.LogError($"Stat not found: {stat}");
                return 0f; // ������ �⺻�� ��ȯ
        }
    }

}
