using System.Collections;
using System.Collections.Generic;
using Intelmatix.Modules.Sidebar.Graphics;
using UnityEngine;
namespace Intelmatix.Modules.Sidebar
{
    public class BakgroundAnim : MonoBehaviour
    {
        [SerializeField] private RectTransform transformBackground;

        private void Start()
        {
            transformBackground.LeanMoveX(1884f, 0);
        }

        private void OnEnable()
        {
            //HideRect();
        }

        public void HideRect(float duration = 1f)
        {
            // Ocultar el rectángulo moviéndolo a la posición final (posición x = 1884)
            LeanTween.cancel(transformBackground);
            LeanTween.moveX(transformBackground, 1884f, duration).setEaseInOutSine();
        }

        public void ShowRect(float duration = 1f)
        {
            // Mostrar el rectángulo moviéndolo a la posición inicial (posición x = 0)
            LeanTween.cancel(transformBackground);
            LeanTween.moveX(transformBackground, 0f, duration).setEaseInOutSine();
        }
    }
}
