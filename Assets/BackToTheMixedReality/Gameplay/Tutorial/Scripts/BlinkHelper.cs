using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class BlinkHelper : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private float blinkDuration = 0.25f;


    private void Awake()
    {
        if (textComponent == null)
        {
            textComponent = GetComponent<TextMeshProUGUI>();
        }
    }

    void Start()
    {
        BlinkText();
    }

    void BlinkText()
    {
        textComponent.DOFade(0.25f, blinkDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    void StopBlinking()
    {
        textComponent.DOKill();
        textComponent.alpha = 1; // Ensure it's fully visible when stopping
    }

}
