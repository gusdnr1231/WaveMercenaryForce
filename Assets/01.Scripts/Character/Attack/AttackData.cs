using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackData", menuName = "Attack/AttackData")]
public abstract class AttackData : ScriptableObject
{
    [Header("���� ����")]
    public string attackName;
    [Space]
    [Tooltip("���� ���(�Ϲ�/�޺�)")]
    public AttackMethod attackType;
    [Tooltip("���ݿ� ����� �ɷ�ġ")]
    public Stat AttackBy;
    [Space]
    public GameObject effectPrefab;

    public abstract void Execute(MonoCharacter attacker, Transform target);
}
