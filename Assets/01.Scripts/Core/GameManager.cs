﻿using BehaviorDesigner.Runtime.Tasks.Unity.Timeline;
using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum Phase
{
    Setting,
    Battle,
}

public class GameManager : Manager<GameManager>
{
    [Header("플레이어 관련 수치")]
    [Range(1, 10)] private int PlayerHp = 5;
    public int playerHp => PlayerHp;
    public int CollectGold = 0;
    public int MaxCollectGold = 9999;

    [Header("건물 관련 수치")]
    [Tooltip("건물의 레벨")]
    public int BuildingLevel = 1;                           //건물의 레벨
    public int MaxLevel = 10;                               //건물의 최대 레벨
    public int CurrentExp = 0;                              //현재 Exp
    public List<int> NeedToExpValues = new List<int>();     //레벨 업에 필요한 EXP 수치들
    [HideInInspector] public int CurrentNeedExp = 0;        //현재 레벨업에 필요한 Exp 수치
    [Range(5, 20)] public int MaxSeatCount = 10;            //대기석 숫자
    [Range(1, 50)] public int MaxDeployCount = 3;           //최대 배치 가능 수
    [Range(1, 5)] public int IncreaseDeployCount = 2;      //레벨 증가시 추가될 배치 가능 수

    [Header("인게임 플레이 수치")]
    [Tooltip("페이즈 변경 대기 시간")]
    [Range(0f, 5f)] public float WaitPhaseChangeTime = 3f;
    [Tooltip("세팅 페이즈 시간")]
    [Range(60, 180)] public int MaxSettingTime = 120;
    [Tooltip("배틀 페이즈 시간")]
    [Range(60, 300)] public int MaxBattleTime = 240;
    private int RemainTime;
    public int remainTime => RemainTime;

    [Tooltip("현재 배치된 플레이어 캐릭터")]
    [HideInInspector] public List<PlayerCharacter> DeployCharacters = new List<PlayerCharacter>();
    [Tooltip("남아있는 적 캐릭터")]
    [HideInInspector] public List<EnemyCharacter> RemainEnemys = new List<EnemyCharacter>();

    private bool IsWinRound => RemainEnemys.Count <= 0;

    #region Game Manager Events

    public event Action<int> OnChangeGold;
    public event Action<int> OnUpdateTimer;
    public event Action<int> OnUpdateExp;
    public event Action<int, int> OnUpdateLevel;
    public event Action<bool> IsPause;
    public event Action<bool> OnActionWait;
    public event Action<bool> OnActionRound;

    #endregion

    private void Start()
    {
        SetLevel();
        StartSettingTimer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PauseGame(true);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            GoldChangeToValue(10);
        }
    }

    #region Gold Methods

    public bool CanUseGold(int useValue) => CollectGold - useValue >= 0;
    
    public void GoldChangeToFixed(int fixedValue)
    {
        if (fixedValue < 0) return;
        CollectGold = fixedValue;
        CollectGold = Mathf.Clamp(CollectGold, 0, MaxCollectGold);
        OnChangeGold?.Invoke(CollectGold);
    }

    public void GoldChangeToValue(int changeValue)
    {
        //바뀌는 값이 음수일 때, 만약 소모한 이후 값이 0보다 작을 경우 실행하지 않음
        if(changeValue < 0) if(CanUseGold(changeValue) == false) return;
        CollectGold += changeValue;
        CollectGold = Mathf.Clamp(CollectGold, 0, MaxCollectGold);
        OnChangeGold?.Invoke(CollectGold);
    }

    #endregion

    #region Building Level Methods

    public void AddExp(int add)
    {
        CurrentExp += add;
        OnUpdateExp?.Invoke(CurrentExp);
        CheckExp();
    }

    private void CheckExp()
    {
        if (CurrentExp >= CurrentNeedExp)
        {
            CurrentExp -= CurrentNeedExp;
            BuildingLevel += 1;

            MaxDeployCount += IncreaseDeployCount;

            SetLevel();
        }
    }

    public void SetLevel()
    {
        CurrentNeedExp = NeedToExpValues[BuildingLevel];
        OnUpdateLevel?.Invoke(BuildingLevel, CurrentNeedExp);
        OnUpdateExp?.Invoke(CurrentExp);
    }

    #endregion

    #region Characters Methods

    public void AddDeployCharacters(PlayerCharacter add)
    {
        DeployCharacters.Add(add);
    }

    public void RemoveDeployCharacters(PlayerCharacter remove)
    {
        DeployCharacters.Remove(remove);
    }

    public void AddRemainEnemy(EnemyCharacter add)
    {
        RemainEnemys.Add(add);
    }

    public void RemoceRemainEnemy(EnemyCharacter remove)
    {
        RemainEnemys.Remove(remove);
    }

    #endregion

    #region Play Flow

    private Phase GamePhase;
    private bool IsStartRound = false;

    public void StarBattlePhase()
    {
        if (IsStartRound) return;
        IsStartRound = true;
        StartBattleTimer();

        OnActionRound?.Invoke(IsStartRound);
    }

    public void EndBattlePhase()
    {
        if (!IsStartRound) return;
        IsStartRound = false;

        StartSettingTimer();

        if (IsWinRound == false)
        {
            PlayerHp -= 1;
            if(PlayerHp <= 0) EndGame();
        }

        OnActionRound?.Invoke(IsStartRound);
    }

    public void PauseGame(bool pause)
    {
        IsPause?.Invoke(pause);

        if (pause)
        {
            if(timerCrt != null) StopCoroutine(timerCrt);
            timerCrt = null;
        }
        else
        {
            timerCrt = StartCoroutine(ResumeTimer(GamePhase));
        }
    }

    public void EndGame()
    {
        Debug.Log("Game Over");
    }

    #region Turn Timer

    private Coroutine timerCrt;

    public void StartSettingTimer()
    {
        RemainTime = MaxSettingTime;
        GamePhase = Phase.Setting;
        if (timerCrt != null) StopCoroutine(timerCrt);
        timerCrt = null;
        timerCrt = StartCoroutine(TimerCoroutine(GamePhase));
    }

    public void StartBattleTimer()
    {
        RemainTime = MaxBattleTime;
        GamePhase = Phase.Battle;
        if (timerCrt != null) StopCoroutine(timerCrt);
        timerCrt = null;
        timerCrt = StartCoroutine(TimerCoroutine(GamePhase));
    }

    /// <summary>
    /// 타이머 실행
    /// </summary>
    /// <param name="phase">현재 게임 진행 상황을 받아와, 끝났을 경우 알맞은 이벤트를 실행</param>
    private IEnumerator TimerCoroutine(Phase phase)
    {
        //기다리는 UI 작업은 여기서
        OnActionWait?.Invoke(true);
        yield return new WaitForSeconds(WaitPhaseChangeTime);
        OnActionWait?.Invoke(false);

        OnUpdateTimer?.Invoke(remainTime); // 초기 시간 업데이트

        while (remainTime > 0)
        {
            yield return new WaitForSeconds(1f);
            RemainTime--;
            OnUpdateTimer?.Invoke(remainTime); // 시간 업데이트

            // 전투 중에 모든 적이 제거되면 라운드 종료
            if (phase == Phase.Battle && IsWinRound)
            {
                EndBattlePhase();
                yield break;
            }
        }

        // 타이머가 끝난 후의 처리
        if (phase == Phase.Setting)
        {
            // 설정 시간이 끝나면 전투 시간 타이머 시작
            StartBattleTimer();
        }
        else if (phase == Phase.Battle)
        {
            // 전투 시간이 끝나면 라운드 종료
            EndBattlePhase();
        }
    }

    /// <summary>
    /// 타이머가 실행 중, 중단 되었을 경우 실행
    /// </summary>
    /// <param name="phase">현재 게임 진행 상황을 받아와, 끝났을 경우 알맞은 이벤트를 실행</param>
    private IEnumerator ResumeTimer(Phase phase)
    {
        OnUpdateTimer?.Invoke(remainTime); // 초기 시간 업데이트

        while (remainTime > 0)
        {
            yield return new WaitForSeconds(1f);
            RemainTime--;
            OnUpdateTimer?.Invoke(remainTime); // 시간 업데이트

            // 전투 중에 모든 적이 제거되면 라운드 종료
            if (phase == Phase.Battle && IsWinRound)
            {
                EndBattlePhase();
                yield break;
            }
        }

        // 타이머가 끝난 후의 처리
        if (phase == Phase.Setting)
        {
            // 설정 시간이 끝나면 전투 시간 타이머 시작
            StartBattleTimer();
        }
        else if (phase == Phase.Battle)
        {
            // 전투 시간이 끝나면 라운드 종료
            EndBattlePhase();
        }
    }

    #endregion

    #endregion
}
