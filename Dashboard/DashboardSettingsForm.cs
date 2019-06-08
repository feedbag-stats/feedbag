using System;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace Dashboard
{
    public partial class DashboardSettingsForm : Form
    { 
        private readonly dynamic _myPrivacySettingJson = new JObject();
        private string _myCurrentSolutionId = "0";
        private dynamic _myDefaultPrivacySettingsJson = new JObject();

        public DashboardSettingsForm()
        {
            InitializeComponent();
            InitializePrivacySettingsJObject();
            //_myDefaultPrivacySettingsJson = JObject.Parse(_myPrivacySettingJson.ToString());
            _myDefaultPrivacySettingsJson = new JObject();

            // set default state of default setting
            dynamic privacySettings = new JObject();
            privacySettings.Solution = _myPrivacySettingJson[_myCurrentSolutionId]["Solution"];
            privacySettings.Enabled = false;

            privacySettings.FeedBagGenericInteraction = true;
            privacySettings.FeedBagProjectSpecific = true;
            privacySettings.FeedBagSourceCode = true;

            privacySettings.ResearchGenericInteraction = false;
            privacySettings.ResearchProjectSpecific = false;
            privacySettings.ResearchSourceCode = false;

            privacySettings.OpenDataGenericInteraction = false;
            privacySettings.OpenDataProjectSpecific = false;
            privacySettings.OpenDataSourceCode = false;
        }

        //only used for testing
        public DashboardSettingsForm(bool testing)
        {
            InitializeComponent();
        }

        public void SaveDefaultSettings()
        {
            _myDefaultPrivacySettingsJson = JObject.Parse(_myPrivacySettingJson[_myCurrentSolutionId].ToString());
            
            //TODO: Send Default settings to server
        }

        public void EnableCheckBoxDependingOnSelection(JObject selectedSettings)
        {
            this.checkBox10.Checked = (bool)selectedSettings["Enabled"];
            if (this.checkBox10.Checked)
            {
                this.checkBoxResearchGenericInteraction.Enabled = true;
                this.checkBox2ResearchProjectSpecific.Enabled = true;
                this.checkBoxResearchSourceCode.Enabled = true;
                this.checkBoxOpenDataSourceCode.Enabled = true;
                this.checkBoxOpenDataProjectSpecific.Enabled = true;
                this.checkBoxOpenDataGenericInteraction.Enabled = true;
            }
            else
            {
                this.checkBoxResearchGenericInteraction.Enabled = false;
                this.checkBox2ResearchProjectSpecific.Enabled = false;
                this.checkBoxResearchSourceCode.Enabled = false;
                this.checkBoxOpenDataSourceCode.Enabled = false;
                this.checkBoxOpenDataProjectSpecific.Enabled = false;
                this.checkBoxOpenDataGenericInteraction.Enabled = false;
            }
        }

        public void CheckCheckBoxesAccodingToSelection(JObject selectedSettings)
        {
            this.checkBoxResearchGenericInteraction.Checked = (bool)selectedSettings["ResearchGenericInteraction"];
            this.checkBox2ResearchProjectSpecific.Checked = (bool)selectedSettings["ResearchProjectSpecific"];
            this.checkBoxResearchSourceCode.Checked = (bool)selectedSettings["ResearchSourceCode"];
            this.checkBoxOpenDataSourceCode.Checked = (bool)selectedSettings["OpenDataSourceCode"];
            this.checkBoxOpenDataProjectSpecific.Checked = (bool)selectedSettings["OpenDataProjectSpecific"];
            this.checkBoxOpenDataGenericInteraction.Checked = (bool)selectedSettings["OpenDataGenericInteraction"];
        }

        public void LoadDefaultSettingsForCurrentSolution()
        {
            
            //JObject selectedDefaultSettings = JObject.Parse(_myDefaultPrivacySettingsJson[_myCurrentSolutionId].ToString());
            try
            {
                EnableCheckBoxDependingOnSelection(_myDefaultPrivacySettingsJson);
                CheckCheckBoxesAccodingToSelection(_myDefaultPrivacySettingsJson);
            }
            catch (Exception)
            { }
        }

        public void SetCombobox(ComboBox newComboBox)
        {
            this.comboBox1 = newComboBox;
        }

        public JObject GetPrivacySettingJson()
        {
            return _myPrivacySettingJson;
        }

        public void InitializePrivacySettingsJObject()
        {
            var mySolutions = this.comboBox1.Items;

            // change combobox text to name of first solution
            this.comboBox1.Text = mySolutions[0].ToString();
            
            int id = 0;
            //initialize privacy settings JObject
            foreach (object solution in mySolutions)
            {
                dynamic privacySettings = new JObject();
                privacySettings.Solution = solution.ToString();
                privacySettings.Enabled = false;

                privacySettings.FeedBagGenericInteraction = false;
                privacySettings.FeedBagProjectSpecific = false;
                privacySettings.FeedBagSourceCode = false;

                privacySettings.ResearchGenericInteraction = false;
                privacySettings.ResearchProjectSpecific = false;
                privacySettings.ResearchSourceCode = false;

                privacySettings.OpenDataGenericInteraction = false;
                privacySettings.OpenDataProjectSpecific = false;
                privacySettings.OpenDataSourceCode = false;

                _myPrivacySettingJson.Add(id.ToString(), privacySettings);
                id++;

            }   
        }


        // create and return a Json Object with the given content
        public void AddPrivacyJson(string solutionSelectionId, string solutionName, bool enabledSettingsForSolution, 
            bool researchGenericInteraction, bool researchProjectSpecific, bool researchSourceCode,
            bool openDataGenericInteraction, bool openDataProjectSpecific, bool openDataSourceCode)
        {
            dynamic privacySettings = new JObject();
            privacySettings.Solution = solutionName;
            privacySettings.Enabled = enabledSettingsForSolution;

            if (enabledSettingsForSolution)
            {
                privacySettings.FeedBagGenericInteraction = true;
                privacySettings.FeedBagProjectSpecific = true;
                privacySettings.FeedBagSourceCode = true;

                privacySettings.ResearchGenericInteraction = researchGenericInteraction;
                privacySettings.ResearchProjectSpecific = researchProjectSpecific;
                privacySettings.ResearchSourceCode = researchSourceCode;

                privacySettings.OpenDataGenericInteraction = openDataGenericInteraction;
                privacySettings.OpenDataProjectSpecific = openDataProjectSpecific;
                privacySettings.OpenDataSourceCode = openDataSourceCode;
            }
            else
            {
                privacySettings.FeedBagGenericInteraction = false;
                privacySettings.FeedBagProjectSpecific = false;
                privacySettings.FeedBagSourceCode = false;

                privacySettings.ResearchGenericInteraction = false;
                privacySettings.ResearchProjectSpecific = false;
                privacySettings.ResearchSourceCode = false;

                privacySettings.OpenDataGenericInteraction = false;
                privacySettings.OpenDataProjectSpecific = false;
                privacySettings.OpenDataSourceCode = false;
            }

            _myPrivacySettingJson.SolutionSelectionId = privacySettings;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
           _myPrivacySettingJson[_myCurrentSolutionId]["ResearchGenericInteraction"] = this.checkBoxResearchGenericInteraction.Checked;
            // if you uncheck open data, you automatically uncheck research 
            if (!this.checkBoxResearchGenericInteraction.Checked)
            {
                _myPrivacySettingJson[_myCurrentSolutionId]["OpenDatahGenericInteraction"] = this.checkBoxResearchGenericInteraction.Checked;
                this.checkBoxOpenDataGenericInteraction.Checked = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            _myPrivacySettingJson[_myCurrentSolutionId]["ResearchProjectSpecific"] = this.checkBox2ResearchProjectSpecific.Checked;
            // if you uncheck open data, you automatically uncheck research 
            if (!this.checkBox2ResearchProjectSpecific.Checked)
            {
                _myPrivacySettingJson[_myCurrentSolutionId]["OpenDataProjectSpecific"] = this.checkBox2ResearchProjectSpecific.Checked;
                this.checkBoxOpenDataProjectSpecific.Checked = false;
            }
        }

        // cancle button
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // save button
        private void button1_Click(object sender, EventArgs e)
        {
            //TODO: send Jarray to server

            this.Close();

        }

        // feedback only, generic interaction data
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

        }

        // feedback only
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private bool myCheckBox10Checked = false;
        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {

            if (!myCheckBox10Checked)
            {
                this.checkBoxResearchGenericInteraction.Enabled = true;
                this.checkBox2ResearchProjectSpecific.Enabled = true;
                this.checkBoxResearchSourceCode.Enabled = true;
                this.checkBoxOpenDataSourceCode.Enabled = true;
                this.checkBoxOpenDataProjectSpecific.Enabled = true;
                this.checkBoxOpenDataGenericInteraction.Enabled = true;
                myCheckBox10Checked = true;

            }
            else
            {
                this.checkBoxResearchGenericInteraction.Enabled = false;
                this.checkBox2ResearchProjectSpecific.Enabled = false;
                this.checkBoxResearchSourceCode.Enabled = false;
                this.checkBoxOpenDataSourceCode.Enabled = false;
                this.checkBoxOpenDataProjectSpecific.Enabled = false;
                this.checkBoxOpenDataGenericInteraction.Enabled = false;
                myCheckBox10Checked = false;

            }
            _myPrivacySettingJson[_myCurrentSolutionId]["Enabled"] = myCheckBox10Checked;

        }

        // feedback only
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            // get selected item of combobox
            int selectedIndex = comboBox1.SelectedIndex;
            this._myCurrentSolutionId = selectedIndex.ToString();

            LoadSettingsForSolution(selectedIndex.ToString());

         
        }

        private void LoadSettingsForSolution(string solutionSettingsId)
        {
            JObject selectedSettings = _myPrivacySettingJson[solutionSettingsId];
            try
            {
                EnableCheckBoxDependingOnSelection(selectedSettings);
                CheckCheckBoxesAccodingToSelection(selectedSettings);
            }
            catch (Exception)
            {}
           
        }

        private void checkBoxResearchSourceCode_CheckedChanged(object sender, EventArgs e)
        {
            _myPrivacySettingJson[_myCurrentSolutionId]["ResearchSourceCode"] = this.checkBoxResearchSourceCode.Checked;
            // if you uncheck open data, you automatically uncheck research 
            if (!this.checkBoxResearchSourceCode.Checked)
            {
                _myPrivacySettingJson[_myCurrentSolutionId]["OpenDataSourceCode"] = this.checkBoxResearchSourceCode.Checked;
                this.checkBoxOpenDataSourceCode.Checked = false;
            }
        }

        private void checkBoxOpenDataGenericInteraction_CheckedChanged(object sender, EventArgs e)
        {
            _myPrivacySettingJson[_myCurrentSolutionId]["OpenDataGenericInteraction"] = this.checkBoxOpenDataGenericInteraction.Checked;
            // if you check open data, you automatically check research    
            if (this.checkBoxOpenDataGenericInteraction.Checked)
            {
                _myPrivacySettingJson[_myCurrentSolutionId]["ResearchGenericInteraction"] = this.checkBoxOpenDataGenericInteraction.Checked;
                this.checkBoxResearchGenericInteraction.Checked = true;
            }

        }

        private void checkBoxOpenDataProjectSpecific_CheckedChanged(object sender, EventArgs e)
        {
            _myPrivacySettingJson[_myCurrentSolutionId]["OpenDataProjectSpecific"] = this.checkBoxOpenDataProjectSpecific.Checked;
            // if you check open data, you automatically check research    
            if (this.checkBoxOpenDataProjectSpecific.Checked)
            {
                _myPrivacySettingJson[_myCurrentSolutionId]["ResearchProjectSpecific"] = this.checkBoxOpenDataProjectSpecific.Checked;
                this.checkBox2ResearchProjectSpecific.Checked = true;
            }
            
        }

        private void checkBoxOpenDataSourceCode_CheckedChanged(object sender, EventArgs e)
        {
            _myPrivacySettingJson[_myCurrentSolutionId]["OpenDataSourceCode"] = this.checkBoxOpenDataSourceCode.Checked;
            // if you check open data, you automatically check research    
            if (this.checkBoxOpenDataSourceCode.Checked)
            {
                _myPrivacySettingJson[_myCurrentSolutionId]["ResearchSourceCode"] = this.checkBoxOpenDataSourceCode.Checked;
                this.checkBoxResearchSourceCode.Checked = true;

            }

        }

       

        private void SaveDefaultSettingsButton_Click(object sender, EventArgs e)
        {
            SaveDefaultSettings();
        }

        private void LoadDefaultSettingsButton_Click(object sender, EventArgs e)
        {
            LoadDefaultSettingsForCurrentSolution();
        }

        private void DashboardSettingsForm_Load(object sender, EventArgs e)
        {

        }
    }

    //test comment
}
