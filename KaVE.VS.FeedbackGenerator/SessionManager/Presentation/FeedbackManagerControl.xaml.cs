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

using System.Windows;
using System.Windows.Controls;
using JetBrains.Application.UI.Options.Actions;
using JetBrains.DataFlow;
using KaVE.RS.Commons;
using KaVE.RS.Commons.Settings;
using KaVE.VS.Commons;
using KaVE.VS.FeedbackGenerator.Generators;
using KaVE.VS.FeedbackGenerator.Interactivity;
using KaVE.VS.FeedbackGenerator.Menu;
using KaVE.VS.FeedbackGenerator.Settings.ExportSettingsSuite;

namespace KaVE.VS.FeedbackGenerator.SessionManager.Presentation
{
    public partial class SessionManagerControl
    {
        private readonly FeedbackViewModel _feedbackViewModel;
        private readonly ActionExecutor _actionExec;
        private readonly ISettingsStore _settingsStore;
        private readonly IKaVECommandGenerator _cmdGen;

        public SessionManagerControl(FeedbackViewModel feedbackViewModel,
            ActionExecutor actionExec,
            ISettingsStore settingsStore,
            IKaVECommandGenerator cmdGen)
        {
            DataContext = feedbackViewModel;
            _feedbackViewModel = feedbackViewModel;
            _actionExec = actionExec;
            _feedbackViewModel.ConfirmationRequest.Raised += new ConfirmationRequestHandler(this).Handle;

            _settingsStore = settingsStore;
            _cmdGen = cmdGen;

            InitializeComponent();
        }

        public void OnVisibilityChanged(PropertyChangedEventArgs<bool> e)
        {
            var wasVisible = e.HasOld && e.Old;
            var isVisible = e.HasNew && e.New;
            if (!wasVisible && isVisible)
            {
                RefreshControl();
            }
        }

        private void RefreshButton_OnClick(object sender, RoutedEventArgs e)
        {
            _cmdGen.FireReloadEvents();
            RefreshControl();
        }

        private void RefreshControl()
        {
            _feedbackViewModel.Refresh();
        }

        /// <summary>
        ///     Makes the overflow dropdown button invisible.
        /// </summary>
        private void ToolBar_OnLoaded(object sender, RoutedEventArgs e)
        {
            var toolBar = sender as ToolBar;
            if (toolBar == null)
            {
                return;
            }
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
        }

        public void Export_OnClick(object sender, RoutedEventArgs e)
        {
            // no _cmdGen invocation, as it is done in the UploadWizardAction
            _actionExec.ExecuteActionGuarded<UploadWizardAction>();
        }

        private void VisitUploadPageButton_OnClick(object sender, RoutedEventArgs e)
        {
            _cmdGen.FireGotoUploadPage();
            var settingsStore = Registry.GetComponent<ISettingsStore>();
            var export = settingsStore.GetSettings<ExportSettings>();

            System.Diagnostics.Process.Start(export.UploadUrl);
        }

        private void VisitHomepageButton_OnClick(object sender, RoutedEventArgs e)
        {
            _cmdGen.FireGotoHomepage();
            var prefix = _settingsStore.GetSettings<ExportSettings>().WebAccessPrefix;
            System.Diagnostics.Process.Start(prefix + "http://kave.cc");
        }

        private void OpenOptionPage_OnClick(object sender, RoutedEventArgs e)
        {
            _cmdGen.FireOpenOptions();
            _actionExec.ExecuteActionGuarded<ShowOptionsAction>();
        }

        private void PossiblyInvisibleTabItem_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var tabItem = sender as TabItem;
            if (tabItem == null)
            {
                return;
            }

            if (tabItem.IsSelected && tabItem.DataContext == null)
            {
                var tabControl = tabItem.Parent as TabControl;
                if (tabControl == null)
                {
                    return;
                }

                tabControl.SelectedItem = tabControl.Items[0];
            }
        }
    }
}