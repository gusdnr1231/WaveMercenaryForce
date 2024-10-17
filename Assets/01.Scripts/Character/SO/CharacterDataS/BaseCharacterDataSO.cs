using UnityEngine;

public class BaseCharacterDataSO : ScriptableObject
{
    [Header("�⺻ ĳ���� ����")]
    [Tooltip("ĳ���� �̸�")]
    public string CharacterName;
    [Tooltip("ĳ���� �̹���")]
    public Sprite CharacterSprite;
    [Tooltip("ĳ���� �ɷ�ġ ����")]
    public CharacterStat StatusData;
    [Tooltip("ĳ���� Ÿ��")]
    public CharacterType TypeData;
    [Tooltip("ĳ���� ���")]
    public CharacterGrade CharacterGrade;
    public AttackData CharacterAttack;
    public AttackData CharacterSkill;
    [field:SerializeField] public FightingSpirit CharacterSpirit { get; set; }
    public AnimatorOverrideController CharacterAnimator;
}
