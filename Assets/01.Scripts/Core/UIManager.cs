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

    private GameManager mng_Game;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        mng_Game = GameManager.Instance.GetInstance();

        mng_Game.OnUpdateTimer += HandleTimer;
    }

    private void HandleTimer(int time)
    {
        int minutes = time / 60;
        int seconds = time % 60;

        string timerText = string.Format("{0:00}:{1:00}", minutes, seconds);
        Timer_Text.text = timerText;
    }
}
