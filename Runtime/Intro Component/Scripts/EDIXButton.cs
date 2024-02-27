using UnityEngine;
using UnityEngine.EventSystems;

namespace Intelmatix
{
    /// <summary>
    /// Main button shown in the application that allows the user to go back to main menu.
    /// </summary>
    public class EDIXButton : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData _) => ResetScreensaver();
        public void ResetScreensaver()
        {
            Screensaver.Instance.ActivateScreensaver(true);
        }
    }
}
