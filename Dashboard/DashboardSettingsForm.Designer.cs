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
            this.checkBoxResearchGenericInteraction = new System.Windows.Forms.CheckBox();
            this.checkBox2ResearchProjectSpecific = new System.Windows.Forms.CheckBox();
            this.checkBoxResearchSourceCode = new System.Windows.Forms.CheckBox();
            this.checkBoxFeedBagOnlySourceCode = new System.Windows.Forms.CheckBox();
            this.checkBoxFeedBagOnlyProjectSpecific = new System.Windows.Forms.CheckBox();
            this.checkBoxFeedBagOnlyGenericInteraction = new System.Windows.Forms.CheckBox();
            this.checkBoxOpenDataSourceCode = new System.Windows.Forms.CheckBox();
            this.checkBoxOpenDataProjectSpecific = new System.Windows.Forms.CheckBox();
            this.checkBoxOpenDataGenericInteraction = new System.Windows.Forms.CheckBox();
            this.checkBox10 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SaveDefaultSettingsButton = new System.Windows.Forms.Button();
            this.LoadDefaultSettingsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkBoxResearchGenericInteraction
            // 
            this.checkBoxResearchGenericInteraction.AutoSize = true;
            this.checkBoxResearchGenericInteraction.Enabled = false;
            this.checkBoxResearchGenericInteraction.Location = new System.Drawing.Point(424, 241);
            this.checkBoxResearchGenericInteraction.Name = "checkBoxResearchGenericInteraction";
            this.checkBoxResearchGenericInteraction.Size = new System.Drawing.Size(22, 21);
            this.checkBoxResearchGenericInteraction.TabIndex = 1;
            this.checkBoxResearchGenericInteraction.UseVisualStyleBackColor = true;
            this.checkBoxResearchGenericInteraction.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2ResearchProjectSpecific
            // 
            this.checkBox2ResearchProjectSpecific.AutoSize = true;
            this.checkBox2ResearchProjectSpecific.Enabled = false;
            this.checkBox2ResearchProjectSpecific.Location = new System.Drawing.Point(424, 277);
            this.checkBox2ResearchProjectSpecific.Name = "checkBox2ResearchProjectSpecific";
            this.checkBox2ResearchProjectSpecific.Size = new System.Drawing.Size(22, 21);
            this.checkBox2ResearchProjectSpecific.TabIndex = 2;
            this.checkBox2ResearchProjectSpecific.UseVisualStyleBackColor = true;
            this.checkBox2ResearchProjectSpecific.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
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
            // checkBoxFeedBagOnlySourceCode
            // 
            this.checkBoxFeedBagOnlySourceCode.AutoSize = true;
            this.checkBoxFeedBagOnlySourceCode.Checked = true;
            this.checkBoxFeedBagOnlySourceCode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFeedBagOnlySourceCode.Enabled = false;
            this.checkBoxFeedBagOnlySourceCode.Location = new System.Drawing.Point(269, 313);
            this.checkBoxFeedBagOnlySourceCode.Name = "checkBoxFeedBagOnlySourceCode";
            this.checkBoxFeedBagOnlySourceCode.Size = new System.Drawing.Size(22, 21);
            this.checkBoxFeedBagOnlySourceCode.TabIndex = 6;
            this.checkBoxFeedBagOnlySourceCode.UseVisualStyleBackColor = true;
            this.checkBoxFeedBagOnlySourceCode.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
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
            this.checkBoxFeedBagOnlyProjectSpecific.TabIndex = 5;
            this.checkBoxFeedBagOnlyProjectSpecific.UseVisualStyleBackColor = true;
            this.checkBoxFeedBagOnlyProjectSpecific.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged);
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
            this.checkBoxFeedBagOnlyGenericInteraction.TabIndex = 4;
            this.checkBoxFeedBagOnlyGenericInteraction.UseVisualStyleBackColor = true;
            this.checkBoxFeedBagOnlyGenericInteraction.CheckedChanged += new System.EventHandler(this.checkBox6_CheckedChanged);
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
            // checkBox10
            // 
            this.checkBox10.AutoSize = true;
            this.checkBox10.Location = new System.Drawing.Point(28, 125);
            this.checkBox10.Name = "checkBox10";
            this.checkBox10.Size = new System.Drawing.Size(302, 24);
            this.checkBox10.TabIndex = 16;
            this.checkBox10.Text = "Enable data collection for this solution";
            this.checkBox10.UseVisualStyleBackColor = true;
            this.checkBox10.CheckedChanged += new System.EventHandler(this.checkBox10_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(702, 405);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 33);
            this.button1.TabIndex = 18;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(621, 405);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 33);
            this.button2.TabIndex = 19;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
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
            // comboBox1
            // 

            var solutions = JetBrains.Rider.Model.Solution.Read;
            // var solutions = Registry.GetComponent<ISolutionName>();
            // var solution = DTE.Solution.GetName();


            /*var solutions = DTE.Solution.GetName();*/
            /*DTE dte = new DTEClass();
            var solutionEvents = dte.Events.SolutionEvents;
            JetBrains.ProjectModel.SolutionsManager solutionMangager = new JetBrains.ProjectModel.SolutionsManager(null);
            JetBrains.DataFlow.IViewable<JetBrains.ProjectModel.ISolution> solutions = solutionMangager.Solutions;*/
            /*foreach (var solution in solutions)
            {
                this.comboBox1.Items.AddRange(new object[] {solution.ToString()});
            }*/

            this.comboBox1.AllowDrop = true;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {solutions});

            this.comboBox1.Items.AddRange(new object[] {
            "Solution 1 ",
            "Solution 2",
            "Solution 3",
            "Solution 4"});
            this.comboBox1.Location = new System.Drawing.Point(432, 41);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(345, 45);
            this.comboBox1.TabIndex = 28;
            this.comboBox1.Text = "Solution";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
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
            // DashboardSettingsForm
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.OutlineButton;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.LoadDefaultSettingsButton);
            this.Controls.Add(this.SaveDefaultSettingsButton);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox10);
            this.Controls.Add(this.checkBoxOpenDataSourceCode);
            this.Controls.Add(this.checkBoxOpenDataProjectSpecific);
            this.Controls.Add(this.checkBoxOpenDataGenericInteraction);
            this.Controls.Add(this.checkBoxFeedBagOnlySourceCode);
            this.Controls.Add(this.checkBoxFeedBagOnlyProjectSpecific);
            this.Controls.Add(this.checkBoxFeedBagOnlyGenericInteraction);
            this.Controls.Add(this.checkBoxResearchSourceCode);
            this.Controls.Add(this.checkBox2ResearchProjectSpecific);
            this.Controls.Add(this.checkBoxResearchGenericInteraction);
            this.Name = "DashboardSettingsForm";
            this.Text = "Dashboard Privacy Settings";
            this.Load += new System.EventHandler(this.DashboardSettingsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox checkBoxResearchGenericInteraction;
        private System.Windows.Forms.CheckBox checkBox2ResearchProjectSpecific;
        private System.Windows.Forms.CheckBox checkBoxResearchSourceCode;
        private System.Windows.Forms.CheckBox checkBoxFeedBagOnlySourceCode;
        private System.Windows.Forms.CheckBox checkBoxFeedBagOnlyProjectSpecific;
        private System.Windows.Forms.CheckBox checkBoxFeedBagOnlyGenericInteraction;
        private System.Windows.Forms.CheckBox checkBoxOpenDataSourceCode;
        private System.Windows.Forms.CheckBox checkBoxOpenDataProjectSpecific;
        private System.Windows.Forms.CheckBox checkBoxOpenDataGenericInteraction;
        private System.Windows.Forms.CheckBox checkBox10;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button SaveDefaultSettingsButton;
        private System.Windows.Forms.Button LoadDefaultSettingsButton;
    }
}