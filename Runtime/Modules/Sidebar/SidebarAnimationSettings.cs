using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Intelmatix.Modules.Sidebar
{
    public class SidebarAnimationSettings : Singleton<SidebarAnimationSettings>
    {
        [SerializeField] private float backgroundAppearDuration = 3f;
        [SerializeField] private float backgroundCloseDuration = 3f;
        [SerializeField] private float contentAppearDelay = 1f;
        [SerializeField] private float contentAppearDuration = 1f;
        [SerializeField] private float contentCloseDuration = 1f;
        [SerializeField] private float delayBetweenCharts = 1f;

        public static float BackgroundAppearDuration => Instance.backgroundAppearDuration;
        public static float BackgroundCloseDuration => Instance.backgroundCloseDuration;
        public static float ContentAppearDelay => Instance.contentAppearDelay;
        public static float ContentAppearDuration => Instance.contentAppearDuration;
        public static float ContentCloseDuration => Instance.contentCloseDuration;
        public static float DelayBetweenCharts => Instance.delayBetweenCharts;
    }
}
