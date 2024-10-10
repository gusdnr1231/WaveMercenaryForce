using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : Manager<GameManager>
{
    [Header("게임 내 플레이어 관련 수치")]
    [Range(0, 50)]public int MaxDeployCount = 3;
    public int CollectGold = 0;

    public List<PlayerCharacter> DeployCharacters = new List<PlayerCharacter>();
    public List<EnemyCharacter> RemainEnemys = new List<EnemyCharacter>();

    private bool IsWinRound => RemainEnemys.Count <= 0;

    #region Game Manager Events

    public event Action<int> OnChangeGold;
    public event Action OnStartRound;
    public event Action<bool> OnEndRound;

    #endregion

    private bool StartRound = false;
    
    public void AddGold(int add)
    {
        CollectGold += add;
        CollectGold = Mathf.Clamp(CollectGold, 0, 9999);
        OnChangeGold?.Invoke(CollectGold);
    }

    public void AddRemainEnemy(EnemyCharacter add)
    {
        RemainEnemys.Add(add);
    }
}
