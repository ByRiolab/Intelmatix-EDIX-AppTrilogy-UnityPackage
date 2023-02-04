
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Intelmatix.Data.StoreData;
using Intelmatix.Settings;

namespace Intelmatix.Examples.Templates
{
    public class FloatingCard : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasToAnimate;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image image;

        [Header("Images")]
        [SerializeField] private Sprite inventorySprite;
        [SerializeField] private Sprite marketingSprite;
        [SerializeField] private Sprite staffSprite;
        [SerializeField] private Sprite supplierSprite;

        public void SetData(Card card)
        {
            Debug.Log(card.Icon);
            this.text.text = card.Label;
            image.sprite = Resources.Load<Sprite>(card.Icon);

            // 'inventory' 'marketing' 'staff' 'supplier'
            switch (card.Icon)
            {
                case "inventory":
                    image.sprite = inventorySprite;
                    break;

                case "marketing":
                    image.sprite = marketingSprite;
                    break;

                case "staff":
                    image.sprite = staffSprite;
                    break;

                case "supplier":
                    image.sprite = supplierSprite;
                    break;

                default:
                    image.sprite = inventorySprite;
                    break;
            }
        }

        public void Show()
        {
            LeanTween.cancel(this.canvasToAnimate.gameObject);
            AnimationManager.AnimateIn(this.canvasToAnimate,
                direction: AnimationManager.Direction.Up,
                distance: SidebarAnimationSettings.NotificationAppearDistance
                ).setOnComplete(() =>
            {
                LeanTween.cancel(this.canvasToAnimate.gameObject);

                AnimationManager.AnimateOut(this.canvasToAnimate,
                    direction: AnimationManager.Direction.Up,
                    distance: SidebarAnimationSettings.NotificationCloseDistance,
                    delay: SidebarAnimationSettings.NotificationCloseDelay).setOnComplete(() =>
                {
                    Destroy(this.gameObject);
                });
            });
        }
    }

}