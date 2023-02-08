using Intelmatix.Tools;
using UnityEngine;


namespace Intelmatix
{
    public class Screensaver : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float delay = 1f;
        [SerializeField] private float fadeInDuration = 2f;

        [SerializeField] private float fadeOutDuration = 2f;
        [SerializeField] private float inactivityDuration = 10f; // tiempo de inactividad en segundos

        [SerializeField] private LeanTweenType fadeInEaseType = LeanTweenType.easeOutQuad;
        [SerializeField] private LeanTweenType fadeOutEaseType = LeanTweenType.easeOutQuad;

        [SerializeField] private CubeController cubeController;

        [SerializeField] private bool _startOnAwake = true;
        [SerializeField] private bool _isScreensaverActive = true;

        private float _timer;
        void Awake()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            _timer = 0f;

            if (_startOnAwake)
            {
                canvasGroup.alpha = 1;
                canvasGroup.blocksRaycasts = true;
                _isScreensaverActive = true;
                LeanTween.cancel(gameObject);
            }
        }

        void Update()
        {
            var isInteractive = Input.anyKey || Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2) || Input.touchCount > 0;

            if (isInteractive)
            {
                _timer = 0f;

            }
            if (_isScreensaverActive && isInteractive)
            {
                LeanTween.cancel(gameObject, true);
                cubeController.EvaluateCandidates();
                _isScreensaverActive = false;
                _timer = 0f;
                LeanTween.value(gameObject, UpdateAlpha, canvasGroup.alpha, 0, fadeOutDuration).setEase(fadeOutEaseType).setOnStart(() =>
                {
                    canvasGroup.blocksRaycasts = false;
                }).setDelay(delay);
            }
            else
            {
                _timer += Time.deltaTime;
                if (_timer >= inactivityDuration)
                {
                    LeanTween.cancel(gameObject, true);
                    LeanTween.value(gameObject, UpdateAlpha, canvasGroup.alpha, 1, fadeInDuration).setEase(fadeInEaseType)
                        .setOnComplete(() =>
                        {
                            Debug.Log("Screensaver active");
                            _isScreensaverActive = true;
                            canvasGroup.blocksRaycasts = true;
                        });
                }
            }
        }

        void UpdateAlpha(float alpha)
        {
            canvasGroup.alpha = alpha;
        }
    }

}

