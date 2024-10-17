using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCharacterData", menuName = "Character/EnemyCharacter")]
public class EnemyCharacterDataSO : BaseCharacterDataSO
{
    [Space]
    [Header("적 캐릭터 정보")]
    public float EquipmentDropRate;
    public float GoldDropRate;
}
