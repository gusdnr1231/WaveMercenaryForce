using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCharacterData", menuName = "Character/PlayerCharacter", order = 1)]
public class PlayerCharacterDataSO : BaseCharacterDataSO
{
    [Space]
    [Header("�÷��̾� ĳ���� ����")]
    [Tooltip("ĳ���� ����")]
    public string CharacterAlias;
}
