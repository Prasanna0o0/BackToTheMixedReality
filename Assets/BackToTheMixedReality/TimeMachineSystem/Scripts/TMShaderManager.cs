using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMShaderManager : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;
    private Material targetMaterial;

    private void Start()
    {
        if (targetRenderer != null)
        {
            targetMaterial = targetRenderer.material;
           DeActivateLightEffect();
        }


    }

    public void TweenLightEffect(float targetValue, float duration)
    {
        if (targetMaterial != null)
        {
            DOTween.To(() => targetMaterial.GetFloat("_LightEffect"),
                       x => targetMaterial.SetFloat("_LightEffect", x),
                       targetValue, duration);
        }
    }

    public void TweenSpeed(float targetValue, float duration)
    {
        if (targetMaterial != null)
        {
            DOTween.To(() => targetMaterial.GetFloat("_Speed"),
                       x => targetMaterial.SetFloat("_Speed", x),
                       targetValue, duration);
        }
    }


    public void ActivateLightEffect()
    {
        TweenLightEffect(2f,1f);
        TweenSpeed(10f,5f);
    }


    public void DeActivateLightEffect()
    {
        TweenLightEffect(0f, 1.5f);
        TweenSpeed(0f, 1.5f);
    }
}
