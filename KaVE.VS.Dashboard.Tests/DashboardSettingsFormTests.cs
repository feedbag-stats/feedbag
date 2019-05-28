using System.Windows.Forms;
using Dashboard;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace KaVE.VS.Dashboard.Tests
{
    [TestClass]
    public class DashboardSettingsFormTests
    {
        DashboardSettingsForm dashboardSettingsForm = new DashboardSettingsForm(true);

        [TestMethod]
        public void TestInitializePrivacySettings()
        {
            ComboBox comboBox1 = new ComboBox();
            comboBox1.AllowDrop = true;
            comboBox1.Font = new System.Drawing.Font(
                "Microsoft Sans Serif",
                16F,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                ((byte) (0)));
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(
                new object[]
                {
                    "Solution 1",
                    "Solution 2"
                });
            comboBox1.Location = new System.Drawing.Point(432, 41);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(345, 45);
            comboBox1.TabIndex = 28;
            comboBox1.Text = "Solution";

            dashboardSettingsForm.SetCombobox(comboBox1);
            dashboardSettingsForm.InitializePrivacySettingsJObject();

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

            JObject resultPrivacySettingsJson = dashboardSettingsForm.GetPrivacySettingJson();
            Assert.AreEqual(resultPrivacySettingsJson.ToString(), expectedJObect.ToString());
        }

        [TestMethod]
        public void TestLoad() { }
    }
}
