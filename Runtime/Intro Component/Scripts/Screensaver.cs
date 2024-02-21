using UnityEngine;

namespace Intelmatix
{
    public class Screensaver : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CubeBehaviorController cubeController;
        [SerializeField] private CanvasGroup screensaverCanvas;
        [SerializeField] private CanvasGroup buttonsCanvas;
        [SerializeField] private CanvasGroup videoCanvas;
        [SerializeField] private CanvasGroup cubesCanvas;

        [Header("Settings")]
        [SerializeField, Min(0)] private float inactivityDuration = 240f;

        [Header("Initialization")]
        [SerializeField] private bool startOnAwake = true;

        private bool isScreensaverActive = true;
        private float inactivityTimer;
        private static bool projectOpened;

        private void Awake()
        {
            ResetInactivityTimer();

            if (startOnAwake)
                ActivateScreensaver();
        }

        private void Update()
        {
            bool inputReceived = Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2);

            if (isScreensaverActive && inputReceived)
            {
                if (projectOpened)
                    DeactivateScreensaver();
                else
                    ShowButtons();
            }
            else if (!isScreensaverActive)
            {
                inactivityTimer += Time.deltaTime;
                if (inactivityTimer >= inactivityDuration)
                    ActivateScreensaver();
            }

            screensaverCanvas.blocksRaycasts = screensaverCanvas.alpha > 0.1f;
        }

        private void ShowButtons()
        {
            buttonsCanvas.LeanAlpha(1, 0.3f).setOnComplete(() => buttonsCanvas.blocksRaycasts = true);
            cubeController.Deactivate();
            videoCanvas.LeanAlpha(0.1f, 0.3f);
            cubesCanvas.LeanAlpha(0, 0.3f);
        }

        public void ActivateScreensaver()
        {
            ResetScreensaver();
            screensaverCanvas.LeanAlpha(1, 0.5f);
        }

        public void DeactivateScreensaver()
        {
            projectOpened = true;
            isScreensaverActive = false;
            screensaverCanvas.LeanAlpha(0, 0.5f);
            cubeController.Deactivate();
        }

        public void ResetScreensaver()
        {
            cubesCanvas.LeanAlpha(1, 0.3f);
            videoCanvas.LeanAlpha(1, 0.3f);
            cubeController.Collapse();
            buttonsCanvas.LeanAlpha(0, 0.3f);
            ResetInactivityTimer();
            isScreensaverActive = true;
        }

        private void ResetInactivityTimer() => inactivityTimer = 0f;
    }
}
