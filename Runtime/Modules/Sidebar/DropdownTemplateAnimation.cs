using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Intelmatix
{
    public class DropdownTemplateAnimation : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasToAnimate;
        void Awake()
        {
            canvasToAnimate.alpha = 0;
            LeanTween.cancel(canvasToAnimate.gameObject);
            LeanTween.alphaCanvas(canvasToAnimate, 1, 1f).setEaseOutSine();
        }
        // void OnEnable()
        // {
        //     canvasToAnimate.alpha = 0;
        // }
        // void OnDisable()
        // {
        //     LeanTween.alphaCanvas(canvasToAnimate, 0, 0.5f);
        // }
    }
}
