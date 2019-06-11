using System.Collections;
using JetBrains.IDE.SolutionBuilders;
using JetBrains.ReSharper.Psi.Resx.Utils;
using KaVE.Commons.Model.Naming.Impl.v0.IDEComponents;

namespace Dashboard
{
    using System.Linq;
    using EnvDTE;
    using JetBrains.Application;
    using JetBrains.Application.Threading;
    using KaVE.Commons.Model.Events.VisualStudio;
    using KaVE.Commons.Model.Naming.IDEComponents;
    using KaVE.Commons.Utils;
    using KaVE.VS.Commons;
    using KaVE.VS.Commons.Generators;
    using KaVE.VS.Commons.Naming;

    partial class DashboardSettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBoxResearchSourceCode = new System.Windows.Forms.CheckBox();
            this.checkBoxOpenDataSourceCode = new System.Windows.Forms.CheckBox();
            this.checkBoxOpenDataProjectSpecific = new System.Windows.Forms.CheckBox();
            this.checkBoxOpenDataGenericInteraction = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.SaveDefaultSettingsButton = new System.Windows.Forms.Button();
            this.LoadDefaultSettingsButton = new System.Windows.Forms.Button();
            this.checkBoxResearchGenericInteraction = new System.Windows.Forms.CheckBox();
            this.checkBoxResearchProjectSpecific = new System.Windows.Forms.CheckBox();
            this.checkBoxFeedBagOnlyGenericInteraction = new System.Windows.Forms.CheckBox();
            this.checkBoxFeedBagOnlyProjectSpecific = new System.Windows.Forms.CheckBox();
            this.checkBoxFeedBagOnlySourceCode = new System.Windows.Forms.CheckBox();
            this.checkBoxEnableDataCollection = new System.Windows.Forms.CheckBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.comboBoxSolutions = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // checkBoxResearchSourceCode
            // 
            this.checkBoxResearchSourceCode.AutoSize = true;
            this.checkBoxResearchSourceCode.Enabled = false;
            this.checkBoxResearchSourceCode.Location = new System.Drawing.Point(424, 313);
            this.checkBoxResearchSourceCode.Name = "checkBoxResearchSourceCode";
            this.checkBoxResearchSourceCode.Size = new System.Drawing.Size(22, 21);
            this.checkBoxResearchSourceCode.TabIndex = 3;
            this.checkBoxResearchSourceCode.UseVisualStyleBackColor = true;
            this.checkBoxResearchSourceCode.CheckedChanged += new System.EventHandler(this.checkBoxResearchSourceCode_CheckedChanged);
            // 
            // checkBoxOpenDataSourceCode
            // 
            this.checkBoxOpenDataSourceCode.AutoSize = true;
            this.checkBoxOpenDataSourceCode.Enabled = false;
            this.checkBoxOpenDataSourceCode.Location = new System.Drawing.Point(607, 313);
            this.checkBoxOpenDataSourceCode.Name = "checkBoxOpenDataSourceCode";
            this.checkBoxOpenDataSourceCode.Size = new System.Drawing.Size(22, 21);
            this.checkBoxOpenDataSourceCode.TabIndex = 12;
            this.checkBoxOpenDataSourceCode.UseVisualStyleBackColor = true;
            this.checkBoxOpenDataSourceCode.CheckedChanged += new System.EventHandler(this.checkBoxOpenDataSourceCode_CheckedChanged);
            // 
            // checkBoxOpenDataProjectSpecific
            // 
            this.checkBoxOpenDataProjectSpecific.AutoSize = true;
            this.checkBoxOpenDataProjectSpecific.Enabled = false;
            this.checkBoxOpenDataProjectSpecific.Location = new System.Drawing.Point(607, 277);
            this.checkBoxOpenDataProjectSpecific.Name = "checkBoxOpenDataProjectSpecific";
            this.checkBoxOpenDataProjectSpecific.Size = new System.Drawing.Size(22, 21);
            this.checkBoxOpenDataProjectSpecific.TabIndex = 11;
            this.checkBoxOpenDataProjectSpecific.UseVisualStyleBackColor = true;
            this.checkBoxOpenDataProjectSpecific.CheckedChanged += new System.EventHandler(this.checkBoxOpenDataProjectSpecific_CheckedChanged);
            // 
            // checkBoxOpenDataGenericInteraction
            // 
            this.checkBoxOpenDataGenericInteraction.AutoSize = true;
            this.checkBoxOpenDataGenericInteraction.Enabled = false;
            this.checkBoxOpenDataGenericInteraction.Location = new System.Drawing.Point(607, 241);
            this.checkBoxOpenDataGenericInteraction.Name = "checkBoxOpenDataGenericInteraction";
            this.checkBoxOpenDataGenericInteraction.Size = new System.Drawing.Size(22, 21);
            this.checkBoxOpenDataGenericInteraction.TabIndex = 10;
            this.checkBoxOpenDataGenericInteraction.UseVisualStyleBackColor = true;
            this.checkBoxOpenDataGenericInteraction.CheckedChanged += new System.EventHandler(this.checkBoxOpenDataGenericInteraction_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(265, 201);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 20);
            this.label1.TabIndex = 20;
            this.label1.Text = "FeedBag Only\r\n";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(420, 201);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 20);
            this.label2.TabIndex = 21;
            this.label2.Text = "Research Purpose";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(603, 201);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 20);
            this.label3.TabIndex = 22;
            this.label3.Text = "Open Data Set";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 242);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(184, 20);
            this.label4.TabIndex = 23;
            this.label4.Text = "Generic Interaction Data";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 278);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(157, 20);
            this.label5.TabIndex = 24;
            this.label5.Text = "Project Specific Data";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 314);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(102, 20);
            this.label6.TabIndex = 25;
            this.label6.Text = "Source Code";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(21, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(405, 37);
            this.label7.TabIndex = 26;
            this.label7.Text = "Data Collection Settings for";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // SaveDefaultSettingsButton
            // 
            this.SaveDefaultSettingsButton.Location = new System.Drawing.Point(215, 405);
            this.SaveDefaultSettingsButton.Name = "SaveDefaultSettingsButton";
            this.SaveDefaultSettingsButton.Size = new System.Drawing.Size(177, 33);
            this.SaveDefaultSettingsButton.TabIndex = 30;
            this.SaveDefaultSettingsButton.Text = "Save Default Settings";
            this.SaveDefaultSettingsButton.UseVisualStyleBackColor = true;
            this.SaveDefaultSettingsButton.Click += new System.EventHandler(this.SaveDefaultSettingsButton_Click);
            // 
            // LoadDefaultSettingsButton
            // 
            this.LoadDefaultSettingsButton.Location = new System.Drawing.Point(28, 405);
            this.LoadDefaultSettingsButton.Name = "LoadDefaultSettingsButton";
            this.LoadDefaultSettingsButton.Size = new System.Drawing.Size(181, 33);
            this.LoadDefaultSettingsButton.TabIndex = 31;
            this.LoadDefaultSettingsButton.Text = "Load Default Settings";
            this.LoadDefaultSettingsButton.UseVisualStyleBackColor = true;
            this.LoadDefaultSettingsButton.Click += new System.EventHandler(this.LoadDefaultSettingsButton_Click);
            // 
            // checkBoxResearchGenericInteraction
            // 
            this.checkBoxResearchGenericInteraction.AutoSize = true;
            this.checkBoxResearchGenericInteraction.Enabled = false;
            this.checkBoxResearchGenericInteraction.Location = new System.Drawing.Point(424, 241);
            this.checkBoxResearchGenericInteraction.Name = "checkBoxResearchGenericInteraction";
            this.checkBoxResearchGenericInteraction.Size = new System.Drawing.Size(22, 21);
            this.checkBoxResearchGenericInteraction.TabIndex = 32;
            this.checkBoxResearchGenericInteraction.UseVisualStyleBackColor = true;
            this.checkBoxResearchGenericInteraction.CheckedChanged += new System.EventHandler(this.checkBoxResearchGenericInteraction_CheckedChanged);
            // 
            // checkBoxResearchProjectSpecific
            // 
            this.checkBoxResearchProjectSpecific.AutoSize = true;
            this.checkBoxResearchProjectSpecific.Enabled = false;
            this.checkBoxResearchProjectSpecific.Location = new System.Drawing.Point(424, 277);
            this.checkBoxResearchProjectSpecific.Name = "checkBoxResearchProjectSpecific";
            this.checkBoxResearchProjectSpecific.Size = new System.Drawing.Size(22, 21);
            this.checkBoxResearchProjectSpecific.TabIndex = 34;
            this.checkBoxResearchProjectSpecific.UseVisualStyleBackColor = true;
            this.checkBoxResearchProjectSpecific.CheckedChanged += new System.EventHandler(this.checkBoxResearchProjectSpecific_CheckedChanged);
            // 
            // checkBoxFeedBagOnlyGenericInteraction
            // 
            this.checkBoxFeedBagOnlyGenericInteraction.AutoSize = true;
            this.checkBoxFeedBagOnlyGenericInteraction.Checked = true;
            this.checkBoxFeedBagOnlyGenericInteraction.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFeedBagOnlyGenericInteraction.Enabled = false;
            this.checkBoxFeedBagOnlyGenericInteraction.Location = new System.Drawing.Point(269, 241);
            this.checkBoxFeedBagOnlyGenericInteraction.Name = "checkBoxFeedBagOnlyGenericInteraction";
            this.checkBoxFeedBagOnlyGenericInteraction.Size = new System.Drawing.Size(22, 21);
            this.checkBoxFeedBagOnlyGenericInteraction.TabIndex = 35;
            this.checkBoxFeedBagOnlyGenericInteraction.UseVisualStyleBackColor = true;
            this.checkBoxFeedBagOnlyGenericInteraction.CheckedChanged += new System.EventHandler(this.checkBoxFeedBagOnlyGenericInteraction_CheckedChanged);
            // 
            // checkBoxFeedBagOnlyProjectSpecific
            // 
            this.checkBoxFeedBagOnlyProjectSpecific.AutoSize = true;
            this.checkBoxFeedBagOnlyProjectSpecific.Checked = true;
            this.checkBoxFeedBagOnlyProjectSpecific.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFeedBagOnlyProjectSpecific.Enabled = false;
            this.checkBoxFeedBagOnlyProjectSpecific.Location = new System.Drawing.Point(269, 277);
            this.checkBoxFeedBagOnlyProjectSpecific.Name = "checkBoxFeedBagOnlyProjectSpecific";
            this.checkBoxFeedBagOnlyProjectSpecific.Size = new System.Drawing.Size(22, 21);
            this.checkBoxFeedBagOnlyProjectSpecific.TabIndex = 36;
            this.checkBoxFeedBagOnlyProjectSpecific.UseVisualStyleBackColor = true;
            this.checkBoxFeedBagOnlyProjectSpecific.CheckedChanged += new System.EventHandler(this.checkBoxFeedBagOnlyProjectSpecific_CheckedChanged);
            // 
            // checkBoxFeedBagOnlySourceCode
            // 
            this.checkBoxFeedBagOnlySourceCode.AutoSize = true;
            this.checkBoxFeedBagOnlySourceCode.Checked = true;
            this.checkBoxFeedBagOnlySourceCode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFeedBagOnlySourceCode.Enabled = false;
            this.checkBoxFeedBagOnlySourceCode.Location = new System.Drawing.Point(269, 313);
            this.checkBoxFeedBagOnlySourceCode.Name = "checkBoxFeedBagOnlySourceCode";
            this.checkBoxFeedBagOnlySourceCode.Size = new System.Drawing.Size(22, 21);
            this.checkBoxFeedBagOnlySourceCode.TabIndex = 37;
            this.checkBoxFeedBagOnlySourceCode.UseVisualStyleBackColor = true;
            this.checkBoxFeedBagOnlySourceCode.CheckedChanged += new System.EventHandler(this.checkBoxFeedBagOnlySourceCode_CheckedChanged);
            // 
            // checkBoxEnableDataCollection
            // 
            this.checkBoxEnableDataCollection.AutoSize = true;
            this.checkBoxEnableDataCollection.Location = new System.Drawing.Point(28, 121);
            this.checkBoxEnableDataCollection.Name = "checkBoxEnableDataCollection";
            this.checkBoxEnableDataCollection.Size = new System.Drawing.Size(302, 24);
            this.checkBoxEnableDataCollection.TabIndex = 38;
            this.checkBoxEnableDataCollection.Text = "Enable data collection for this solution";
            this.checkBoxEnableDataCollection.UseVisualStyleBackColor = true;
            this.checkBoxEnableDataCollection.CheckedChanged += new System.EventHandler(this.checkBoxEnableDataCollection_CheckedChanged);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(621, 405);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 33);
            this.buttonCancel.TabIndex = 39;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(702, 405);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 33);
            this.buttonSave.TabIndex = 40;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // comboBoxSolutions
            // 
            this.comboBoxSolutions.AllowDrop = true;
            this.comboBoxSolutions.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxSolutions.FormattingEnabled = true;
            this.comboBoxSolutions.Items.AddRange(new object[] {
            "Solution 1 ",
            "Solution 2",
            "Solution 3",
            "Solution 4"});
            this.comboBoxSolutions.Location = new System.Drawing.Point(424, 41);
            this.comboBoxSolutions.Name = "comboBoxSolutions";
            this.comboBoxSolutions.Size = new System.Drawing.Size(345, 45);
            this.comboBoxSolutions.TabIndex = 41;
            this.comboBoxSolutions.Text = "Solution";
            this.comboBoxSolutions.SelectedIndexChanged += new System.EventHandler(this.comboBoxSolutions_SelectedIndexChanged);
            // 
            // DashboardSettingsForm
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.OutlineButton;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.comboBoxSolutions);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.checkBoxEnableDataCollection);
            this.Controls.Add(this.checkBoxFeedBagOnlySourceCode);
            this.Controls.Add(this.checkBoxFeedBagOnlyProjectSpecific);
            this.Controls.Add(this.checkBoxFeedBagOnlyGenericInteraction);
            this.Controls.Add(this.checkBoxResearchProjectSpecific);
            this.Controls.Add(this.checkBoxResearchGenericInteraction);
            this.Controls.Add(this.LoadDefaultSettingsButton);
            this.Controls.Add(this.SaveDefaultSettingsButton);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBoxOpenDataSourceCode);
            this.Controls.Add(this.checkBoxOpenDataProjectSpecific);
            this.Controls.Add(this.checkBoxOpenDataGenericInteraction);
            this.Controls.Add(this.checkBoxResearchSourceCode);
            this.Name = "DashboardSettingsForm";
            this.Text = "Dashboard Privacy Settings";
            this.Load += new System.EventHandler(this.DashboardSettingsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox checkBoxResearchSourceCode;
        private System.Windows.Forms.CheckBox checkBoxOpenDataSourceCode;
        private System.Windows.Forms.CheckBox checkBoxOpenDataProjectSpecific;
        private System.Windows.Forms.CheckBox checkBoxOpenDataGenericInteraction;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button SaveDefaultSettingsButton;
        private System.Windows.Forms.Button LoadDefaultSettingsButton;
        private System.Windows.Forms.CheckBox checkBoxResearchGenericInteraction;
        private System.Windows.Forms.CheckBox checkBoxResearchProjectSpecific;
        private System.Windows.Forms.CheckBox checkBoxFeedBagOnlyGenericInteraction;
        private System.Windows.Forms.CheckBox checkBoxFeedBagOnlyProjectSpecific;
        private System.Windows.Forms.CheckBox checkBoxFeedBagOnlySourceCode;
        private System.Windows.Forms.CheckBox checkBoxEnableDataCollection;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.ComboBox comboBoxSolutions;
    }
}