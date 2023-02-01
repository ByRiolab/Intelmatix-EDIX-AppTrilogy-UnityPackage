using System.Collections;
using System.Collections.Generic;
using Intelmatix.Modules.Sidebar.Graphics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace Intelmatix.Modules.Sidebar
{
    public class CloseSidebar : MonoBehaviour
    {

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Button button;

        public void Display(UnityAction onClick)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(onClick);
        }

        private void OnEnable()
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0;
                canvasGroup.blocksRaycasts = false;
                AnimationManager.AnimateIn(this.canvasGroup, direction: AnimationManager.Direction.Right);
            }
        }

        private void OnDisable()
        {
            if (canvasGroup != null)
            {
                AnimationManager.AnimateOut(this.canvasGroup, direction: AnimationManager.Direction.Right);
            }
        }

        public LTDescr Hide()
        {
            return AnimationManager.AnimateOut(this.canvasGroup, direction: AnimationManager.Direction.Right);
        }


    }
}
