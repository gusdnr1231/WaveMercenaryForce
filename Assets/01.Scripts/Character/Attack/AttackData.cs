using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackData", menuName = "Attack/AttackData")]
public abstract class AttackData : ScriptableObject
{
    [Header("공격 정보")]
    public string attackName;
    public int addSpirit = 5;
    [Space]
    [Tooltip("공격에 사용할 능력치")]
    public Stat AttackBy;
    [Space]
    public GameObject effectPrefab;

    public abstract void Execute(MonoCharacter attacker, Transform target);
}
