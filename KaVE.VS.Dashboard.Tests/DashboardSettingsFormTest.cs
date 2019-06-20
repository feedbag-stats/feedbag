using System;
using System.Windows.Forms;
using Dashboard;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using KaVE.RS.Commons;

namespace KaVE.VS.Dashboard.Tests
{
    [TestClass]
    public class DashboardSettingsFormTest
    {
        private readonly DashboardSettingsForm _dashboardSettingsForm = new DashboardSettingsForm(true);
        private readonly ComboBox _comboBox1 = new ComboBox();
        private readonly string _myCurrentSolutionId = "1";


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
        }

        public void TestSaveDefaultSettings()
        {
            _dashboardSettingsForm.SaveDefaultSettings();

            JObject expectedDefaultSettingsJObect = JObject.Parse(
                @"{'Solution': 'Solution 2', " +
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
                "}");

            JObject resultDefaultPrivacySettingsJson = _dashboardSettingsForm.GetDefaultPrivacySettingsJobject();
            Assert.AreEqual(resultDefaultPrivacySettingsJson.ToString(), expectedDefaultSettingsJObect.ToString());
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

            _dashboardSettingsForm.LoadDefaultSettingsForCurrentSolution();

            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxFeedBagOnlyGenericInteraction().Checked, true);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxFeedBagOnlyProjectSpecific().Checked, true);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxFeedBagOnlySourceCode().Checked, true);

            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchGenericInteraction().Checked, true);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchProjectSpecific().Checked, true);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxResearchSourceCode().Checked, true);

            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataGenericInteraction().Checked, true);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataProjectSpecific().Checked, false);
            Assert.AreEqual(_dashboardSettingsForm.GetCheckBoxOpenDataSourceCode().Checked, false);

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

    }
}
