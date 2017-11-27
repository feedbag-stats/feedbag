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

using JetBrains.Application;
using JetBrains.Util;
using KaVE.Commons.Utils;
using KaVE.RS.Commons;
using KaVE.RS.Commons.Settings;
using KaVE.VS.Commons;
using KaVE.VS.FeedbackGenerator.Properties;
using KaVE.VS.FeedbackGenerator.UserControls.UploadWizard;
using KaVE.VS.FeedbackGenerator.UserControls.UserProfileDialogs;
using KaVE.VS.FeedbackGenerator.Utils.Export;
using KaVE.VS.FeedbackGenerator.Utils.Logging;
using ILogger = KaVE.Commons.Utils.Exceptions.ILogger;

namespace KaVE.VS.FeedbackGenerator.Menu
{
    public interface IUploadWizardWindowCreator
    {
        void OpenUserProfile();

        void OpenUploadWizard();

        void OpenNothingToExport();
    }

    [ShellComponent]
    public class UploadWizardWindowCreator : IUploadWizardWindowCreator
    {
        private readonly ISettingsStore _settingsStore;
        private readonly IExporter _exporter;
        private readonly ILogManager _logManager;
        private readonly ILogger _logger;
        private readonly IDateUtils _dateUtils;
        private readonly ActionExecutor _actionExecutor;
        private readonly IUserProfileSettingsUtils _userSettingsUtil;

        public UploadWizardWindowCreator(ISettingsStore settingsStore,
            IExporter exporter,
            ILogManager logManager,
            ILogger logger,
            IDateUtils dateUtils,
            ActionExecutor actionExecutor,
            IUserProfileSettingsUtils userSettingsUtil)
        {
            _settingsStore = settingsStore;
            _exporter = exporter;
            _logManager = logManager;
            _logger = logger;
            _dateUtils = dateUtils;
            _actionExecutor = actionExecutor;
            _userSettingsUtil = userSettingsUtil;
        }

        public void OpenUserProfile()
        {
            new UserProfileDialog(
                _actionExecutor,
                UploadWizardPolicy.OpenUploadWizardOnFinish,
                _userSettingsUtil).Show();
        }

        public void OpenUploadWizard()
        {
            var actExec = Registry.GetComponent<IActionExecutor>();
            var viewModel = new UploadWizardContext(
                _exporter,
                _logManager,
                _settingsStore,
                _dateUtils,
                _logger);
            new UploadWizardControl(viewModel, _settingsStore, actExec, _userSettingsUtil).ShowDialog();
        }

        public void OpenNothingToExport()
        {
            MessageBox.ShowInfo(UploadWizard.NothingToExport, UploadWizardMessages.Title);
        }
    }
}