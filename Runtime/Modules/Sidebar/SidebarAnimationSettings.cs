using System.Collections;
using System.Collections.Generic;
using Intelmatix.Base;
using UnityEngine;

namespace Intelmatix.Settings
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


        [Space]

        [SerializeField] private float notificationAppearDuration = 1f;
        [SerializeField] private float notificationCloseDuration = 1f;
        [SerializeField] private float notificationAppearDelay = 1f;
        [SerializeField] private float notificationCloseDelay = 1f;
        [SerializeField] private float notificationDelayBetweenNotifications = 1f;
        [SerializeField] private float notificationAppearDistance = 4;
        [SerializeField] private float notificationCloseDistance = 20f;

        public static float NotificationAppearDuration => Instance.notificationAppearDuration;
        public static float NotificationCloseDuration => Instance.notificationCloseDuration;
        public static float NotificationAppearDelay => Instance.notificationAppearDelay;
        public static float NotificationCloseDelay => Instance.notificationCloseDelay;
        public static float NotificationDelayBetweenNotifications => Instance.notificationDelayBetweenNotifications;
        public static float NotificationAppearDistance => Instance.notificationAppearDistance;
        public static float NotificationCloseDistance => Instance.notificationCloseDistance;


    }
}
