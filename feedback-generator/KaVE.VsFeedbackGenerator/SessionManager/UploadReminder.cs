﻿/*
 * Copyright 2014 Technische Universität Darmstadt
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *    http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * Contributors:
 *    - Uli Fahrer
 */

using System;
using JetBrains.Application;
using KaVE.VsFeedbackGenerator.TrayNotification;
using KaVE.VsFeedbackGenerator.Utils;

namespace KaVE.VsFeedbackGenerator.SessionManager
{
    [ShellComponent]
    public class UploadReminder
    {
        private const int WorkTimeStart = 10;
        private const int WorkTimeEnd = 16;

        private readonly ISettingsStore _settingsStore;
        private readonly NotifyTrayIcon _taskbarIcon;
        private readonly ICallbackManager _callbackManager;
        private readonly IDateUtils _dateUtils;
        private static readonly Random RandomGenerator = new Random();

        public UploadReminder(ISettingsStore settingsStore,
            NotifyTrayIcon notify,
            ICallbackManager callbackManager,
            IDateUtils dateUtils)
        {
            _taskbarIcon = notify;
            _settingsStore = settingsStore;
            _callbackManager = callbackManager;
            _dateUtils = dateUtils;

            EnsureUploadSettingsInitialized();
            RegisterCallback();
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

        private void RegisterCallback()
        {
            var nextNotificationTime = CalculateNextNotificationTime();
            _callbackManager.RegisterCallback(ShowNotificationAndUpdateSettings, nextNotificationTime, RegisterCallback);
        }

        private DateTime CalculateNextNotificationTime()
        {
            var settings = _settingsStore.GetSettings<UploadSettings>();
            var nextNotification = CreateRandomDateInWorkingHoursOfDayAfter(settings.LastNotificationDate);

            if (_dateUtils.Now < nextNotification)
            {
                return nextNotification;
            }
            if (_dateUtils.Now.Hour < WorkTimeEnd)
            {
                return _dateUtils.Now;
            }
            var tomorrow = CreateRandomDateInWorkingHoursOfDayAfter(_dateUtils.Now);
            return tomorrow;
        }

        private static DateTime CreateRandomDateInWorkingHoursOfDayAfter(DateTime baseDate)
        {
            var newDate = new DateTime(
                baseDate.Year,
                baseDate.Month,
                baseDate.Day,
                RandomGenerator.Next(WorkTimeStart, WorkTimeEnd),
                RandomGenerator.Next(0, 60),
                0).AddDays(1);
            return newDate;
        }

        private void ShowNotificationAndUpdateSettings()
        {
            // at this point, lastNotification is at least one day past
            if (OneWeekSinceLastUpload())
            {
                _taskbarIcon.ShowHardBalloonPopup();
                UpdateLastNotificationDate();
            }
            else if (OndDaySinceLastUpload())
            {
                _taskbarIcon.ShowSoftBalloonPopup();
                UpdateLastNotificationDate();
            }
        }

        private bool OndDaySinceLastUpload()
        {
            var settings = _settingsStore.GetSettings<UploadSettings>();
            return settings.LastUploadDate.AddDays(1) < _dateUtils.Now;
        }

        private void UpdateLastNotificationDate()
        {
            var settings = _settingsStore.GetSettings<UploadSettings>();
            settings.LastNotificationDate = _dateUtils.Now;
            _settingsStore.SetSettings(settings);
        }

        private bool OneWeekSinceLastUpload()
        {
            var settings = _settingsStore.GetSettings<UploadSettings>();
            return settings.LastUploadDate.AddDays(7) < _dateUtils.Now;
        }
    }
}