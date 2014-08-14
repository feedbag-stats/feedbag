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
 * 
 * Contributors:
 *    - Sven Amann
 *    - Uli Fahrer
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using KaVE.Model.Events;
using KaVE.Utils;
using KaVE.VsFeedbackGenerator.Generators;
using KaVE.VsFeedbackGenerator.Interactivity;
using KaVE.VsFeedbackGenerator.SessionManager;
using KaVE.VsFeedbackGenerator.SessionManager.Presentation;
using KaVE.VsFeedbackGenerator.Utils;
using KaVE.VsFeedbackGenerator.Utils.Logging;

namespace KaVE.VsFeedbackGenerator.Export
{
    public class UploadWizardViewModel : ViewModelBase<UploadWizardViewModel>
    {
        private readonly IExporter _exporter;
        private readonly ILogManager<IDEEvent> _logManager;
        private readonly ISettingsStore _settingsStore;
        private readonly IDateUtils _dateUtils;
        private readonly BackgroundWorker _exportWorker;
        private DateTime _exportEndDate;

        private readonly InteractionRequest<Notification> _errorNotificationRequest;
        private readonly InteractionRequest<LinkNotification> _successNotificationRequest; 

        public IInteractionRequest<Notification> ErrorNotificationRequest
        {
            get { return _errorNotificationRequest; }
        }

        public IInteractionRequest<LinkNotification> SuccessNotificationRequest
        {
            get { return _successNotificationRequest; }
        }

        public UploadWizardViewModel(IExporter exporter,
            ILogManager<IDEEvent> logManager,
            ISettingsStore settingsStore,
            IDateUtils dateUtils)
        {
            _exporter = exporter;
            _logManager = logManager;
            _settingsStore = settingsStore;
            _dateUtils = dateUtils;
            _errorNotificationRequest = new InteractionRequest<Notification>();
            _successNotificationRequest = new InteractionRequest<LinkNotification>();
            _exportWorker = new BackgroundWorker {WorkerSupportsCancellation = false};
            _exportWorker.DoWork += OnExport;
            _exportWorker.RunWorkerCompleted += OnExportCompleted;
        }

        private ExportSettings ExportSettings
        {
            get { return _settingsStore.GetSettings<ExportSettings>(); }
        }

        public bool RemoveCodeNames
        {
            get { return ExportSettings.RemoveCodeNames; }
            set { _settingsStore.UpdateSettings<ExportSettings>(s => s.RemoveCodeNames = value); }
        }

        public bool RemoveDurations
        {
            get { return ExportSettings.RemoveDurations; }
            set { _settingsStore.UpdateSettings<ExportSettings>(s => s.RemoveDurations = value); }
        }

        public bool RemoveSessionIDs
        {
            get { return ExportSettings.RemoveSessionIDs; }
            set { _settingsStore.UpdateSettings<ExportSettings>(s => s.RemoveSessionIDs = value); }
        }

        public bool RemoveStartTimes
        {
            get { return ExportSettings.RemoveStartTimes; }
            set { _settingsStore.UpdateSettings<ExportSettings>(s => s.RemoveStartTimes = value); }
        }

        internal void Export(UploadWizard.ExportType exportType)
        {
            SetBusy(Properties.UploadWizard.Export_BusyMessage);
            var exportSettings = ExportSettings;
            _exportEndDate = exportSettings.LastReviewDate ?? _dateUtils.Now;
            _exportWorker.RunWorkerAsync(exportType);
        }

        private void OnExport(object worker, DoWorkEventArgs e)
        {
            var exportType = (UploadWizard.ExportType) e.Argument;
            var events = ExtractEventsForExport();

            if (exportType == UploadWizard.ExportType.ZipFile)
            {
                _exporter.Export(events, new FilePublisher(AskForExportLocation));
            }
            else
            {
                _exporter.Export(events, new HttpPublisher(GetUploadUrl()));
            }

            _logManager.DeleteLogsOlderThan(_exportEndDate);
            e.Result = events.Count;
        }

        private IList<IDEEvent> ExtractEventsForExport()
        {
            var events = new List<IDEEvent>();
            foreach (var log in _logManager.Logs)
            {
                using (var reader = log.NewLogReader())
                {
                    events.AddRange(reader.ReadAll().Where(e => e.TriggeredAt <= _exportEndDate));
                }
            }
            return events;
        }

        private static string AskForExportLocation()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = Properties.SessionManager.SaveFileDialogFilter,
                AddExtension = true
            };
            var result = Invoke.OnSTA(() => saveFileDialog.ShowDialog());
            return result == DialogResult.Cancel ? null : saveFileDialog.FileName;
        }

        private Uri GetUploadUrl()
        {
            var exportSettings = _settingsStore.GetSettings<ExportSettings>();
            return new Uri(exportSettings.UploadUrl);
        }

        private void OnExportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                UpdateLastUploadDate();
                ShowExportSucceededMessage((int) e.Result);
            }
            else
            {
                ShowExportFailedMessage(e.Error.Message);
                Registry.GetComponent<ILogger>().Error(e.Error, "export failed");
            }
            SetIdle();
        }

        private void UpdateLastUploadDate()
        {
            var settings = _settingsStore.GetSettings<UploadSettings>();
            settings.LastUploadDate = _dateUtils.Now;
            _settingsStore.SetSettings(settings);
        }

        private void ShowExportSucceededMessage(int numberOfExportedEvents)
        {
            var export = _settingsStore.GetSettings<ExportSettings>();
            RaiseLinkNotificationRequest(
                string.Format(Properties.UploadWizard.ExportSuccess, numberOfExportedEvents),
                export.UploadUrl);
        }

        private void ShowExportFailedMessage(string message)
        {
            RaiseNotificationRequest(
                Properties.UploadWizard.ExportFail + (string.IsNullOrWhiteSpace(message) ? "" : ":\n" + message));
        }


        private void RaiseNotificationRequest(string text)
        {
            _errorNotificationRequest.Raise(
                new Notification
                {
                    Caption = Properties.UploadWizard.window_title,
                    Message = text,
                });
        }

        private void RaiseLinkNotificationRequest(string text, string url)
        {
            _successNotificationRequest.Raise(
                new LinkNotification
                {
                    Caption = Properties.UploadWizard.window_title,
                    Message = text,
                    LinkDescription =  Properties.UploadWizard.ExportSuccessLinkDescription,
                    Link = url,
                });
        }
    }
}