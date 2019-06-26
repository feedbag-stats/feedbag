using System;
using System.Windows.Forms;
using KaVE.RS.Commons.Settings;
using KaVE.VS.Commons;
using KaVE.VS.FeedbackGenerator.Generators;
using Newtonsoft.Json.Linq;

namespace Dashboard
{
    public partial class DashboardSettingsForm : Form
    { 
        private dynamic _myPrivacySettingJson = new JObject();
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

            var privacyDefaultSettings = new DashboardDefaultPrivacySettings
            {
                SharingDataEnabled = _myDefaultPrivacySettingsJson["Enabled"],

                SharingGenericInteractionDataForFeedBagOnlyEnabled = _myDefaultPrivacySettingsJson["FeedBagGenericInteraction"],
                SharingGenericInteractionDataForResearchEnabled = _myDefaultPrivacySettingsJson["ResearchGenericInteraction"],
                SharingGenericInteractionDataForOpenDataSetEnabled = _myDefaultPrivacySettingsJson["OpenDataGenericInteraction"],

                SharingProjectSpecificDataForFeedBagOnlyEnabled = _myDefaultPrivacySettingsJson["FeedBagProjectSpecific"],
                SharingProjectSpecificDataForResearchEnabled = _myDefaultPrivacySettingsJson["ResearchProjectSpecific"],
                SharingProjectSpecificDataForOpenDataSetEnabled = _myDefaultPrivacySettingsJson["OpenDataProjectSpecific"],

                SharingSourceCodeForFeedBagOnlyEnabled = _myDefaultPrivacySettingsJson["FeedBagSourceCode"],
                SharingSourceCodeForResearchEnabled = _myDefaultPrivacySettingsJson["ResearchSourceCode"],
                SharingSourceCodeForOpenDataSetEnabled = _myDefaultPrivacySettingsJson["OpenDataSourceCode"]

            };

            // add the privacy stettings to the settings store
            var settingsStore = Registry.GetComponent<ISettingsStore>();
            settingsStore.SetSettings<DashboardDefaultPrivacySettings>(privacyDefaultSettings);

            //FIXME: replace the nulls with values
            var sut = new UserProfileEventGenerator(null, null, null, settingsStore, null);

            //sut.CreateEvent();
        }

        public void EnableCheckBoxDependingOnSelection(JObject selectedSettings)
        {
            this.checkBoxEnableDataCollection.Checked = (bool)selectedSettings["Enabled"];
            if (this.checkBoxEnableDataCollection.Checked)
            {
                this.checkBoxResearchGenericInteraction.Enabled = true;
                this.checkBoxResearchProjectSpecific.Enabled = true;
                this.checkBoxResearchSourceCode.Enabled = true;
                this.checkBoxOpenDataSourceCode.Enabled = true;
                this.checkBoxOpenDataProjectSpecific.Enabled = true;
                this.checkBoxOpenDataGenericInteraction.Enabled = true;
            }
            else
            {
                this.checkBoxResearchGenericInteraction.Enabled = false;
                this.checkBoxResearchProjectSpecific.Enabled = false;
                this.checkBoxResearchSourceCode.Enabled = false;
                this.checkBoxOpenDataSourceCode.Enabled = false;
                this.checkBoxOpenDataProjectSpecific.Enabled = false;
                this.checkBoxOpenDataGenericInteraction.Enabled = false;
            }
        }

        public void CheckCheckBoxesAccodingToSelection(JObject selectedSettings)
        {
            this.checkBoxResearchGenericInteraction.Checked = (bool)selectedSettings["ResearchGenericInteraction"];
            this.checkBoxResearchProjectSpecific.Checked = (bool)selectedSettings["ResearchProjectSpecific"];
            this.checkBoxResearchSourceCode.Checked = (bool)selectedSettings["ResearchSourceCode"];
            this.checkBoxOpenDataSourceCode.Checked = (bool)selectedSettings["OpenDataSourceCode"];
            this.checkBoxOpenDataProjectSpecific.Checked = (bool)selectedSettings["OpenDataProjectSpecific"];
            this.checkBoxOpenDataGenericInteraction.Checked = (bool)selectedSettings["OpenDataGenericInteraction"];
        }

        public void LoadDefaultSettingsForCurrentSolution()
        {
            try
            {
                var settingsStore = Registry.GetComponent<SettingsStore>();
                DashboardDefaultPrivacySettings privacyDefaultSettings = settingsStore.GetSettings<DashboardDefaultPrivacySettings>();

                if (privacyDefaultSettings.Equals(null))
                {
                    // take the locally saved default privacy setting
                    try
                    {
                        EnableCheckBoxDependingOnSelection(_myDefaultPrivacySettingsJson);
                        CheckCheckBoxesAccodingToSelection(_myDefaultPrivacySettingsJson);
                    }
                    catch (Exception) { }
                }
                else
                {
                    try
                    {
                        // otherwise load the default settings stored in the settings store
                        dynamic privacyDefaultJObject = new JObject();
                        privacyDefaultJObject.Solution = _myPrivacySettingJson[_myCurrentSolutionId]["Solution"];
                        privacyDefaultJObject.Enabled = privacyDefaultSettings.SharingDataEnabled;

                        privacyDefaultJObject.FeedBagGenericInteraction = privacyDefaultSettings.SharingGenericInteractionDataForFeedBagOnlyEnabled;
                        privacyDefaultJObject.FeedBagProjectSpecific = privacyDefaultSettings.SharingProjectSpecificDataForFeedBagOnlyEnabled;
                        privacyDefaultJObject.FeedBagSourceCode = privacyDefaultSettings.SharingSourceCodeForFeedBagOnlyEnabled;

                        privacyDefaultJObject.ResearchGenericInteraction = privacyDefaultSettings.SharingGenericInteractionDataForResearchEnabled;
                        privacyDefaultJObject.ResearchProjectSpecific = privacyDefaultSettings.SharingProjectSpecificDataForResearchEnabled;
                        privacyDefaultJObject.ResearchSourceCode = privacyDefaultSettings.SharingSourceCodeForResearchEnabled;

                        privacyDefaultJObject.OpenDataGenericInteraction = privacyDefaultSettings.SharingGenericInteractionDataForOpenDataSetEnabled;
                        privacyDefaultJObject.OpenDataProjectSpecific = privacyDefaultSettings.SharingProjectSpecificDataForOpenDataSetEnabled;
                        privacyDefaultJObject.OpenDataSourceCode = privacyDefaultSettings.SharingSourceCodeForOpenDataSetEnabled;

                        EnableCheckBoxDependingOnSelection(privacyDefaultJObject);
                        CheckCheckBoxesAccodingToSelection(privacyDefaultJObject);
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception )
            {
                // take the locally saved default privacy setting
                try
                {
                    EnableCheckBoxDependingOnSelection(_myDefaultPrivacySettingsJson);
                    CheckCheckBoxesAccodingToSelection(_myDefaultPrivacySettingsJson);
                }
                catch (Exception) { }
            }
            
        }    

        public void InitializePrivacySettingsJObject()
        {
            var mySolutions = this.comboBoxSolutions.Items;

            // change combobox text to name of first solution
            this.comboBoxSolutions.Text = mySolutions[0].ToString();
            
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
        /*public void AddPrivacyJson(string solutionSelectionId, string solutionName, bool enabledSettingsForSolution, 
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
        }*/

        private void checkBoxResearchGenericInteraction_CheckedChanged(object sender, EventArgs e)
        {
            _myPrivacySettingJson[_myCurrentSolutionId]["ResearchGenericInteraction"] = this.checkBoxResearchGenericInteraction.Checked;
            // if you uncheck research, you automatically uncheck open data 
            if (!this.checkBoxResearchGenericInteraction.Checked)
            {
                _myPrivacySettingJson[_myCurrentSolutionId]["OpenDataGenericInteraction"] = this.checkBoxResearchGenericInteraction.Checked;
                this.checkBoxOpenDataGenericInteraction.Checked = false;
            }
        }

        private void checkBoxResearchProjectSpecific_CheckedChanged(object sender, EventArgs e)
        {
            _myPrivacySettingJson[_myCurrentSolutionId]["ResearchProjectSpecific"] = this.checkBoxResearchProjectSpecific.Checked;
            // if you uncheck research, you automatically uncheck open data 
            if (!this.checkBoxResearchProjectSpecific.Checked)
            {
                _myPrivacySettingJson[_myCurrentSolutionId]["OpenDataProjectSpecific"] = this.checkBoxResearchProjectSpecific.Checked;
                this.checkBoxOpenDataProjectSpecific.Checked = false;
            }

        }
        
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            var privacySettings = new DashboardPrivacySettings
            {
                SharingDataEnabled = _myPrivacySettingJson[_myCurrentSolutionId]["Enabled"],

                SharingGenericInteractionDataForFeedBagOnlyEnabled = _myPrivacySettingJson[_myCurrentSolutionId]["FeedBagGenericInteraction"],
                SharingGenericInteractionDataForResearchEnabled = _myPrivacySettingJson[_myCurrentSolutionId]["ResearchGenericInteraction"],
                SharingGenericInteractionDataForOpenDataSetEnabled = _myPrivacySettingJson[_myCurrentSolutionId]["OpenDataGenericInteraction"],

                SharingProjectSpecificDataForFeedBagOnlyEnabled = _myPrivacySettingJson[_myCurrentSolutionId]["FeedBagProjectSpecific"],
                SharingProjectSpecificDataForResearchEnabled = _myPrivacySettingJson[_myCurrentSolutionId]["ResearchProjectSpecific"],
                SharingProjectSpecificDataForOpenDataSetEnabled = _myPrivacySettingJson[_myCurrentSolutionId]["OpenDataProjectSpecific"],

                SharingSourceCodeForFeedBagOnlyEnabled = _myPrivacySettingJson[_myCurrentSolutionId]["FeedBagSourceCode"],
                SharingSourceCodeForResearchEnabled = _myPrivacySettingJson[_myCurrentSolutionId]["ResearchSourceCode"],
                SharingSourceCodeForOpenDataSetEnabled = _myPrivacySettingJson[_myCurrentSolutionId]["OpenDataSourceCode"]
                
            };

            // add the privacy stettings to the settings store
            var settingsStore = Registry.GetComponent<SettingsStore>();
            settingsStore.SetSettings<DashboardPrivacySettings>(privacySettings);

            //FIXME: replace the nulls with values
            var sut = new UserProfileEventGenerator(null, null, null, settingsStore, null);

            sut.CreateEvent();

            // close the privacy settings windows
            this.Close();
        }

        private void checkBoxFeedBagOnlyGenericInteraction_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxFeedBagOnlyProjectSpecific_CheckedChanged(object sender, EventArgs e)
        {

        }

        private bool _myCheckBoxEnableDataCollectionChecked = false;
        private void checkBoxEnableDataCollection_CheckedChanged(object sender, EventArgs e)
        {
            if (!_myCheckBoxEnableDataCollectionChecked)
            {
                this.checkBoxResearchGenericInteraction.Enabled = true;
                this.checkBoxResearchProjectSpecific.Enabled = true;
                this.checkBoxResearchSourceCode.Enabled = true;
                this.checkBoxOpenDataSourceCode.Enabled = true;
                this.checkBoxOpenDataProjectSpecific.Enabled = true;
                this.checkBoxOpenDataGenericInteraction.Enabled = true;
                _myCheckBoxEnableDataCollectionChecked = true;

            }
            else
            {
                this.checkBoxResearchGenericInteraction.Enabled = false;
                this.checkBoxResearchProjectSpecific.Enabled = false;
                this.checkBoxResearchSourceCode.Enabled = false;
                this.checkBoxOpenDataSourceCode.Enabled = false;
                this.checkBoxOpenDataProjectSpecific.Enabled = false;
                this.checkBoxOpenDataGenericInteraction.Enabled = false;
                _myCheckBoxEnableDataCollectionChecked = false;

            }
            _myPrivacySettingJson[_myCurrentSolutionId]["Enabled"] = _myCheckBoxEnableDataCollectionChecked;
        }

        private void checkBoxFeedBagOnlySourceCode_CheckedChanged(object sender, EventArgs e)
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


        private void comboBoxSolutions_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get selected item of combobox
            int selectedIndex = comboBoxSolutions.SelectedIndex;
            this._myCurrentSolutionId = selectedIndex.ToString();

            LoadSettingsForSolution(selectedIndex.ToString());
        }

        public void LoadSettingsForSolution(string solutionSettingsId)
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
                this.checkBoxResearchProjectSpecific.Checked = true;
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

        //Getter and Setter// 

        public void SetPrivacySettingsJOject(JObject newPrivacySettingsJObject)
        {
            this._myPrivacySettingJson = newPrivacySettingsJObject;
        }

        public JObject GetDefaultPrivacySettingsJobject()
        {
            return _myDefaultPrivacySettingsJson;
        }

        public JObject GetPrivacySettingJson()
        {
            return _myPrivacySettingJson;
        }

        public CheckBox GetCheckBoxFeedBagOnlyGenericInteraction()
        {
            return this.checkBoxFeedBagOnlyGenericInteraction;
        }

        public CheckBox GetCheckBoxFeedBagOnlyProjectSpecific()
        {
            return this.checkBoxFeedBagOnlyProjectSpecific;
        }

        public CheckBox GetCheckBoxFeedBagOnlySourceCode()
        {
            return this.checkBoxFeedBagOnlySourceCode;
        }

        public CheckBox GetCheckBoxResearchGenericInteraction()
        {
            return this.checkBoxResearchGenericInteraction;
        }

        public CheckBox GetCheckBoxResearchProjectSpecific()
        {
            return this.checkBoxResearchProjectSpecific;
        }

        public CheckBox GetCheckBoxResearchSourceCode()
        {
            return this.checkBoxResearchSourceCode;
        }

        public CheckBox GetCheckBoxOpenDataSourceCode()
        {
            return this.checkBoxOpenDataSourceCode;
        }

        public CheckBox GetCheckBoxOpenDataProjectSpecific()
        {
            return this.checkBoxOpenDataProjectSpecific;
        }

        public CheckBox GetCheckBoxOpenDataGenericInteraction()
        {
            return this.checkBoxOpenDataGenericInteraction;
        }

        public CheckBox GetCheckBoxEnableDataCollection()
        {
            return this.checkBoxEnableDataCollection;
        }

        public void SetCurrentPrivacySettingsJObjectId(string newId)
        {
            _myCurrentSolutionId = newId;
        }

        public void SetDefaultPrivacySettingsJObject(JObject newJObject)
        {
            _myDefaultPrivacySettingsJson = newJObject;
        }

        public void SetCombobox(ComboBox newComboBox)
        {
            this.comboBoxSolutions = newComboBox;
        }

    }
}
