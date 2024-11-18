public static class GameValueEvents
{
    public static ChangeRoundValue ChangeRoundValue = new ChangeRoundValue();
    public static ChangeGoldValue ChangeGoldValue = new ChangeGoldValue();
    public static ChangeHPValue ChangeHPValue = new ChangeHPValue();
}

public class ChangeRoundValue : GameEvent
{
    public int currentRound;
}

public class ChangeGoldValue : GameEvent
{
    public int currentGold;
}

public class ChangeHPValue : GameEvent
{
    public int currentPlayerHP;
}