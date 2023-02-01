using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSettings : Singleton<AnimationSettings>
{

    [Header("Settiings")]
    [SerializeField] private SidebarSettings sidebar;
    [SerializeField] private WorldDataSettings worldData;
    [SerializeField] private LocalDataSettings localData;

    public static SidebarSettings Sidebar => Instance.sidebar;
    public static WorldDataSettings WorldData => Instance.worldData;
    public static LocalDataSettings LocalData => Instance.localData;


    [System.Serializable]
    public class LocalDataSettings
    {
        [Header("Bar Settings")]
        [SerializeField] private float barAppearDuration = 2f;
        [SerializeField] private float barDisappearDuration = 1f;

        [Header("Tag Settings")]
        [SerializeField] private float tagAppearDuration = 2f;
        [SerializeField] private float tagDisappearDuration = 2f;
        [SerializeField] private float tagAppearDistance = 50f;
        [SerializeField] private float tagDisappearDistance = 50f;
        [SerializeField] private float tagAppearDelay = 1f;

        public float BarAppearDuration => barAppearDuration;
        public float BarDisappearDuration => barDisappearDuration;

        public float TagAppearDelay => tagAppearDelay;
        public float TagAppearDuration => tagAppearDuration;
        public float TagAppearDistance => tagAppearDistance;
        public float TagDisappearDuration => tagDisappearDuration;
        public float TagDisappearDistance => tagDisappearDistance;

    }
    [System.Serializable]
    public class WorldDataSettings
    {
        [Header("Bar Settings")]
        [SerializeField] private float barAppearDuration = 2f;
        [SerializeField] private float barCloseDuration = 1f;
        [SerializeField] private float barAppearDelay = 1f;
        [SerializeField] private LeanTweenType barAppearEaseType = LeanTweenType.easeInOutSine;
        [SerializeField] private LeanTweenType barCloseEaseType = LeanTweenType.easeInOutSine;

        public float BarAppearDuration => barAppearDuration;
        public float BarCloseDuration => barCloseDuration;
        public float BarAppearDelay => barAppearDelay;

        public LeanTweenType BarAppearEaseType => barAppearEaseType;
        public LeanTweenType BarCloseEaseType => barCloseEaseType;

        [Space(10)]
        [Header("Tag Settings")]
        [SerializeField] private float tagAppearDelay = 1f;
        [SerializeField] private float tagAppearDuration = 2f;
        [SerializeField] private float tagCloseDuration = 2f;
        [SerializeField] private float tagAppearDistance = 300f;
        [SerializeField] private float tagCloseDistance = 300f;

        public float TagAppearDelay => tagAppearDelay;
        public float TagAppearDuration => tagAppearDuration;
        public float TagAppearDistance => tagAppearDistance;

        public float TagCloseDuration => tagCloseDuration;
        public float TagCloseDistance => tagCloseDistance;

    }

    [System.Serializable]
    public class SidebarSettings
    {
        [SerializeField] private float backgroundAppearDuration = 3f;
        [SerializeField] private float backgroundCloseDuration = 3f;
        [SerializeField] private float contentAppearDelay = 1f;
        [SerializeField] private float contentAppearDuration = 1f;
        [SerializeField] private float contentCloseDuration = 1f;
        [SerializeField] private float delayBetweenCharts = 1f;

        public float BackgroundAppearDuration => backgroundAppearDuration;
        public float BackgroundCloseDuration => backgroundCloseDuration;
        public float ContentAppearDelay => contentAppearDelay;
        public float ContentAppearDuration => contentAppearDuration;
        public float ContentCloseDuration => contentCloseDuration;
        public float DelayBetweenCharts => delayBetweenCharts;

    }
}
