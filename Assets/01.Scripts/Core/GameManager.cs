using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : Manager<GameManager>
{
    [Header("게임 내 플레이어 관련 수치")]
    [Range(1, 50)] public int MaxDeployCount = 3;
    public int CollectGold = 0;
    public int MaxCollectGold = 9999;
    [Header("인게임 플레이 수치")]
    [Range(60, 180)] public int MaxTurnTime = 120;
    [Range(60, 180)] public int MaxBattleTime = 120;
    private int remainTime;
    public int RemainTime => remainTime;

    [Tooltip("현재 배치된 플레이어 캐릭터")]
    [HideInInspector] public List<PlayerCharacter> DeployCharacters = new List<PlayerCharacter>();
    [Tooltip("남아있는 적 캐릭터")]
    [HideInInspector] public List<EnemyCharacter> RemainEnemys = new List<EnemyCharacter>();

    private bool IsWinRound => RemainEnemys.Count <= 0;

    #region Game Manager Events

    public event Action<int> OnChangeGold;
    public event Action OnStartRound;
    public event Action<bool> OnEndRound;

    #endregion
    
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

    public void AddRemainEnemy(EnemyCharacter add)
    {
        RemainEnemys.Add(add);
    }

    #region Play Flow

    private bool StartRound = false;

    public void ActiveTimer()
    {

    }

    #endregion
}
