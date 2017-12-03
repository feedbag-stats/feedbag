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

using System;
using EnvDTE;
using JetBrains.Application;
using JetBrains.Threading;
using KaVE.Commons.Model.Events.CompletionEvents;
using KaVE.Commons.Model.Events.VisualStudio;
using KaVE.Commons.Utils;
using KaVE.VS.Commons;
using KaVE.VS.Commons.Generators;
using KaVE.VS.FeedbackGenerator.Generators.VisualStudio.EditEventGenerators.EventContext;

namespace KaVE.VS.FeedbackGenerator.Generators.VisualStudio.EditEventGenerators
{
    [ShellComponent]
    internal class EditEventGenerator : EventGeneratorBase
    {
        // TODO evaluate good threshold value
        private static readonly TimeSpan InactivityPeriodToCompleteEditAction = TimeSpan.FromSeconds(2);

        private readonly IDateUtils _dateUtils;
        private readonly ContextGenerator _contextGenerator;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly TextEditorEvents _textEditorEvents;

        private EditEvent _currentEditEvent;

        public EditEventGenerator(IRSEnv env,
            IMessageBus messageBus,
            IDateUtils dateUtils,
            ContextGenerator contextGenerator,
            IThreading threading)
            : base(env, messageBus, dateUtils, threading)
        {
            _dateUtils = dateUtils;
            _contextGenerator = contextGenerator;
            _textEditorEvents = DTE.Events.TextEditorEvents;
            _textEditorEvents.LineChanged += TextEditorEvents_LineChanged;
        }

        private void TextEditorEvents_LineChanged(TextPoint startPoint, TextPoint endPoint, int hint)
        {
            if (_currentEditEvent == null)
            {
                _currentEditEvent = Create<EditEvent>();
                _currentEditEvent.Context2 = _contextGenerator.GetCurrentContext(startPoint);
            }

            _currentEditEvent.NumberOfChanges += 1;
            // TODO subtract whitespaces from change size
            _currentEditEvent.SizeOfChanges += endPoint.AbsoluteCharOffset - startPoint.AbsoluteCharOffset;

            if (_currentEditEvent.TriggeredAt < _dateUtils.Now - InactivityPeriodToCompleteEditAction)
            {
                FireNow(_currentEditEvent);
                _currentEditEvent = null;
            }
        }
    }
}