using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundCounter : MonoBehaviour
{
    private readonly int TintFade = Shader.PropertyToID("_StrongTintFade");
    private readonly int Brightness = Shader.PropertyToID("_Brightness");

    [Header("카운터 이미지")]
    [SerializeField] private List<Sprite> CounterSpirtes;

    private Image CounterImage;
    private Material CounterShader;
    private int currentRound = 0;

    private void Awake()
    {
        CounterImage = GetComponent<Image>();
        CounterShader = CounterImage.material;

        CounterImage.sprite = CounterSpirtes[currentRound];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) TestCounter();
    }

    private void TestCounter()
    {
        currentRound = Mathf.Clamp(currentRound + 1, 0, 11);
        CounterImage.sprite = CounterSpirtes[currentRound];

        CounterShader.DOKill();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(CounterShader.DOFloat(1, TintFade, 0.2f));
        sequence.Append(CounterShader.DOFloat(5, Brightness, 0.2f));
        sequence.Append(CounterShader.DOFloat(0, TintFade, 0.2f));
        sequence.Append(CounterShader.DOFloat(1, Brightness, 0.2f));

        sequence.Play();
    }
}
