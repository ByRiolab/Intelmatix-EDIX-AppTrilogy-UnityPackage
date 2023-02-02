using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Intelmatix.Modules.Sidebar.Primitives.SidebarData;
using UnityEngine.UI;

namespace Intelmatix.Modules.Sidebar.Graphics
{
    public class TableChart : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Transform parentOfOptions;
        [Space]
        [Header("Components")]
        [SerializeField] private Components.RowHandler rowHandlerPrefab;

        [Header("Animation")]
        [SerializeField] private CanvasGroup canvasToAnimate;
        [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;

        [Header("Images")]
        private Sprite potatoImage;
        public void Display(DataTable tableChart)
        {
            this.name = "<table-chart> [" + tableChart.Title + "]";
            titleText.text = tableChart.Title;

            parentOfOptions.DestroyChildren();
            // if(tableChart.TableType == "time")

            rowHandlerPrefab.gameObject.SetActive(false);
            if (tableChart.TableType == "product")
            {
                foreach (var row in tableChart.Rows)
                {
                    var instance = Instantiate(rowHandlerPrefab, parentOfOptions);
                    instance.Display(row);
                    instance.gameObject.SetActive(true);
                }
                //force reset of vertical layout group
                verticalLayoutGroup.enabled = false;
                verticalLayoutGroup.enabled = true;
            }
        }

        private void OnEnable()
        {
            AnimationManager.AnimateIn(this.canvasToAnimate, direction: AnimationManager.Direction.Right, duration: AnimationSettings.Sidebar.ContentAppearDuration);
        }

        private void OnDisable()
        {
            AnimationManager.AnimateOut(this.canvasToAnimate, direction: AnimationManager.Direction.Right, duration: AnimationSettings.Sidebar.ContentCloseDuration);
        }
    }
}
