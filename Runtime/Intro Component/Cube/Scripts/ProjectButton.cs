using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Intelmatix
{
    [System.Serializable]
    public class ProjectDisplayInfo
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        /// <summary>
        /// Is this project the current one?
        /// </summary>
        [field: SerializeField] public bool IsThis { get; private set; }
    }
    [RequireComponent(typeof(CanvasGroup))]
    public class ProjectButton : MonoBehaviour, IPointerClickHandler
    {
        ProjectDisplayInfo project;
        [SerializeField] Image iconImage;
        [SerializeField] TMPro.TextMeshProUGUI nameText;

        public Image Icon => iconImage;

        private CanvasGroup canvasGroup;

        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (project.IsThis)
            {
                Screensaver.Instance.DeactivateScreensaver();
            }
        }

        public void SetData(ProjectDisplayInfo project)
        {
            this.project = project;

            iconImage.sprite = project.Icon;
            nameText.SetText(project.Name);

            canvasGroup.alpha = project.IsThis ? 1 : 0.2f;
        }
    }
}
