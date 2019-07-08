/*
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
 */

using JetBrains.Application;
using KaVE.Commons.Utils;

namespace KaVE.RS.Commons.Settings
{
    public interface IDashboardPrivacySettingsUtils
    {
        void EnsureSharingDataEnabled();
        //bool HasBeenAskedToFillProfile();
        bool CreateNewSharingDataEnabled();
        DashboardPrivacySettings GetSettings();
        void StoreSettings(DashboardPrivacySettings userProfileSettings);
    }

    [ShellComponent]
    public class DashboardPrivacySettingsUtils : IDashboardPrivacySettingsUtils
    {
        private readonly ISettingsStore _settingsStore;
        private readonly IRandomizationUtils _rnd;

        public DashboardPrivacySettingsUtils(ISettingsStore settingsStore, IRandomizationUtils rnd)
        {
            _settingsStore = settingsStore;
            _rnd = rnd;
        }

        public void EnsureSharingDataEnabled()
        {
            var settings = GetSettings();
            if (!HasSharingDataEnabled())
            {
            settings.SharingDataEnabled = false;
            StoreSettings(settings);
            }
        }

        /*public bool HasBeenAskedToFillProfile()
        {
            return GetSettings().HasBeenAskedToFillProfile;
        }*/

        public bool HasSharingDataEnabled()
        {
            return GetSettings().SharingDataEnabled;
        }

        public bool CreateNewSharingDataEnabled()
        {
            return false;
        }

        public DashboardPrivacySettings GetSettings()
        {
            return _settingsStore.GetSettings<DashboardPrivacySettings>();
        }

        public void StoreSettings(DashboardPrivacySettings settings)
        {
            _settingsStore.SetSettings(settings);
        }
    }
}