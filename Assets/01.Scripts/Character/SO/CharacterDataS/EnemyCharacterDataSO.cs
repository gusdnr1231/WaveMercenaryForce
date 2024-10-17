using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCharacterData", menuName = "Character/EnemyCharacter")]
public class EnemyCharacterDataSO : BaseCharacterDataSO
{
    [Space]
    [Header("�� ĳ���� ����")]
    public float EquipmentDropRate;
    public float GoldDropRate;
}
