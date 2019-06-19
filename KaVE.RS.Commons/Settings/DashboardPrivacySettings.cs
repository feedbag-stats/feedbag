using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Application.Settings;
using KaVE.Commons.Model.Events.Enums;
using KaVE.Commons.Model.Events.UserProfiles;

namespace KaVE.RS.Commons.Settings
{
    [SettingsKey(typeof(KaVESettings), "KaVE DashboardPrivacy Settings")]
    // WARNING: Do not change classname, as it is used to identify settings
    public class DashboardPrivacySettings
    {
        [SettingsEntry(false, "DashboardPrivacy: SharingDataEnabled")]
        public bool SharingDataEnabled;

        [SettingsEntry(false, "DashboardPrivacy: SharingGenericInteractionDataForFeedBagOnlyEnabled")]
        public bool SharingGenericInteractionDataForFeedBagOnlyEnabled;

        [SettingsEntry(false, "DashboardPrivacy: SharingGenericInteractionDataForResearchEnabled")]
        public bool SharingGenericInteractionDataForResearchEnabled;

        [SettingsEntry(false, "DashboardPrivacy: SharingGenericInteractionDataForFeedBagOnlyEnabled")]
        public bool SharingGenericInteractionDataForOpenDataSetEnabled;

        [SettingsEntry(false, "DashboardPrivacy: SharingProjectSpecificDataForFeedBagOnlyEnabled")]
        public bool SharingProjectSpecificDataForFeedBagOnlyEnabled;

        [SettingsEntry(false, "DashboardPrivacy: SharingProjectSpecificDataForResearchEnabled")]
        public bool SharingProjectSpecificDataForResearchEnabled;

        [SettingsEntry(false, "DashboardPrivacy: SharingProjectSpecificDataForOpenDataSetEnabled")]
        public bool SharingProjectSpecificDataForOpenDataSetEnabled;

        [SettingsEntry(false, "DashboardPrivacy: SharingSourceCodeForFeedBagOnlyEnabled")]
        public bool SharingSourceCodeForFeedBagOnlyEnabled;

        [SettingsEntry(false, "DashboardPrivacy: SharingSourceCodeForResearchEnabled")]
        public bool SharingSourceCodeForResearchEnabled;

        [SettingsEntry(false, "DashboardPrivacy: SharingSourceCodeForOpenDataSetEnabled")]
        public bool SharingSourceCodeForOpenDataSetEnabled;
    }
}
