using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Exoa.Responsive
{
    /// <summary>
    /// Add this component on any Responsive Container's children 
    /// if you want to customize it's resize behaviour
    /// </summary>
    public class ResponsiveItem : MonoBehaviour
    {
        public ResponsiveContainer.ResponsiveItemData settings;
    }
}
