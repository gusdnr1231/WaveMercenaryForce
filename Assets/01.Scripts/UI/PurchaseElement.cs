using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseElement : MonoBehaviour, IPoolable
{
    [field: SerializeField] public PoolTypeSO PoolType { get; private set; }
    [SerializeField] private GameEventChannelSO SpawnChannel;
    public GameObject GameObject => gameObject;
    private Pool _purchaseUIPool;

    [Header("캐릭터 정보 UI")]
    [SerializeField] private Image CharacterImage;
    [SerializeField] private TextMeshProUGUI CharacterName;
    [SerializeField] private TextMeshProUGUI CharacterCost;

    private Button InteractionButton;
    public PlayerCharacterDataSO BuyCharacter { get; private set; }
    private int _costValue = 0;

    private void Awake()
    {
        InteractionButton = GetComponent<Button>();
        
        InteractionButton.onClick.RemoveAllListeners();
        InteractionButton.onClick.AddListener(() => BuyElement());
    }

    private void BuyElement()
    {
        if (BuyCharacter == null) return;
        if (GameManager.Instance.GoldChangeToValue(-_costValue))
        {
            var evt = SpawnEvents.PlayerCharacterCreate;
            evt.pos = Vector3.zero;
            evt.rot = Vector3.zero;
            evt.plcData = BuyCharacter;
            Debug.Log(evt.ToString());
        }

    }

    public void SetBuyCharacter(PlayerCharacterDataSO initData)
    {
        BuyCharacter = initData;

        _costValue = (int)initData.CharacterGrade;

        CharacterImage.sprite = initData.CharacterSprite;
        CharacterName.text = initData.CharacterName;
        CharacterCost.text = _costValue.ToString();
    }

    #region IPoolable Methods

    public void SetUpPool(Pool pool)
    {
        _purchaseUIPool = pool;
    }

    public void ResetItem()
    {
    }

    #endregion
}
