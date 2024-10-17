using UnityEngine;

public static class SpawnEvents
{
    public static PlayerCharacterCreate PlayerCharacterCreate = new PlayerCharacterCreate();
    public static EnemyCharacterCreate EnemyCharacterCreate = new EnemyCharacterCreate();
}

public class PlayerCharacterCreate: GameEvent
{
    public Vector3 pos;
    public Vector3 rot;
    public PlayerCharacterDataSO plcData;
}

public class EnemyCharacterCreate : GameEvent
{
    public Vector3 pos;
    public Vector3 rot;
    public EnemyCharacterDataSO emcData;
}
