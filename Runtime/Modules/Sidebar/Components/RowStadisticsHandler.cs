using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static Intelmatix.Data.SidebarData;

namespace Intelmatix.Modules.Sidebar.Components
{
    public class RowStadisticsHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI valueText;

        [SerializeField] private Image image;

        [Header("Images")]
        [SerializeField] private Sprite increaseSprite;
        [SerializeField] private Sprite decreaseSprite;
      
        public void Display(RowStadistics row)
        {
            titleText.text = row.Title;
            string ValueText = row.Data.Value.GetNumberConversion();
            valueText.text = ValueText + " " + row.Data.Unit;

            switch (row.Data.Extra.Type)
            {
                case "increase":
                    image.sprite = increaseSprite;
                    valueText.color = Color.green;
                    break;
                case "decrease":
                    image.sprite = decreaseSprite;
                    valueText.color = Color.red;
                    break;
                
            }


        }

    }
}