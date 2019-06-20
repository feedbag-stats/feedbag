using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Application.Settings;


namespace KaVE.RS.Commons.Settings
{
    [SettingsKey(typeof(KaVESettings), "KaVE DashboardDefaultPrivacy Settings")]
    // WARNING: Do not change classname, as it is used to identify settings
    public class DashboardDefaultPrivacySettings
    {
        [SettingsEntry(false, "DashboardDefaultPrivacy: SharingDataEnabled")]
        public bool SharingDataEnabled;

        [SettingsEntry(false, "DashboardDefaultPrivacy: SharingGenericInteractionDataForFeedBagOnlyEnabled")]
        public bool SharingGenericInteractionDataForFeedBagOnlyEnabled;

        [SettingsEntry(false, "DashboardDefaultPrivacy: SharingGenericInteractionDataForResearchEnabled")]
        public bool SharingGenericInteractionDataForResearchEnabled;

        [SettingsEntry(false, "DashboardDefaultPrivacy: SharingGenericInteractionDataForFeedBagOnlyEnabled")]
        public bool SharingGenericInteractionDataForOpenDataSetEnabled;

        [SettingsEntry(false, "DashboardDefaultPrivacy: SharingProjectSpecificDataForFeedBagOnlyEnabled")]
        public bool SharingProjectSpecificDataForFeedBagOnlyEnabled;

        [SettingsEntry(false, "DashboardDefaultPrivacy: SharingProjectSpecificDataForResearchEnabled")]
        public bool SharingProjectSpecificDataForResearchEnabled;

        [SettingsEntry(false, "DashboardDefaultPrivacy: SharingProjectSpecificDataForOpenDataSetEnabled")]
        public bool SharingProjectSpecificDataForOpenDataSetEnabled;

        [SettingsEntry(false, "DashboardDefaultPrivacy: SharingSourceCodeForFeedBagOnlyEnabled")]
        public bool SharingSourceCodeForFeedBagOnlyEnabled;

        [SettingsEntry(false, "DashboardDefaultPrivacy: SharingSourceCodeForResearchEnabled")]
        public bool SharingSourceCodeForResearchEnabled;

        [SettingsEntry(false, "DashboardDefaultPrivacy: SharingSourceCodeForOpenDataSetEnabled")]
        public bool SharingSourceCodeForOpenDataSetEnabled;
    }
}
