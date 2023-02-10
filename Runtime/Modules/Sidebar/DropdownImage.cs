using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Intelmatix
{
    public class DropdownImage : MonoBehaviour
    {

        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Sprite tomatoSprite;
        [SerializeField] private Sprite potatoSprite;
        [SerializeField] private Sprite lettuceSprite;
        [SerializeField] private Sprite onionSprite;
        [SerializeField] private Sprite pepperSprite;
        [SerializeField] private Sprite mshroomSprite;
        [SerializeField] private Sprite defaultSprite;


        // Start is called before the first frame update
        void Start()
        {
            Display();
        }

        public void Display()
        {
           

            switch (text.text)
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
                    break;
                default:
                    image.sprite = defaultSprite;
                    break;
            }


        }
    }
}
