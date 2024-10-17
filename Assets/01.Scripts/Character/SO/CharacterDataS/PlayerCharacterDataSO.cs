using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCharacterData", menuName = "Character/PlayerCharacter", order = 1)]
public class PlayerCharacterDataSO : BaseCharacterDataSO
{
    [Space]
    [Header("플레이어 캐릭터 정보")]
    [Tooltip("캐릭터 별명")]
    public string CharacterAlias;
}
