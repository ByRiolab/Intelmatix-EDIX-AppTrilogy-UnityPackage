using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Intelmatix
{
    public class EDIXButton : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            ResetScreensaver();
        }

        [ContextMenu("Reset Screensaver")]
        public void ResetScreensaver()
        {
            Screensaver.Instance.ActivateScreensaver(true);
        }
    }
}
