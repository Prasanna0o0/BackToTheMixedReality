using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassthroughBrightnessManager : MonoBehaviour
{

    [SerializeField] private OVRPassthroughLayer passthroughLayer;

    public void AdjustBrightness(float brightnessAmount)
    {
        if (passthroughLayer != null)
        {

            passthroughLayer.SetBrightnessContrastSaturation(brightnessAmount);
        }
    }


    public void AdjustBrightnessGradually(float targetBrightness, float duration)
    {
        if (passthroughLayer != null)
        {
            // Get the current brightness value
            float currentBrightness = -1;

          
            DOTween.To(() => currentBrightness, x => currentBrightness = x, 0f, 12f).OnUpdate(() =>
            {
                passthroughLayer.SetBrightnessContrastSaturation(currentBrightness);
            })
            .SetEase(Ease.Linear);
        }
    }
}
