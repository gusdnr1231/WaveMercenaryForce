using BehaviorDesigner.Runtime.Tasks.Unity.Timeline;
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
    [Header("게임 내 플레이어 관련 수치")]
    [Range(1, 10)] private int PlayerHp = 5;
    public int playerHp => PlayerHp;
    [Range(1, 50)] public int MaxDeployCount = 3;
    public int CollectGold = 0;
    public int MaxCollectGold = 9999;

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
    public event Action<bool> IsPause;
    public event Action<bool> OnActionWait;
    public event Action<bool> OnActionRound;

    #endregion

    private void Start()
    {
        StartSettingTimer();
    }

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
            StopCoroutine(timerCrt);
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
