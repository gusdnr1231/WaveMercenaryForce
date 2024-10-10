using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public struct LocationInfo
{
    public Vector3 Position;
    public Quaternion Rotation;
}

public class CameraManager : Manager<CameraManager>
{
    private Camera PlayerCam;

    protected override void Awake()
    {
        base.Awake();
        PlayerCam = Camera.main;
        MoveCamera(0);
    }

    #region Move Cam

    [Header("카메라 이동")]
    [SerializeField] private float CamMoveDuration = 0.3f;
    [SerializeField] private LocationInfo[] CamLocations;

    private int CurrentCamLocation = -1;
    /// <summary>
    /// 카메라를 미리 설정한 위치로 이동 및 회전 시킨다.
    /// </summary>
    /// <param name="index">몇 번째 값으로 이동할 것인지 설정</param>
    public void MoveCamera(int index)
    {
        if (CurrentCamLocation == index) return;
        CurrentCamLocation = index;

        LocationInfo moveToInfo = CamLocations[CurrentCamLocation];

        PlayerCam.transform.DOMove(moveToInfo.Position, CamMoveDuration)
        .SetEase(Ease.OutQuint);
        PlayerCam.transform.DORotate(moveToInfo.Rotation.eulerAngles, CamMoveDuration);
    }

    #endregion

}
