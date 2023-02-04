using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static Intelmatix.Data.SidebarData;

namespace Intelmatix.Modules.Sidebar.Components
{
    public class RowHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI valueText;

        [SerializeField] private Image image;

        [Header("Images")]
        [SerializeField] private Sprite tomatoSprite;
        [SerializeField] private Sprite potatoSprite;
        [SerializeField] private Sprite lettuceSprite;
        [SerializeField] private Sprite onionSprite;
        [SerializeField] private Sprite pepperSprite;
        [SerializeField] private Sprite mshroomSprite;

        public void Display(RowInfo row)
        {
            titleText.text = row.Title;
            valueText.text = row.Data.Value + " " + row.Data.Unit.ToLower();

            switch (row.Title)
            {
                case "Tomato":
                    image.sprite = tomatoSprite;
                    break;
                case "Potato":
                    image.sprite = potatoSprite;
                    break;
                case "Lettuce":
                    image.sprite = lettuceSprite;
                    break;
                case "Onion":
                    image.sprite = onionSprite;
                    break;
                case "Pepper":
                    image.sprite = pepperSprite;
                    break;
                case "Mshroom":
                    image.sprite = mshroomSprite;
                    titleText.text = "Mushroom";
                    break;
                default:
                    image.sprite = tomatoSprite;
                    break;
            }


        }

    }
}