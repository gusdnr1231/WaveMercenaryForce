using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class UIManager : Manager<UIManager>
{
    [Header("게임 화면 UI 요소들")]
    [SerializeField] private TextMeshProUGUI Timer_Text;
    [SerializeField] private TextMeshProUGUI Gold_Text;
    [SerializeField] private GameObject SettingUI;

    [Header("용병단 건물 정보 UI 요소들")]
    [SerializeField] private TextMeshProUGUI BuilgindLevel_Text;
    [SerializeField] private TextMeshProUGUI CurrentExp_Text;
    [SerializeField] private TextMeshProUGUI NeedExp_Text;
    [Space]
    [Tooltip("용병단 건물 EXP 구매")]
    [SerializeField] private Button BuyExp_Btn;
    [SerializeField] private int AddExpValue = 3;
    [SerializeField] private int NeedGold = 2;

    private GameManager mng_Game;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        mng_Game = GameManager.Instance.GetInstance();

        mng_Game.OnUpdateTimer += HandleTimer;
        mng_Game.OnChangeGold += HandleGold;
        mng_Game.OnActionBattlePhase += HandleActiveRound;

        mng_Game.OnUpdateLevel += HandleBuildingLevel;
        mng_Game.OnUpdateExp += HandleBuildingExp;
        
        BuyExp_Btn.onClick.RemoveAllListeners();
        BuyExp_Btn.onClick.AddListener(() =>
        {
            if (mng_Game.CanUseGold(NeedGold))
            {
                mng_Game.GoldChangeToValue(-NeedGold);
                mng_Game.AddExp(AddExpValue);
            }
        });
    }

    private void HandleTimer(int time)
    {
        int minutes = time / 60;
        int seconds = time % 60;

        string timerText = string.Format("{0:00}:{1:00}", minutes, seconds);
        Timer_Text.text = timerText;
    }

    private void HandleGold(int gold)
    {
        Gold_Text.text = gold.ToString();
    }

    private void HandleActiveRound(bool isActiveRound)
    {
        SettingUI.SetActive(!isActiveRound);
    }

    private void HandleBuildingLevel(int level, int need)
    {
        BuilgindLevel_Text.text = level.ToString();
        NeedExp_Text.text = need.ToString();
    }

    private void HandleBuildingExp(int exp)
    {
        CurrentExp_Text.text = exp.ToString();
    }
}
