using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSeat : MonoBehaviour
{
    public int MaxSeatCount = 10;
    public int CurrentReaminSeatCount { get; private set; }
    public Dictionary<int ,PlayerCharacter> SeatPair = new Dictionary<int ,PlayerCharacter>();

    private void Awake()
    {
        CurrentReaminSeatCount = 10;
        SeatPair = new Dictionary<int, PlayerCharacter>();
        
        for(int count  = 0; count < MaxSeatCount; count++)
        {
            SeatPair.Add(count, null);
        }
    }

    public bool AddCharacterToSeat(PlayerCharacter addCharacter)
    {
        if (CurrentReaminSeatCount - 1 <= 0) return false;

        return true;
    }
}
