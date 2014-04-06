﻿using System;
using System.Windows.Controls.Primitives;
using JetBrains.Application;
using KaVE.Utils;
using KaVE.VsFeedbackGenerator.SessionManager.Presentation;
using KaVE.VsFeedbackGenerator.TrayNotification;
using KaVE.VsFeedbackGenerator.Utils;

namespace KaVE.VsFeedbackGenerator.SessionManager
{
    [ShellComponent]
    public class UploadReminder
    {
        private readonly ISettingsStore _settingsStore;
        private readonly SessionManagerWindowRegistrar _sessionWindowRegistrar;
        private readonly NotifyTrayIcon _taskbarIcon;
        private readonly ICallbackManager _callbackManager;

        public UploadReminder(ISettingsStore settingsStore, NotifyTrayIcon notify, ICallbackManager callbackManager, SessionManagerWindowRegistrar sessionWindowRegistrar)
        {
            _taskbarIcon = notify;
            _settingsStore = settingsStore;
            _callbackManager = callbackManager;
            _sessionWindowRegistrar = sessionWindowRegistrar;

            EnsureUploadSettingsInitialized();
            RegisterCallback();       
        }

        private void RegisterCallback()
        {
            var settings = _settingsStore.GetSettings<UploadSettings>();
            var nextNotificationTime = settings.LastNotificationDate.AddDays(1);
            _callbackManager.RegisterCallback(ShowNotificationAndUpdateSettings, nextNotificationTime, RegisterCallback);
        }

        private void EnsureUploadSettingsInitialized()
        {
            var settings = _settingsStore.GetSettings<UploadSettings>();
            if (settings.IsInitialized())
            {
                return;
            }
            settings.Initialize();
            _settingsStore.SetSettings(settings);
        }

        private void ShowNotificationAndUpdateSettings()
        {
            var settings = _settingsStore.GetSettings<UploadSettings>();

            if (OneWeekWithoutUpload(settings))
            {
                Invoke.OnSTA(
                   () => _taskbarIcon.ShowCustomNotification(
                       new HardBalloonPopup(
                           _sessionWindowRegistrar,
                           SessionManagerWindowActionHandler.ActionId),
                       PopupAnimation.Slide,
                       null));
            }
            else
            {
                Invoke.OnSTA(
                   () => _taskbarIcon.ShowCustomNotification(
                       new SoftBalloonPopup(
                           _sessionWindowRegistrar,
                           SessionManagerWindowActionHandler.ActionId), 
                       PopupAnimation.Slide,
                       null));
            }

            settings.LastNotificationDate = DateTime.Now;
            _settingsStore.SetSettings(settings);
        }

        private static bool OneWeekWithoutUpload(UploadSettings settings)
        {
            return settings.LastUploadDate.AddDays(7) < DateTime.Now;
        }
    }
}
