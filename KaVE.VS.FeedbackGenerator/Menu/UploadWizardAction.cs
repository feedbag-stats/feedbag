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

using System.IO;
using System.Linq;
using JetBrains.ActionManagement;
using JetBrains.Application;
using JetBrains.Application.DataContext;
using JetBrains.UI.ActionsRevised;
using KaVE.RS.Commons.Settings;
using KaVE.VS.FeedbackGenerator.Generators;
using KaVE.VS.FeedbackGenerator.Utils.Logging;
using NuGet;

namespace KaVE.VS.FeedbackGenerator.Menu
{
    [Action(Id, "Export Feedback...", Id = 12193486)]
    public class UploadWizardAction : MenuActionBase
    {
        internal const string Id = "KaVE.VS.FeedbackGenerator.UploadWizard";
    }

    [ShellComponent]
    public class UploadWizardActionHandler : MenuActionHandlerBase<UploadWizardAction>
    {
        private readonly IUploadWizardWindowCreator _uploadWizardWindowCreator;
        private readonly ILogManager _logManager;
        private readonly IKaVECommandGenerator _cmdGen;
        private readonly IUserProfileSettingsUtils _userProfileSettingsUtils;

        public UploadWizardActionHandler(IUploadWizardWindowCreator uploadWizardWindowCreator,
            IUserProfileSettingsUtils userProfileSettingsUtils,
            ILogManager logManager,
            IKaVECommandGenerator cmdGen,
            IActionManager am) : base(am)
        {
            _uploadWizardWindowCreator = uploadWizardWindowCreator;
            _userProfileSettingsUtils = userProfileSettingsUtils;
            _logManager = logManager;
            _cmdGen = cmdGen;
        }

        public override void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            _cmdGen.FireOpenExportDialog();
            _userProfileSettingsUtils.EnsureProfileId();

            if (!_userProfileSettingsUtils.HasBeenAskedToFillProfile())
            {
                _uploadWizardWindowCreator.OpenUserProfile();
            }
            else
            {
                if (HasContentToExport())
                {
                    _uploadWizardWindowCreator.OpenUploadWizard();
                }
                else
                {
                    _uploadWizardWindowCreator.OpenNothingToExport();
                }
            }
        }

        private bool HasContentToExport()
        {
            try
            {
                var logs = _logManager.Logs.ToList();
                var noLogs = logs.IsEmpty();
                var singleEmptyLog = logs.Count == 1 && logs[0].IsEmpty();
                return !noLogs && !singleEmptyLog;
            }
            catch (DirectoryNotFoundException)
            {
                // directly after the reset, before any event is generated
                return false;
            }
        }
    }
}