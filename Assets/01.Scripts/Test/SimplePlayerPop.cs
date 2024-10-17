using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerPop : MonoBehaviour
{
    [SerializeField] private PlayerCharacterDataSO make;
    [SerializeField] private GameEventChannelSO makeChannel;

    public Vector3 MinPoint;
    public Vector3 MaxPoint;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            PopPlayer();
        }   
    }

    private void PopPlayer()
    {
        var evt = SpawnEvents.PlayerCharacterCreate;
        evt.pos = new Vector3(Random.Range(MinPoint.x, MaxPoint.x), 0, Random.Range(MinPoint.z, MaxPoint.z));
        evt.rot = Vector3.zero;
        evt.plcData = make;

        makeChannel.RasieEvent(evt);
    }
}
