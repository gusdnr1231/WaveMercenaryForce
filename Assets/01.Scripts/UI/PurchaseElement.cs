using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseElement : MonoBehaviour
{
    [Header("캐릭터 정보 UI")]
    [SerializeField] private Image CharacterImage;
    [SerializeField] private TextMeshProUGUI CharacterName;
    [SerializeField] private TextMeshProUGUI CharacterCost;

    private Button InteractionButton;

    private void Awake()
    {
        InteractionButton = GetComponent<Button>();
        
        InteractionButton.onClick.RemoveAllListeners();
        InteractionButton.onClick.AddListener(() => BuyElement());
    }

    private void BuyElement()
    {
        GameManager.Instance.GoldChangeToValue(-2);
    }
}
