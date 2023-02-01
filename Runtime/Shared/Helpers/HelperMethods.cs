using UnityEngine;
using System.Collections.Generic;

namespace Intelmatix
{
    public static class Helpers
    {
        public static void DestroyChildren(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        public static void DestroyChildren(this RectTransform transform)
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public static LTDescr BlockCanvas(this CanvasGroup canvasGroup, float duration = 0)//, bool interactable, bool blockRaycast)
        {
            Debug.Log("blocking raycast");
            canvasGroup.blocksRaycasts = true;
            return canvasGroup.LeanAlpha(1, duration);//.setOnStart(() => canvasGroup.blocksRaycasts = true);
        }

        public static LTDescr UnblockCanvas(this CanvasGroup canvasGroup, float duration = 0)//, bool interactable, bool blockRaycast)
        {
            Debug.Log("unblocking raycast");
            //canvasGroup.blocksRaycasts = false;
            return canvasGroup.LeanAlpha(0, duration).setOnComplete(() => canvasGroup.blocksRaycasts = false);
        }


        public static string GetNumberConversion(this float number)
        {
            if (number > 1000000000) // use B
                return (number / 1000000000).ToString("0.0") + "B";
            else if (number > 1000000) // use M
                return (number / 1000000).ToString("0.0") + "M";
            else if (number > 1000) // use K
                return (number / 1000).ToString("0.0") + "K";
            else
                return number.ToString("0.0");
        }

        public static void SetPositionY(this LineRenderer lineRenderer, float y, int index = 1)
        {
            var line = lineRenderer.GetPosition(index);
            lineRenderer.SetPosition(index, new Vector3(line.x, y, line.z));
        }
        public static float GetPositionY(this LineRenderer lineRenderer, int index = 1)
        {
            var line = lineRenderer.GetPosition(index);
            return line.y;
        }
    }

}