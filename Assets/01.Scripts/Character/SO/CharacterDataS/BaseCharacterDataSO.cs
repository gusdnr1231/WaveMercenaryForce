using UnityEngine;

public class BaseCharacterDataSO : ScriptableObject
{
    [Header("기본 캐릭터 정보")]
    [Tooltip("캐릭터 이름")]
    public string CharacterName;
    [Tooltip("캐릭터 이미지")]
    public Sprite CharacterSprite;
    [Tooltip("캐릭터 능력치 정보")]
    public CharacterStat StatusData;
    [Tooltip("캐릭터 타입")]
    public CharacterType TypeData;
    [Tooltip("캐릭터 등급")]
    public CharacterGrade CharacterGrade;
    public AttackData CharacterAttack;
    public AttackData CharacterSkill;
    [field:SerializeField] public FightingSpirit CharacterSpirit { get; set; }
    public AnimatorOverrideController CharacterAnimator;
}
