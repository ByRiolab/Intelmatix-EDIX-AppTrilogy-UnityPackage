using UnityEngine;
using UnityEngine.UI;

namespace Intelmatix.Modules.UI.Components
{
    public class MultiImageTargetGraphics : MonoBehaviour
    {
        [SerializeField] private Graphic[] targetGraphics;

        public Graphic[] GetTargetGraphics => targetGraphics;
    }
}