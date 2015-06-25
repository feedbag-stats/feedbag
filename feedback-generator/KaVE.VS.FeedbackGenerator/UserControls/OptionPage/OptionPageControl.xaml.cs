﻿/*
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

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Avalon.Windows.Dialogs;
using JetBrains.ActionManagement;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Features.Navigation.Resources;
using JetBrains.UI.CrossFramework;
using JetBrains.UI.Options;
using JetBrains.UI.Options.OptionPages.ToolsPages;
using KaVE.Commons.Model.Events.UserProfiles;
using KaVE.Commons.Utils;
using KaVE.RS.Commons.Settings.KaVE.RS.Commons.Settings;
using KaVE.VS.FeedbackGenerator.Settings;
using KaVE.VS.FeedbackGenerator.Settings.ExportSettingsSuite;
using KaVE.VS.FeedbackGenerator.UserControls.Anonymization;
using KaVE.VS.FeedbackGenerator.UserControls.UserProfile;
using KaVEISettingsStore = KaVE.RS.Commons.Settings.ISettingsStore;
using MessageBox = JetBrains.Util.MessageBox;

namespace KaVE.VS.FeedbackGenerator.UserControls.OptionPage
{
    [OptionsPage(PID, "KaVE Project", typeof (FeaturesFindingThemedIcons.SearchOptionsPage),
        ParentId = ToolsPage.PID)]
    public partial class OptionPageControl : IOptionsPage
    {
        private const string PID = "KaVE.VS.FeedbackGenerator.UserControls.OptionPage.OptionPageControl";

        private readonly Lifetime _lifetime;
        private readonly OptionsSettingsSmartContext _ctx;
        private readonly IActionManager _actionManager;
        private readonly KaVEISettingsStore _settingsStore;
        private readonly ExportSettings _exportSettings;
        private readonly UserProfileSettings _userProfileSettings;
        private readonly ModelStoreSettings _modelStoreSettings;
        private readonly UserProfileContext _userProfileContext;

        public OptionPageControl(Lifetime lifetime,
            OptionsSettingsSmartContext ctx,
            IActionManager actionManager,
            KaVEISettingsStore settingsStore,
            IRandomizationUtils rnd)
        {
            _lifetime = lifetime;
            _ctx = ctx;
            _actionManager = actionManager;
            _settingsStore = settingsStore;

            InitializeComponent();

            _exportSettings = settingsStore.GetSettings<ExportSettings>();
            _userProfileSettings = settingsStore.GetSettings<UserProfileSettings>();
            _modelStoreSettings = settingsStore.GetSettings<ModelStoreSettings>();

            _userProfileContext = new UserProfileContext(_exportSettings, _userProfileSettings, rnd);
            _userProfileContext.PropertyChanged += UserProfileContextOnPropertyChanged;

            DataContext = new OptionPageViewModel
            {
                ExportSettings = _exportSettings,
                ModelStoreSettings = _modelStoreSettings,
                AnonymizationContext = new AnonymizationContext(_exportSettings),
                UserProfileContext = _userProfileContext
            };

            if (_ctx != null)
            {
                BindToGeneralChanges();
                BindChangesToAnonymization();
                BindToUserProfileChanges();
            }
        }

        private void OnBrowse(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                ModelStorePathTextBox.Text = dialog.SelectedPath;
            }
        }

        private void OnReset(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.ShowYesNo(Properties.SessionManager.Option_SettingsCleaner_Dialog);
            if (result)
            {
                _actionManager.ExecuteActionGuarded<SettingsCleaner>(_lifetime);

                var window = Window.GetWindow(this);
                if (window != null)
                {
                    window.Close();
                }
            }
        }

        public bool OnOk()
        {
            // TODO: validation
            _settingsStore.SetSettings(_exportSettings);
            _settingsStore.SetSettings(_userProfileSettings);
            _settingsStore.SetSettings(_modelStoreSettings);
            return true;
        }

        public bool ValidatePage()
        {
            return true;
        }

        public EitherControl Control
        {
            get { return this; }
        }

        public string Id
        {
            get { return PID; }
        }

        #region jetbrains smart-context bindings

        private void BindToGeneralChanges()
        {
            _ctx.SetBinding(
                _lifetime,
                (ExportSettings s) => s.UploadUrl,
                UploadUrlTextBox,
                TextBox.TextProperty);
            _ctx.SetBinding(
                _lifetime,
                (ExportSettings s) => s.WebAccessPrefix,
                WebPraefixTextBox,
                TextBox.TextProperty);

            _ctx.SetBinding(
                _lifetime,
                (ModelStoreSettings s) => s.ModelStorePath,
                ModelStorePathTextBox,
                TextBox.TextProperty);
        }

        private void BindChangesToAnonymization()
        {
            _ctx.SetBinding(
                _lifetime,
                (ExportSettings s) => (bool?) s.RemoveCodeNames,
                Anonymization.RemoveCodeNamesCheckBox,
                ToggleButton.IsCheckedProperty);

            _ctx.SetBinding(
                _lifetime,
                (ExportSettings s) => (bool?) s.RemoveDurations,
                Anonymization.RemoveDurationsCheckBox,
                ToggleButton.IsCheckedProperty);

            _ctx.SetBinding(
                _lifetime,
                (ExportSettings s) => (bool?) s.RemoveSessionIDs,
                Anonymization.RemoveSessionIDsCheckBox,
                ToggleButton.IsCheckedProperty);

            _ctx.SetBinding(
                _lifetime,
                (ExportSettings s) => (bool?) s.RemoveStartTimes,
                Anonymization.RemoveStartTimesCheckBox,
                ToggleButton.IsCheckedProperty);
        }

        private void BindToUserProfileChanges()
        {
            // IsProviding
            _ctx.SetBinding(
                _lifetime,
                (UserProfileSettings s) => (bool?) s.IsProvidingProfile,
                UserProfile.IsProvidingProfileCheckBox,
                ToggleButton.IsCheckedProperty);

            // ProfileId
            _ctx.SetBinding(
                _lifetime,
                (UserProfileSettings s) => s.ProfileId,
                UserProfile.ProfileIdTextBox,
                TextBox.TextProperty);

            _ctx.SetBinding(
                _lifetime,
                (UserProfileSettings s) => s.Education,
                this,
                EducationProperty);

            _ctx.SetBinding(
                _lifetime,
                (UserProfileSettings s) => s.Position,
                this,
                PositionProperty);

            BindingForProjects();
            BindingForTeams();

            _ctx.SetBinding(
                _lifetime,
                (UserProfileSettings s) => s.CodeReviews,
                this,
                CodeReviewsProperty);

            _ctx.SetBinding(
                _lifetime,
                (UserProfileSettings s) => s.ProgrammingGeneral,
                this,
                ProgrammingGeneralProperty);

            _ctx.SetBinding(
                _lifetime,
                (UserProfileSettings s) => s.ProgrammingCSharp,
                this,
                ProgrammingCSharpProperty);
        }

        private void BindingForProjects()
        {
            _ctx.SetBinding(
                _lifetime,
                (UserProfileSettings s) => (bool?) s.ProjectsCourses,
                UserProfile.ProjectsCoursesCheckBox,
                ToggleButton.IsCheckedProperty);
            _ctx.SetBinding(
                _lifetime,
                (UserProfileSettings s) => (bool?) s.ProjectsPersonal,
                UserProfile.ProjectsPersonalCheckBox,
                ToggleButton.IsCheckedProperty);
            _ctx.SetBinding(
                _lifetime,
                (UserProfileSettings s) => (bool?) s.ProjectsSharedSmall,
                UserProfile.ProjectsSharedSmallCheckBox,
                ToggleButton.IsCheckedProperty);
            _ctx.SetBinding(
                _lifetime,
                (UserProfileSettings s) => (bool?) s.ProjectsSharedMedium,
                UserProfile.ProjectsSharedMediumCheckBox,
                ToggleButton.IsCheckedProperty);
            _ctx.SetBinding(
                _lifetime,
                (UserProfileSettings s) => (bool?) s.ProjectsSharedLarge,
                UserProfile.ProjectsSharedLargeCheckBox,
                ToggleButton.IsCheckedProperty);
        }

        private void BindingForTeams()
        {
            _ctx.SetBinding(
                _lifetime,
                (UserProfileSettings s) => (bool?) s.TeamsSolo,
                UserProfile.TeamsSoloCheckBox,
                ToggleButton.IsCheckedProperty);
            _ctx.SetBinding(
                _lifetime,
                (UserProfileSettings s) => (bool?) s.TeamsSmall,
                UserProfile.TeamsSmallCheckBox,
                ToggleButton.IsCheckedProperty);
            _ctx.SetBinding(
                _lifetime,
                (UserProfileSettings s) => (bool?) s.TeamsMedium,
                UserProfile.TeamsMediumCheckBox,
                ToggleButton.IsCheckedProperty);
            _ctx.SetBinding(
                _lifetime,
                (UserProfileSettings s) => (bool?) s.TeamsLarge,
                UserProfile.TeamsLargeCheckBox,
                ToggleButton.IsCheckedProperty);
        }

        #endregion

        #region ugly dependency property hack for JetBrains smart context

        private void UserProfileContextOnPropertyChanged(object sender,
            PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var propertyName = propertyChangedEventArgs.PropertyName;
            switch (propertyName)
            {
                case "Education":
                    Education = _userProfileContext.Education;
                    break;
                case "Position":
                    Position = _userProfileContext.Position;
                    break;
                case "CodeReviews":
                    CodeReviews = _userProfileContext.CodeReviews;
                    break;
                case "ProgrammingGeneral":
                    ProgrammingGeneral = _userProfileContext.ProgrammingGeneral;
                    break;
                case "ProgrammingCSharp":
                    ProgrammingCSharp = _userProfileContext.ProgrammingCSharp;
                    break;
            }
        }

        public static readonly DependencyProperty EducationProperty = DependencyProperty.Register(
            "Education",
            typeof (Educations),
            typeof (OptionPageControl)
            );

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
            "Position",
            typeof (Positions),
            typeof (OptionPageControl)
            );

        public static readonly DependencyProperty CodeReviewsProperty = DependencyProperty.Register(
            "CodeReviews",
            typeof (YesNoUnknown),
            typeof (OptionPageControl)
            );

        public static readonly DependencyProperty ProgrammingGeneralProperty = DependencyProperty.Register(
            "ProgrammingGeneral",
            typeof (Likert7Point),
            typeof (OptionPageControl)
            );

        public static readonly DependencyProperty ProgrammingCSharpProperty = DependencyProperty.Register(
            "ProgrammingCSharp",
            typeof (Likert7Point),
            typeof (OptionPageControl)
            );

        public Educations Education
        {
            get { return (Educations) GetValue(EducationProperty); }
            set { SetValue(EducationProperty, value); }
        }

        public Positions Position
        {
            get { return (Positions) GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public YesNoUnknown CodeReviews
        {
            get { return (YesNoUnknown) GetValue(CodeReviewsProperty); }
            set { SetValue(CodeReviewsProperty, value); }
        }

        public Likert7Point ProgrammingGeneral
        {
            get { return (Likert7Point) GetValue(ProgrammingGeneralProperty); }
            set { SetValue(ProgrammingGeneralProperty, value); }
        }

        public Likert7Point ProgrammingCSharp
        {
            get { return (Likert7Point) GetValue(ProgrammingCSharpProperty); }
            set { SetValue(ProgrammingCSharpProperty, value); }
        }

        #endregion
    }
}