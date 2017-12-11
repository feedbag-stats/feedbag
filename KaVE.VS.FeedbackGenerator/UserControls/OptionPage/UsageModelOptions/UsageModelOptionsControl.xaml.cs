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
using Avalon.Windows.Dialogs;
using JetBrains.Application.DataContext;
using JetBrains.Application.Settings;
using JetBrains.Application.UI.Options;
using JetBrains.Application.UI.Options.Options.ThemedIcons;
using JetBrains.Application.UI.UIAutomation;
using JetBrains.DataFlow;
using KaVE.RS.Commons;
using KaVE.RS.Commons.Settings.KaVE.RS.Commons.Settings;
using KaVE.VS.FeedbackGenerator.Settings;
using KaVE.VS.FeedbackGenerator.Utils;
using KaVEISettingsStore = KaVE.RS.Commons.Settings.ISettingsStore;

namespace KaVE.VS.FeedbackGenerator.UserControls.OptionPage.UsageModelOptions
{
    [OptionsPage(
        PID,
        "Usage Models",
        typeof(OptionsThemedIcons.ImportLayer),
        ParentId = RootOptionPage.PID,
        Sequence = 4.0)]
    public partial class UsageModelOptionsControl : IOptionsPage
    {
        private const string PID =
            "KaVE.VS.FeedbackGenerator.UserControls.OptionPage.UsageModelOptions.UsageModelOptionsControl";

        public const ResetTypes ResetType = ResetTypes.ModelStoreSettings;

        private readonly Lifetime _lifetime;
        private readonly KaVEISettingsStore _settingsStore;
        private readonly IActionExecutor _actionExecutor;
        private readonly DataContexts _dataContexts;
        private readonly ModelStoreSettings _modelStoreSettings;
        private readonly IMessageBoxCreator _messageBoxCreator;

        public UsageModelOptionsControl(Lifetime lifetime,
            OptionsSettingsSmartContext ctx,
            KaVEISettingsStore settingsStore,
            IActionExecutor actionExecutor,
            DataContexts dataContexts,
            IMessageBoxCreator messageBoxCreator)
        {
            _messageBoxCreator = messageBoxCreator;
            _lifetime = lifetime;
            _settingsStore = settingsStore;
            _actionExecutor = actionExecutor;
            _dataContexts = dataContexts;
            InitializeComponent();

            _modelStoreSettings = settingsStore.GetSettings<ModelStoreSettings>();

            DataContext = new UsageModelOptionsViewModel
            {
                ModelStoreSettings = _modelStoreSettings
            };

            if (ctx != null)
            {
                // Binding to ModelStorePath
                ctx.SetBinding(
                    lifetime,
                    (ModelStoreSettings s) => s.ModelStorePath,
                    ModelStorePathTextBox,
                    TextBox.TextProperty);
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

        private void OnResetSettings(object sender, RoutedEventArgs e)
        {
            var result = _messageBoxCreator.ShowYesNo(UsageModelOptionsMessages.SettingResetDialog);
            if (result)
            {
                var settingResetType = new SettingResetType {ResetType = ResetType};
                _actionExecutor.ExecuteActionGuarded<SettingsCleaner>(
                    settingResetType.GetDataContextForSettingResultType(_dataContexts, _lifetime));

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
    }
}