using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public struct LocationInfo
{
    public Vector3 Position;
    public Vector3 Rotation;
}

public class CameraManager : Manager<CameraManager>
{
    private Camera PlayerCam;
    public bool IsCameraMoving { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        PlayerCam = Camera.main;
        IsCameraMoving = false;
        MoveCamera(0);
    }

    #region Move Cam

    [Header("ī�޶� �̵�")]
    [SerializeField] private float CamMoveDuration = 0.3f;
    [SerializeField] private LocationInfo[] CamLocations;

    private int CurrentCamLocation = -1;
    /// <summary>
    /// ī�޶� �̸� ������ ��ġ�� �̵� �� ȸ�� ��Ų��.
    /// </summary>
    /// <param name="index">�� ��° ������ �̵��� ������ ����</param>
    public void MoveCamera(int index)
    {
        if (CurrentCamLocation == index) return;
        CurrentCamLocation = index;

        LocationInfo moveToInfo = CamLocations[CurrentCamLocation];

        PlayerCam.transform.DOMove(moveToInfo.Position, CamMoveDuration)
        .SetEase(Ease.OutQuint)
        .OnStart(() => IsCameraMoving = true)
        .OnComplete(() => IsCameraMoving = false);
        PlayerCam.transform.DORotate(moveToInfo.Rotation, CamMoveDuration);
    }

    #endregion

}
