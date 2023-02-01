﻿using TMPro;
using UnityEngine;

namespace AwesomeCharts {
    public class TMProChartLabel : ChartLabel {

        public TMP_Text textLabel;

        public override void SetLabelColor (Color color) {
            textLabel.color = color;
        }

        public override void SetLabelText (string text) {
            //check if is a number
            float number;
            if (float.TryParse(text, out number)) {
                if(number > 1000000000) // use B
                    textLabel.text = (number / 1000000000).ToString("0.0") + "B";
                else if(number > 1000000) // use M
                    textLabel.text = (number / 1000000).ToString("0.0") + "M";
                else if(number > 1000) // use K
                    textLabel.text = (number / 1000).ToString("0.0") + "K";
                else
                    textLabel.text = number.ToString("0.0");
            }
            else if(text.Length > 5) {
                //show the first caracter and the next word
                string[] words = text.Split(' ');
                if(words.Length > 1)
                {
                    textLabel.text = words[0][0] + words[1];
                }
                else{
                    words = text.Split('_');
                    if(words.Length > 1)
                    {
                        textLabel.text = words[1];
                    }
                    else{
                        textLabel.text = text;
                    }
                }
            }
            else
                textLabel.text = text;
        }

        public override void SetLabelTextAlignment (TextAnchor anchor) {
            textLabel.alignment = GetTextMeshProAligment(anchor);
        }

        

        private TextAlignmentOptions GetTextMeshProAligment (TextAnchor anchor) {
            switch (anchor) {
                case TextAnchor.LowerCenter:
                    return TextAlignmentOptions.Bottom;
                case TextAnchor.LowerLeft:
                    return TextAlignmentOptions.BottomLeft;
                case TextAnchor.LowerRight:
                    return TextAlignmentOptions.BottomRight;
                case TextAnchor.MiddleCenter:
                    return TextAlignmentOptions.Midline;
                case TextAnchor.MiddleLeft:
                    return TextAlignmentOptions.MidlineLeft;
                case TextAnchor.MiddleRight:
                    return TextAlignmentOptions.MidlineRight;
                case TextAnchor.UpperCenter:
                    return TextAlignmentOptions.Top;
                case TextAnchor.UpperLeft:
                    return TextAlignmentOptions.TopLeft;
                case TextAnchor.UpperRight:
                    return TextAlignmentOptions.TopRight;
                default:
                    return TextAlignmentOptions.Midline;

            }
        }

        public override void SetLabelTextSize (int size) {
            textLabel.fontSize = size;
        }
    }
}