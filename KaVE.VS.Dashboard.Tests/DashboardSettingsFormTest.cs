using System;
using System.Windows.Forms;
using Dashboard;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using KaVE.RS.Commons;
using KaVE.Commons;
using KaVE.RS.Commons.Settings;
using KaVE.Commons.Utils;
using Moq;
using KaVE.Commons.Model.Events.UserProfiles;
using KaVE.VS.FeedbackGenerator.Generators;
using KaVE.RS.Commons.Utils;
using System.Collections.Generic;
using KaVE.Commons.Model.Events;
using KaVE.Commons.Model.Events.VisualStudio;

namespace KaVE.VS.Dashboard.Tests
{
    [TestClass]
    public class DashboardSettingsFormTest
    {
        private readonly DashboardSettingsForm _dashboardSettingsForm = new DashboardSettingsForm(true);
        private readonly ComboBox _comboBox1 = new ComboBox();
        private readonly string _myCurrentSolutionId = "1";

        private ISettingsStore _settingsStore;
        private UserProfileSettings _settings;
        private DashboardPrivacySettings _settingsDashboard;
        private DashboardDefaultPrivacySettings _settingsDefault;
        private IRandomizationUtils _rnd;
        private Guid _rndGuid;

        private UserProfileSettingsUtils _sut;

        public DashboardSettingsFormTest()
        {
            _dashboardSettingsForm.SetCurrentPrivacySettingsJObjectId(_myCurrentSolutionId);

            _comboBox1.AllowDrop = true;
            _comboBox1.Font = new System.Drawing.Font(
                "Microsoft Sans Serif",
                16F,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                ((byte) (0)));
            _comboBox1.FormattingEnabled = true;
            _comboBox1.Items.AddRange(
                new object[]
                {
                    "Solution 1",
                    "Solution 2"
                });
            _comboBox1.Location = new System.Drawing.Point(432, 41);
            _comboBox1.Name = "comboBox1";
            _comboBox1.Size = new System.Drawing.Size(345, 45);
            _comboBox1.TabIndex = 28;
            _comboBox1.Text = "Solution";

            _dashboardSettingsForm.SetCombobox(_comboBox1);
            _dashboardSettingsForm.InitializePrivacySettingsJObject();

            _settings = new UserProfileSettings();
            _settingsDashboard = new DashboardPrivacySettings();
            _settingsDefault = new DashboardDefaultPrivacySettings();
            _settingsStore = Mock.Of<ISettingsStore>();
            Mock.Get(_settingsStore).Setup(ss => ss.GetSettings<UserProfileSettings>()).Returns(_settings);
            Mock.Get(_settingsStore).Setup(ss => ss.GetSettings<DashboardPrivacySettings>()).Returns(_settingsDashboard);
            Mock.Get(_settingsStore).Setup(ss => ss.GetSettings<DashboardDefaultPrivacySettings>()).Returns(_settingsDefault);

            _rndGuid = Guid.NewGuid();
            _rnd = Mock.Of<IRandomizationUtils>();
            Mock.Get(_rnd).Setup(r => r.GetRandomGuid()).Returns(_rndGuid);

            _sut = new UserProfileSettingsUtils(_settingsStore, _rnd);
        }

        public void TestSaveDefaultSettings()
        {
            //_dashboardSettingsForm.GetDefaultPrivacySettingsJobject()["Enabled"] = true;
            //_dashboardSettingsForm.GetDefaultPrivacySettingsJobject()["FeedBagGenericInteraction"] = true;
            //_dashboardSettingsForm.GetDefaultPrivacySettingsJobject()["ResearchGenericInteraction"] = true;
            _dashboardSettingsForm.GetPrivacySettingJson()["1"]["Enabled"] = true;
            _dashboardSettingsForm.GetPrivacySettingJson()["1"]["FeedBagGenericInteraction"] = true;
            _dashboardSettingsForm.GetPrivacySettingJson()["1"]["ResearchGenericInteraction"] = true;

            ISettingsStore defaultSettingsStore = _dashboardSettingsForm.SafeDefaultSettingsTesting(_settingsStore);

            /*JObject expectedDefaultSettingsJObect = JObject.Parse(
                @"{'Solution': 'Solution 2', " +
                "'Enabled': true," +
                "'FeedBagGenericInteraction': true," +
                "'FeedBagProjectSpecific': false," +
                "'FeedBagSourceCode': false," +
                "'ResearchGenericInteraction': true," +
                "'ResearchProjectSpecific': false," +
                "'ResearchSourceCode': false," +
                "'OpenDataGenericInteraction': false," +
                "'OpenDataProjectSpecific': false," +
                "'OpenDataSourceCode': false" +
                "}");

            JObject resultDefaultPrivacySettingsJson = _dashboardSettingsForm.GetDefaultPrivacySettingsJobject();
            Assert.AreEqual(resultDefaultPrivacySettingsJson.ToString(), expectedDefaultSettingsJObect.ToString());*/

            Assert.Equals(true, defaultSettingsStore.GetSettings<DashboardDefaultPrivacySettings>().SharingDataEnabled);
            Assert.Equals(true, defaultSettingsStore.GetSettings<DashboardDefaultPrivacySettings>().SharingGenericInteractionDataForFeedBagOnlyEnabled);
            Assert.Equals(true, defaultSettingsStore.GetSettings<DashboardDefaultPrivacySettings>().SharingProjectSpecificDataForFeedBagOnlyEnabled);
            Assert.Equals(false, defaultSettingsStore.GetSettings<DashboardDefaultPrivacySettings>().SharingSourceCodeForFeedBagOnlyEnabled);

            Assert.Equals(false, defaultSettingsStore.GetSettings<DashboardDefaultPrivacySettings>().SharingDataEnabled);
            Assert.Equals(false, defaultSettingsStore.GetSettings<DashboardDefaultPrivacySettings>().SharingGenericInteractionDataForResearchEnabled );
            Assert.Equals(false, defaultSettingsStore.GetSettings<DashboardDefaultPrivacySettings>().SharingProjectSpecificDataForResearchEnabled);
            Assert.Equals(false, defaultSettingsStore.GetSettings<DashboardDefaultPrivacySettings>().SharingSourceCodeForResearchEnabled);

            Assert.Equals(false, defaultSettingsStore.GetSettings<DashboardDefaultPrivacySettings>().SharingDataEnabled);
            Assert.Equals(false, defaultSettingsStore.GetSettings<DashboardDefaultPrivacySettings>().SharingGenericInteractionDataForOpenDataSetEnabled);
            Assert.Equals(false, defaultSettingsStore.GetSettings<DashboardDefaultPrivacySettings>().SharingProjectSpecificDataForOpenDataSetEnabled);
            Assert.Equals(false, defaultSettingsStore.GetSettings<DashboardDefaultPrivacySettings>().SharingSourceCodeForOpenDataSetEnabled);

            _dashboardSettingsForm.GetPrivacySettingJson()["1"]["Enabled"] = false;
            _dashboardSettingsForm.GetPrivacySettingJson()["1"]["FeedBagGenericInteraction"] = false;
            _dashboardSettingsForm.GetPrivacySettingJson()["1"]["ResearchGenericInteraction"] = false;

        }

        [TestMethod]
        public void TestEnableCheckBoxDependingOnSelection()
        {
            JObject resultPrivacySettingsJson = _dashboardSettingsForm.GetPrivacySettingJson();

            //false in this case
            _dashboardSettingsForm.EnableCheckBoxDependingOnSelection((JObject) resultPrivacySettingsJson["0"]);

            JObject expectedJObect1 = JObject.Parse(
                @"{'0':" +
                "{'Solution': 'Solution 1', " +
                "'Enabled': false," +
                "'FeedBagGenericInteraction': false," +
                "'FeedBagProjectSpecific': false," +
                "'FeedBagSourceCode': false," +
                "'ResearchGenericInteraction': false," +
                "'ResearchProjectSpecific': false," +
                "'ResearchSourceCode': false," +
                "'OpenDataGenericInteraction': false," +
                "'OpenDataProjectSpecific': false," +
                "'OpenDataSourceCode': false" +
                "}," +
                "'1':" +
                "{'Solution': 'Solution 2', " +
                "'Enabled': false," +
                "'FeedBagGenericInteraction': false," +
                "'FeedBagProjectSpecific': false," +
                "'FeedBagSourceCode': false," +
                "'ResearchGenericInteraction': false," +
                "'ResearchProjectSpecific': false," +
                "'ResearchSourceCode': false," +
                "'OpenDataGenericInteraction': false," +
                "'OpenDataProjectSpecific': false," +
                "'OpenDataSourceCode': false" +
                "}}");

            Assert.AreEqual(resultPrivacySettingsJson.ToString(), expectedJObect1.ToString());

            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchProjectSpecific().Enabled, false);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchGenericInteraction().Enabled, false);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchSourceCode().Enabled, false);

            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataGenericInteraction().Enabled, false);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataProjectSpecific().Enabled, false);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataSourceCode().Enabled, false);

            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchProjectSpecific().Checked, false);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchGenericInteraction().Checked, false);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchSourceCode().Checked, false);

            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataGenericInteraction().Checked, false);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataProjectSpecific().Checked, false);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataSourceCode().Checked, false);


            // change selection
            JObject newSelection = JObject.Parse(
                @"{'0':" +
                "{'Solution': 'Solution 1', " +
                "'Enabled': true," +
                "'FeedBagGenericInteraction': false," +
                "'FeedBagProjectSpecific': false," +
                "'FeedBagSourceCode': false," +
                "'ResearchGenericInteraction': false," +
                "'ResearchProjectSpecific': false," +
                "'ResearchSourceCode': false," +
                "'OpenDataGenericInteraction': false," +
                "'OpenDataProjectSpecific': false," +
                "'OpenDataSourceCode': false" +
                "}," +
                "'1':" +
                "{'Solution': 'Solution 2', " +
                "'Enabled': false," +
                "'FeedBagGenericInteraction': false," +
                "'FeedBagProjectSpecific': false," +
                "'FeedBagSourceCode': false," +
                "'ResearchGenericInteraction': false," +
                "'ResearchProjectSpecific': false," +
                "'ResearchSourceCode': false," +
                "'OpenDataGenericInteraction': false," +
                "'OpenDataProjectSpecific': false," +
                "'OpenDataSourceCode': false" +
                "}}");

            resultPrivacySettingsJson = newSelection;
            _dashboardSettingsForm.SetPrivacySettingsJOject(newSelection);

            JObject expJObject2 = JObject.Parse(
                @"{'0':" +
                "{'Solution': 'Solution 1', " +
                "'Enabled': true," +
                "'FeedBagGenericInteraction': false," +
                "'FeedBagProjectSpecific': false," +
                "'FeedBagSourceCode': false," +
                "'ResearchGenericInteraction': false," +
                "'ResearchProjectSpecific': false," +
                "'ResearchSourceCode': false," +
                "'OpenDataGenericInteraction': false," +
                "'OpenDataProjectSpecific': false," +
                "'OpenDataSourceCode': false" +
                "}," +
                "'1':" +
                "{'Solution': 'Solution 2', " +
                "'Enabled': false," +
                "'FeedBagGenericInteraction': false," +
                "'FeedBagProjectSpecific': false," +
                "'FeedBagSourceCode': false," +
                "'ResearchGenericInteraction': false," +
                "'ResearchProjectSpecific': false," +
                "'ResearchSourceCode': false," +
                "'OpenDataGenericInteraction': false," +
                "'OpenDataProjectSpecific': false," +
                "'OpenDataSourceCode': false" +
                "}}");

            _dashboardSettingsForm.SetCurrentPrivacySettingsJObjectId("0");
            _dashboardSettingsForm.EnableCheckBoxDependingOnSelection((JObject) newSelection["0"]);

            Assert.AreEqual(resultPrivacySettingsJson.ToString(), expJObject2.ToString());

            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchProjectSpecific().Enabled, true);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchGenericInteraction().Enabled, true);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchSourceCode().Enabled, true);

            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataGenericInteraction().Enabled, true);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataProjectSpecific().Enabled, true);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataSourceCode().Enabled, true);


        }

        [TestMethod]
        public void TestCheckCheckBoxesAccodingToSelection()
        {

            var dashboardSettingsForm = new DashboardSettingsForm();

            // change selection
            JObject newSelection = JObject.Parse(
                @"{'0':" +
                "{'Solution': 'Solution 1', " +
                "'Enabled': true," +
                "'FeedBagGenericInteraction': true," +
                "'FeedBagProjectSpecific': false," +
                "'FeedBagSourceCode': true," +
                "'ResearchGenericInteraction': true," +
                "'ResearchProjectSpecific': true," +
                "'ResearchSourceCode': false," +
                "'OpenDataGenericInteraction': true," +
                "'OpenDataProjectSpecific': false," +
                "'OpenDataSourceCode': false" +
                "}," +
                "'1':" +
                "{'Solution': 'Solution 2', " +
                "'Enabled': false," +
                "'FeedBagGenericInteraction': false," +
                "'FeedBagProjectSpecific': false," +
                "'FeedBagSourceCode': false," +
                "'ResearchGenericInteraction': false," +
                "'ResearchProjectSpecific': false," +
                "'ResearchSourceCode': false," +
                "'OpenDataGenericInteraction': false," +
                "'OpenDataProjectSpecific': false," +
                "'OpenDataSourceCode': false" +
                "}}");

            dashboardSettingsForm.SetPrivacySettingsJOject(newSelection);

            dashboardSettingsForm.SetCurrentPrivacySettingsJObjectId("0");
            dashboardSettingsForm.CheckCheckBoxesAccodingToSelection((JObject) newSelection["0"]);

            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxFeedBagOnlyGenericInteraction().Checked, true);
            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxFeedBagOnlyProjectSpecific().Checked, true);
            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxFeedBagOnlySourceCode().Checked, true);

            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxResearchGenericInteraction().Checked, true);
            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxResearchProjectSpecific().Checked, true);
            Assert.AreEqual(false, dashboardSettingsForm.GetCheckBoxResearchSourceCode().Checked);

            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxOpenDataGenericInteraction().Checked, true);
            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxOpenDataProjectSpecific().Checked, false);
            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxOpenDataSourceCode().Checked, false);
        }

        [TestMethod]
        public void TestLoadDefaultSettingsForCurrentSolution()
        {
            _dashboardSettingsForm.SetCurrentPrivacySettingsJObjectId("1");

            JObject defaultPrivacySettings = JObject.Parse(
                "{'Solution': 'Solution 2', " +
                "'Enabled': true," +
                "'FeedBagGenericInteraction': false," +
                "'FeedBagProjectSpecific': false," +
                "'FeedBagSourceCode': false," +
                "'ResearchGenericInteraction': true," +
                "'ResearchProjectSpecific': true," +
                "'ResearchSourceCode': true," +
                "'OpenDataGenericInteraction': true," +
                "'OpenDataProjectSpecific': false," +
                "'OpenDataSourceCode': false" +
                "}");

            _dashboardSettingsForm.SetDefaultPrivacySettingsJObject(defaultPrivacySettings);

            _settingsDefault.SharingDataEnabled = true;
            _settingsDefault.SharingGenericInteractionDataForFeedBagOnlyEnabled = true;
            _settingsDefault.SharingGenericInteractionDataForResearchEnabled = true;
            _settingsDefault.SharingGenericInteractionDataForOpenDataSetEnabled = false;
            _settingsDefault.SharingProjectSpecificDataForFeedBagOnlyEnabled = true;
            _settingsDefault.SharingProjectSpecificDataForResearchEnabled = false;
            _settingsDefault.SharingProjectSpecificDataForOpenDataSetEnabled = false;
            _settingsDefault.SharingSourceCodeForFeedBagOnlyEnabled = true;
            _settingsDefault.SharingSourceCodeForResearchEnabled = true;
            _settingsDefault.SharingSourceCodeForOpenDataSetEnabled = true;

            _settingsStore.SetSettings<DashboardDefaultPrivacySettings>(_settingsDefault);

            _dashboardSettingsForm.LoadDefaultSettingsForCurrentSolutionTest(_settingsStore);

            // Checked
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxEnableDataCollection().Checked, true);

            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxFeedBagOnlyGenericInteraction().Checked, true);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxFeedBagOnlyProjectSpecific().Checked, true);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxFeedBagOnlySourceCode().Checked, true);

            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchGenericInteraction().Checked, true);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchProjectSpecific().Checked, false);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchSourceCode().Checked, true);

            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataGenericInteraction().Checked, false);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataProjectSpecific().Checked, false);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataSourceCode().Checked, true);

            // Enabled
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxEnableDataCollection().Enabled, true);

            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxFeedBagOnlyGenericInteraction().Enabled, false);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxFeedBagOnlyProjectSpecific().Enabled, false);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxFeedBagOnlySourceCode().Enabled, false);

            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchGenericInteraction().Enabled, true);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchProjectSpecific().Enabled, true);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchSourceCode().Enabled, true);

            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataGenericInteraction().Enabled, true);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataProjectSpecific().Enabled, true);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataSourceCode().Enabled, true);

        }

        [TestMethod]
        public void TestInitializePrivacySettings()
        {


            JObject expectedJObect = JObject.Parse(
                @"{'0':" +
                "{'Solution': 'Solution 1', " +
                "'Enabled': false," +
                "'FeedBagGenericInteraction': false," +
                "'FeedBagProjectSpecific': false," +
                "'FeedBagSourceCode': false," +
                "'ResearchGenericInteraction': false," +
                "'ResearchProjectSpecific': false," +
                "'ResearchSourceCode': false," +
                "'OpenDataGenericInteraction': false," +
                "'OpenDataProjectSpecific': false," +
                "'OpenDataSourceCode': false" +
                "}," +
                "'1':" +
                "{'Solution': 'Solution 2', " +
                "'Enabled': false," +
                "'FeedBagGenericInteraction': false," +
                "'FeedBagProjectSpecific': false," +
                "'FeedBagSourceCode': false," +
                "'ResearchGenericInteraction': false," +
                "'ResearchProjectSpecific': false," +
                "'ResearchSourceCode': false," +
                "'OpenDataGenericInteraction': false," +
                "'OpenDataProjectSpecific': false," +
                "'OpenDataSourceCode': false" +
                "}}");

            JObject resultPrivacySettingsJson = _dashboardSettingsForm.GetPrivacySettingJson();
            Assert.AreEqual(resultPrivacySettingsJson.ToString(), expectedJObect.ToString());
        }

        [TestMethod]
        public void TestCheckBoxResearchGenericInteraction_CheckedChanged()
        {
            _dashboardSettingsForm.SetCurrentPrivacySettingsJObjectId("0");

            JObject currentSelection = JObject.Parse(
                @"{'0':{'Solution': 'Solution 1', " +
                "'Enabled': false," +
                "'FeedBagGenericInteraction': false," +
                "'FeedBagProjectSpecific': false," +
                "'FeedBagSourceCode': false," +
                "'ResearchGenericInteraction': false," +
                "'ResearchProjectSpecific': false," +
                "'ResearchSourceCode': false," +
                "'OpenDataGenericInteraction': false," +
                "'OpenDataProjectSpecific': false," +
                "'OpenDataSourceCode': false" +
                "}}");

            _dashboardSettingsForm.SetPrivacySettingsJOject(currentSelection);

            _dashboardSettingsForm.GetCheckBoxResearchGenericInteraction().Checked = true;
            _dashboardSettingsForm.GetCheckBoxOpenDataGenericInteraction().Checked = true;

            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchGenericInteraction().Checked, true);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataGenericInteraction().Checked, true);

            _dashboardSettingsForm.GetCheckBoxResearchGenericInteraction().Checked = false;

            // check if research is unchecked that open data is unchecked as well
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchGenericInteraction().Checked, false);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataGenericInteraction().Checked, false);

        }

        [TestMethod]
        public void TestCheckBoxResearchProjectSpecific_CheckedChanged()
        {
            _dashboardSettingsForm.SetCurrentPrivacySettingsJObjectId("0");

            JObject currentSelection = JObject.Parse(
                @"{'0':{'Solution': 'Solution 1', " +
                "'Enabled': false," +
                "'FeedBagGenericInteraction': false," +
                "'FeedBagProjectSpecific': false," +
                "'FeedBagSourceCode': false," +
                "'ResearchGenericInteraction': false," +
                "'ResearchProjectSpecific': false," +
                "'ResearchSourceCode': false," +
                "'OpenDataGenericInteraction': false," +
                "'OpenDataProjectSpecific': false," +
                "'OpenDataSourceCode': false" +
                "}}");

            _dashboardSettingsForm.SetPrivacySettingsJOject(currentSelection);

            _dashboardSettingsForm.GetCheckBoxResearchProjectSpecific().Checked = true;
            _dashboardSettingsForm.GetCheckBoxOpenDataProjectSpecific().Checked = true;

            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchProjectSpecific().Checked, true);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataProjectSpecific().Checked, true);

            _dashboardSettingsForm.GetCheckBoxResearchProjectSpecific().Checked = false;

            // check if research is unchecked that open data is unchecked as well
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchProjectSpecific().Checked, false);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataProjectSpecific().Checked, false);

        }

        [TestMethod]
        public void TestCheckBoxResearchSourceCode_CheckedChanged()
        {
            _dashboardSettingsForm.SetCurrentPrivacySettingsJObjectId("0");

            JObject currentSelection = JObject.Parse(
                @"{'0':{'Solution': 'Solution 1', " +
                "'Enabled': false," +
                "'FeedBagGenericInteraction': false," +
                "'FeedBagProjectSpecific': false," +
                "'FeedBagSourceCode': false," +
                "'ResearchGenericInteraction': false," +
                "'ResearchProjectSpecific': false," +
                "'ResearchSourceCode': false," +
                "'OpenDataGenericInteraction': false," +
                "'OpenDataProjectSpecific': false," +
                "'OpenDataSourceCode': false" +
                "}}");

            _dashboardSettingsForm.SetPrivacySettingsJOject(currentSelection);

            _dashboardSettingsForm.GetCheckBoxResearchSourceCode().Checked = true;
            _dashboardSettingsForm.GetCheckBoxOpenDataSourceCode().Checked = true;

            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchSourceCode().Checked, true);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataSourceCode().Checked, true);

            _dashboardSettingsForm.GetCheckBoxResearchSourceCode().Checked = false;

            // check if research is unchecked that open data is unchecked as well
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchSourceCode().Checked, false);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataSourceCode().Checked, false);

        }

        [TestMethod]
        public void TestCheckBoxOpenDataGenericInteraction_CheckedChanged()
        {
            _dashboardSettingsForm.SetCurrentPrivacySettingsJObjectId("0");

            JObject currentSelection = JObject.Parse(
                @"{'0':{'Solution': 'Solution 1', " +
                "'Enabled': false," +
                "'FeedBagGenericInteraction': false," +
                "'FeedBagProjectSpecific': false," +
                "'FeedBagSourceCode': false," +
                "'ResearchGenericInteraction': true," +
                "'ResearchProjectSpecific': false," +
                "'ResearchSourceCode': false," +
                "'OpenDataGenericInteraction': true," +
                "'OpenDataProjectSpecific': false," +
                "'OpenDataSourceCode': false" +
                "}}");

            _dashboardSettingsForm.SetPrivacySettingsJOject(currentSelection);

            _dashboardSettingsForm.GetCheckBoxOpenDataGenericInteraction().Checked = false;
            _dashboardSettingsForm.GetCheckBoxResearchGenericInteraction().Checked = false;

            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchGenericInteraction().Checked, false);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataGenericInteraction().Checked, false);

            _dashboardSettingsForm.GetCheckBoxOpenDataGenericInteraction().Checked = true;

            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchGenericInteraction().Checked, true);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataGenericInteraction().Checked, true);

        }

        [TestMethod]
        public void TestCheckBoxOpenDataProjectSpecific_CheckedChanged()
        {
            _dashboardSettingsForm.SetCurrentPrivacySettingsJObjectId("0");

            JObject currentSelection = JObject.Parse(
                @"{'0':{'Solution': 'Solution 1', " +
                "'Enabled': false," +
                "'FeedBagGenericInteraction': false," +
                "'FeedBagProjectSpecific': false," +
                "'FeedBagSourceCode': false," +
                "'ResearchGenericInteraction': false," +
                "'ResearchProjectSpecific': true," +
                "'ResearchSourceCode': false," +
                "'OpenDataGenericInteraction': false," +
                "'OpenDataProjectSpecific': true," +
                "'OpenDataSourceCode': false" +
                "}}");

            _dashboardSettingsForm.SetPrivacySettingsJOject(currentSelection);

            _dashboardSettingsForm.GetCheckBoxResearchProjectSpecific().Checked = false;
            _dashboardSettingsForm.GetCheckBoxOpenDataProjectSpecific().Checked = false;

            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchProjectSpecific().Checked, false);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataProjectSpecific().Checked, false);

            _dashboardSettingsForm.GetCheckBoxOpenDataProjectSpecific().Checked = true;

            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchProjectSpecific().Checked, true);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataProjectSpecific().Checked, true);

        }

        [TestMethod]
        public void TestCheckBoxOpenDataOpenData_CheckedChanged()
        {
            _dashboardSettingsForm.SetCurrentPrivacySettingsJObjectId("0");

            JObject currentSelection = JObject.Parse(
                @"{'0':{'Solution': 'Solution 1', " +
                "'Enabled': false," +
                "'FeedBagGenericInteraction': false," +
                "'FeedBagProjectSpecific': false," +
                "'FeedBagSourceCode': false," +
                "'ResearchGenericInteraction': false," +
                "'ResearchProjectSpecific': true," +
                "'ResearchSourceCode': true," +
                "'OpenDataGenericInteraction': false," +
                "'OpenDataProjectSpecific': true," +
                "'OpenDataSourceCode': true" +
                "}}");

            _dashboardSettingsForm.SetPrivacySettingsJOject(currentSelection);

            _dashboardSettingsForm.GetCheckBoxOpenDataSourceCode().Checked = false;
            _dashboardSettingsForm.GetCheckBoxResearchSourceCode().Checked = false;

            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchSourceCode().Checked, false);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataSourceCode().Checked, false);

            _dashboardSettingsForm.GetCheckBoxOpenDataSourceCode().Checked = true;

            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchSourceCode().Checked, true);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataSourceCode().Checked, true);

        }

        [TestMethod]
        public void TestCheckBoxEnableDataCollection_CheckedChanged()
        {
            JObject currentSelection = JObject.Parse(
                @"{'0':{'Solution': 'Solution 1', " +
                "'Enabled': false," +
                "'FeedBagGenericInteraction': false," +
                "'FeedBagProjectSpecific': false," +
                "'FeedBagSourceCode': false," +
                "'ResearchGenericInteraction': false," +
                "'ResearchProjectSpecific': true," +
                "'ResearchSourceCode': true," +
                "'OpenDataGenericInteraction': false," +
                "'OpenDataProjectSpecific': true," +
                "'OpenDataSourceCode': true" +
                "}}");

            DashboardSettingsForm dashboardSettingsForm = new DashboardSettingsForm();

            dashboardSettingsForm.SetPrivacySettingsJOject(currentSelection);
       
            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxResearchGenericInteraction().Enabled, false);
            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxResearchProjectSpecific().Enabled, false);
            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxResearchSourceCode().Enabled, false);
            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxOpenDataGenericInteraction().Enabled, false);
            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxOpenDataProjectSpecific().Enabled, false);
            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxOpenDataSourceCode().Enabled, false);

            dashboardSettingsForm.GetCheckBoxEnableDataCollection().Checked = true;

            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxResearchGenericInteraction().Enabled, true);
            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxResearchProjectSpecific().Enabled, true);
            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxResearchSourceCode().Enabled, true);
            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxOpenDataGenericInteraction().Enabled, true);
            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxOpenDataProjectSpecific().Enabled, true);
            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxOpenDataSourceCode().Enabled, true);

        }

        [TestMethod]
        public void TestLoadSettingsForSolution()
        {
            string solutionId = "0";

            JObject currentSelection = JObject.Parse(
                @"{'0':{'Solution': 'Solution 1', " +
                "'Enabled': false," +
                "'FeedBagGenericInteraction': false," +
                "'FeedBagProjectSpecific': false," +
                "'FeedBagSourceCode': false," +
                "'ResearchGenericInteraction': false," +
                "'ResearchProjectSpecific': true," +
                "'ResearchSourceCode': true," +
                "'OpenDataGenericInteraction': false," +
                "'OpenDataProjectSpecific': true," +
                "'OpenDataSourceCode': true" +
                "}}");

            DashboardSettingsForm dashboardSettingsForm = new DashboardSettingsForm();
            dashboardSettingsForm.SetCurrentPrivacySettingsJObjectId(solutionId);
            dashboardSettingsForm.SetPrivacySettingsJOject(currentSelection);

            dashboardSettingsForm.LoadSettingsForSolution(solutionId);

            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxFeedBagOnlyGenericInteraction().Checked, true);
            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxFeedBagOnlyProjectSpecific().Checked, true);
            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxFeedBagOnlySourceCode().Checked, true);

            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxResearchGenericInteraction().Checked, false);
            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxResearchProjectSpecific().Checked, true);
            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxResearchSourceCode().Checked, true);

            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxOpenDataGenericInteraction().Checked, false);
            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxOpenDataProjectSpecific().Checked, true);
            Assert.AreEqual(dashboardSettingsForm.GetCheckBoxOpenDataSourceCode().Checked, true);

        }

        [TestMethod]
        public void TestPublishUserProfileEventWithPrivacySettings()
        {
            /*_settingsDashboard.SharingDataEnabled = true;
            _settingsDashboard.SharingGenericInteractionDataForFeedBagOnlyEnabled = true;
            _settingsDashboard.SharingGenericInteractionDataForResearchEnabled = true;
            _settingsDashboard.SharingGenericInteractionDataForOpenDataSetEnabled = false;
            _settingsDashboard.SharingProjectSpecificDataForFeedBagOnlyEnabled = true;
            _settingsDashboard.SharingProjectSpecificDataForResearchEnabled = false;
            _settingsDashboard.SharingProjectSpecificDataForOpenDataSetEnabled = false;
            _settingsDashboard.SharingSourceCodeForFeedBagOnlyEnabled = true;
            _settingsDashboard.SharingSourceCodeForResearchEnabled = true;
            _settingsDashboard.SharingSourceCodeForOpenDataSetEnabled = true;


            _settingsStore.SetSettings<DashboardPrivacySettings>(_settingsDashboard);
            _settingsStore.SetSettings<UserProfileSettings>(new UserProfileSettings());*/

            UserProfileEventGenerator sut = new UserProfileEventGenerator(null, null, null, _settingsStore, null);
            UserProfileEvent userProfileEvent = new UserProfileEvent {
                ProfileId = "p",
                SharingDataEnabled = true,
                SharingGenericInteractionDataForFeedBagOnlyEnabled = true,
                SharingGenericInteractionDataForResearchEnabled = true,
                SharingGenericInteractionDataForOpenDataSetEnabled = false,
                SharingProjectSpecificDataForFeedBagOnlyEnabled = true,
                SharingProjectSpecificDataForResearchEnabled = false,
                SharingProjectSpecificDataForOpenDataSetEnabled = false,
                SharingSourceCodeForFeedBagOnlyEnabled = true,
                SharingSourceCodeForResearchEnabled = true,
                SharingSourceCodeForOpenDataSetEnabled = true
            };

            /*Assert.Equals(userProfileEvent.SharingDataEnabled, true);

            Assert.Equals(userProfileEvent.SharingGenericInteractionDataForFeedBagOnlyEnabled, true);
            Assert.Equals(userProfileEvent.SharingGenericInteractionDataForResearchEnabled, true);
            Assert.Equals(userProfileEvent.SharingGenericInteractionDataForOpenDataSetEnabled, false);

            Assert.Equals(userProfileEvent.SharingProjectSpecificDataForFeedBagOnlyEnabled, true);
            Assert.Equals(userProfileEvent.SharingProjectSpecificDataForResearchEnabled, false);
            Assert.Equals(userProfileEvent.SharingProjectSpecificDataForOpenDataSetEnabled, false);

            Assert.Equals(userProfileEvent.SharingSourceCodeForFeedBagOnlyEnabled, true);
            Assert.Equals(userProfileEvent.SharingSourceCodeForResearchEnabled, true);
            Assert.Equals(userProfileEvent.SharingSourceCodeForOpenDataSetEnabled, true);*/

        }

        private const string SomeTargetLocation = "existing target file";

        protected static IEnumerable<IDEEvent> TestEventSource(int count)
        {
            var baseDate = new DateTime(2014, 1, 1);
            for (var i = 0; i < count; i++)
            {
                yield return new WindowEvent { TriggeredAt = baseDate.AddDays(i) };
            }
        }
    }
}
