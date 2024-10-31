using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterStatusUI : MonoBehaviour, IPlayerComponent, IEnemyComponent
{
    [Header("Character Status Elements")]
    [SerializeField] private Image HpImage;
    [SerializeField] private Image SpiritImage;
    public Canvas StatusCanvas { get; private set; }

    private MonoCharacter _character;

    private Camera _mainCam;
    private FightingSpirit characterSpiritInfo;
    private float characterMaxHp;

    public void Initilize(PlayerCharacter plc)
    {
        _character = plc;
        plc.OnChangeCharacterData += HandleSetData;
        Debug.Log("Initialize this to PlayerCharacter");

        _mainCam = Camera.main;
        StatusCanvas = GetComponent<Canvas>();
    }

    public void Initilize(EnemyCharacter emc)
    {
        _character = emc;
        emc.OnChangeCharacterData += HandleSetData;
        Debug.Log("Initialize this to EnemyCharacter");

        _mainCam = Camera.main;
        StatusCanvas = GetComponent<Canvas>();
    }

    public void AfterInitilize()
    {
        if(_character is ICharacterEvents charEvts)
        {
            charEvts.OnHpChange += HandleHpBarFill;
            charEvts.OnSpiritChange += HandleSpiritBarFill;
        }

        if(_character is PlayerCharacter plc)
        {
            Debug.Log("AfterInitialize to PlayerCharacter");
        }
        if(_character is EnemyCharacter emc)
        {
            Debug.Log("AfterInitialize to EnemyCharacter");
        }
    }

    private void Update()
    {
        Vector3 direction = transform.position - _mainCam.transform.position;
        direction.y = 0; // y축 회전 제거
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // x축만 업데이트하도록 설정
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    private void HandleVisiableStatisUI(bool isVisiable)
    {
        StatusCanvas.enabled = isVisiable;
    }

    private void HandleSetData(EnemyCharacterDataSO data)
    {
        characterMaxHp = data.StatusData.MaxHp.StatValue;
        characterSpiritInfo = _character.characterSpirit;
    }

    private void HandleSetData(PlayerCharacterDataSO data)
    {
        characterMaxHp = data.StatusData.MaxHp.StatValue;
        characterSpiritInfo = _character.characterSpirit;
    }

    private void HandleHpBarFill(float amount)
    {
        float targetFillAmount = amount / characterMaxHp;
        HpImage.DOFillAmount(targetFillAmount, 0.1f).SetEase(Ease.OutCirc);
    }

    private void HandleSpiritBarFill(int amount)
    {
        float targetFillAmount = (float)amount / characterSpiritInfo.MaxSpirit;
        SpiritImage.DOFillAmount(targetFillAmount, 0.1f).SetEase(Ease.OutCirc);
    }

}
