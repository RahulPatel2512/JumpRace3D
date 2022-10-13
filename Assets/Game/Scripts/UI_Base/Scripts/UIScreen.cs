﻿using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UIScreen : MonoBehaviour
{
    CanvasGroup canvas;
    public EasyTween[] Listofanimation;
    Func<EasyTween, float> Time = t => t.animationParts.GetAnimationDuration();

    void Awake()
    {
        canvas = gameObject.GetComponent<CanvasGroup>();
    }


    [ContextMenu("Start")]
    public void AnimationPenal(bool AnimationState)
    {
        CanvasOff(AnimationState, AnimationState == true ? 0 : Listofanimation.Max(Time));
        for (int i = 0; i < Listofanimation.Length; i++)
        {
            Listofanimation[i].OpenCloseObjectAnimationDefine(AnimationState);
        }
    }

    async void CanvasOff(bool state, float T)
    {
        await Task.Delay(TimeSpan.FromSeconds(T));
        canvas.alpha = state==true?1:0;
        canvas.blocksRaycasts=state;
    }

    [EasyButtons.Button]
    void SetStartValues()
    {
        canvas = gameObject.GetComponent<CanvasGroup>();
        CanvasOff(false,0);
        for (int i = 0; i < Listofanimation.Length; i++)
        {

            RectTransform selectedTransform = Listofanimation[i].rectTransform;

            if (Listofanimation[i].animationParts.PositionPropetiesAnim.IsPositionEnabled())
                selectedTransform.anchoredPosition = (Vector2)Listofanimation[i].animationParts.PositionPropetiesAnim.StartPos;

            if (Listofanimation[i].animationParts.ScalePropetiesAnim.IsScaleEnabled())
                selectedTransform.localScale = Listofanimation[i].animationParts.ScalePropetiesAnim.StartScale;

            if (Listofanimation[i].animationParts.RotationPropetiesAnim.IsRotationEnabled())
                selectedTransform.localEulerAngles = Listofanimation[i].animationParts.RotationPropetiesAnim.StartRot;

            if (Listofanimation[i].animationParts.FadePropetiesAnim.IsFadeEnabled())
            {
                if (Listofanimation[i].IsObjectOpened())
                    SetAlphaValue(selectedTransform.transform, Listofanimation[i].animationParts.FadePropetiesAnim.GetEndFadeValue());
                else
                    SetAlphaValue(selectedTransform.transform, Listofanimation[i].animationParts.FadePropetiesAnim.GetStartFadeValue());
            }
        }

    }

    [EasyButtons.Button]
    void SetEndValues()
    {
        canvas = gameObject.GetComponent<CanvasGroup>();
        CanvasOff(true,0);
        for (int i = 0; i < Listofanimation.Length; i++)
        {
            RectTransform selectedTransform = Listofanimation[i].rectTransform;

            if (Listofanimation[i].animationParts.PositionPropetiesAnim.IsPositionEnabled())
                selectedTransform.anchoredPosition = (Vector2)Listofanimation[i].animationParts.PositionPropetiesAnim.EndPos;

            if (Listofanimation[i].animationParts.ScalePropetiesAnim.IsScaleEnabled())
                selectedTransform.localScale = Listofanimation[i].animationParts.ScalePropetiesAnim.EndScale;

            if (Listofanimation[i].animationParts.RotationPropetiesAnim.IsRotationEnabled())
                selectedTransform.localEulerAngles = Listofanimation[i].animationParts.RotationPropetiesAnim.EndRot;

            if (Listofanimation[i].animationParts.FadePropetiesAnim.IsFadeEnabled())
            {
                if (Listofanimation[i].IsObjectOpened())
                    SetAlphaValue(selectedTransform.transform, Listofanimation[i].animationParts.FadePropetiesAnim.GetStartFadeValue());
                else
                    SetAlphaValue(selectedTransform.transform, Listofanimation[i].animationParts.FadePropetiesAnim.GetEndFadeValue());
            }
        }
    }

    void SetAlphaValue(Transform _objectToSetAlpha, float alphaValue)
    {
        if (_objectToSetAlpha.GetComponent<CanvasGroup>())
        {
            CanvasGroup GraphicElement = _objectToSetAlpha.GetComponent<CanvasGroup>();
            GraphicElement.alpha = alphaValue;
        }

        if (_objectToSetAlpha.childCount > 0)
        {
            for (int i = 0; i < _objectToSetAlpha.childCount; i++)
            {
                if (!_objectToSetAlpha.GetChild(i).GetComponent<ReferencedFrom>() || Listofanimation[i].animationParts.FadePropetiesAnim.IsFadeOverrideEnabled())
                {
                    SetAlphaValue(_objectToSetAlpha.GetChild(i), alphaValue);
                }
            }
        }
    }

}