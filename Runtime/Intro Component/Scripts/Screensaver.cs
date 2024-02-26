using System.Collections.Generic;
using UnityEngine;

namespace Intelmatix
{
    public class Screensaver : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CubeBehaviorController cubeController;
        [SerializeField] private CanvasGroup screensaverCanvas;
        [SerializeField] private CanvasGroup logosCanvasGroup;
        [SerializeField] private CanvasGroup videoCanvas;
        [SerializeField] private CanvasGroup cubesCanvas;
        [SerializeField] private ProjectButton projectButtonTemplate;
        [SerializeField] private Canvas logosCanvas;
        [SerializeField] private ProjectDisplayInfo[] projects;

        [Header("Settings")]
        [SerializeField, Min(0)] private float inactivityDuration = 240f;

        [Header("Initialization")]
        [SerializeField] private bool startOnAwake = true;
        public static Screensaver Instance;

        private bool isScreensaverActive = true;
        private float inactivityTimer;
        private static bool projectOpened;
        private bool wasTouchingLastFrame;
        private readonly List<RectTransform> displayedProjects = new();

        private void Awake()
        {
            Instance = this;
            ResetInactivityTimer();

            if (startOnAwake)
                ActivateScreensaver();

            foreach (ProjectDisplayInfo project in projects)
            {
                var instance = Instantiate(projectButtonTemplate, logosCanvasGroup.transform);
                instance.SetData(project);
                displayedProjects.Add(instance.Icon.GetComponent<RectTransform>());
            }

        }

        private void Update()
        {
            bool touch = Input.touchCount > 0 && !wasTouchingLastFrame;
            wasTouchingLastFrame = Input.touchCount > 0;
            bool inputReceived = Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2) || touch;

            if (inputReceived)
            {
                ResetInactivityTimer();
            }

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
            cubeController.Stop();

            cubeController.MoveCubesToPositions(displayedProjects.ToArray(), logosCanvas.scaleFactor);

            logosCanvasGroup.LeanAlpha(1, 0.3f).setDelay(2.5f).setOnComplete(() =>
            {
                logosCanvasGroup.blocksRaycasts = true;

                videoCanvas.LeanAlpha(0.1f, 0.3f);
                cubesCanvas.LeanAlpha(0, 0.3f);
                cubeController.Deactivate();
            });
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
            logosCanvasGroup.LeanAlpha(0, 0.3f);
            ResetInactivityTimer();
            isScreensaverActive = true;
        }

        private void ResetInactivityTimer() => inactivityTimer = 0f;
    }
}
