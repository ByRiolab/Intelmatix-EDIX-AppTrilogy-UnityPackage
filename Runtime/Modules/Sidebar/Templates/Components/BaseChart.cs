using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AwesomeCharts;
using TMPro;
using System.Linq;
using Intelmatix.Data;
using Intelmatix.Settings;
using Intelmatix.Modules.Sidebar.Components;

namespace Intelmatix.Templates
{
    public abstract class BaseChart<T> : MonoBehaviour
    {
        public abstract void Display(T data);
    }
}
