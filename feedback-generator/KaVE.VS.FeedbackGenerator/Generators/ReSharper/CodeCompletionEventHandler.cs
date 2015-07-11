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
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Features.Intellisense.CodeCompletion.CSharp.Rules;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Resources.Shell;
using KaVE.Commons.Model.Events;
using KaVE.Commons.Model.Events.CompletionEvents;
using KaVE.Commons.Utils;
using KaVE.Commons.Utils.Exceptions;
using KaVE.RS.Commons.Analysis;
using KaVE.RS.Commons.Utils;
using KaVE.VS.FeedbackGenerator.CodeCompletion;
using KaVE.VS.FeedbackGenerator.MessageBus;

namespace KaVE.VS.FeedbackGenerator.Generators.ReSharper
{
    [Language(typeof (CSharpLanguage))]
    internal class CodeCompletionEventGeneratorRegistration
    {
        public CodeCompletionEventGeneratorRegistration(CodeCompletionLifecycleManager manager,
            CodeCompletionEventHandler handler)
        {
            manager.OnTriggered += handler.HandleTriggered;
            manager.DisplayedItemsUpdated += handler.HandleDisplayedItemsChanged;
            manager.OnSelectionChanged += handler.HandleSelectionChanged;
            manager.OnPrefixChanged += handler.HandlePrefixChanged;
            manager.OnClosed += handler.HandleClosed;
            manager.OnApplied += handler.HandleApplied;
            manager.OnCancelled += handler.HandleCancelled;
        }
    }

    [Language(typeof (CSharpLanguage))]
    public class CodeCompletionContextAnalysisTrigger : CSharpItemsProviderBase<CSharpCodeCompletionContext>
    {
        public const int LimitInMs = 1000;

        private readonly CodeCompletionEventHandler _handler;
        private readonly ILogger _logger;

        private readonly KaVECancellationTokenSource _tokenSource;

        public CodeCompletionContextAnalysisTrigger(CodeCompletionEventHandler handler, ILogger logger)
        {
            _handler = handler;
            _logger = logger;
            _tokenSource = new KaVECancellationTokenSource();
        }

        protected override bool IsAvailable(CSharpCodeCompletionContext context)
        {
            return true;
        }

        protected override bool AddLookupItems(CSharpCodeCompletionContext context, GroupedItemsCollector collector)
        {
            var token = _tokenSource.CancelAndCreate();

            Func<Context> analysis = () =>
            {
                Context result = null;
                ReadLockCookie.Execute(() => result = ContextAnalysis.Analyze(context, _logger).Context);
                return result;
            };

            TimeLimitRunner.Run(
                analysis,
                LimitInMs,
                token,
                OnSuccess,
                OnTimeout,
                OnError);

            return false;
        }

        private void OnSuccess(Context ctx)
        {
            _handler.SetContext(ctx);
        }

        private void OnTimeout()
        {
            _logger.Error("timeout! analysis did not finish within {0}ms", LimitInMs);
        }

        private void OnError(Exception e)
        {
            _logger.Error(e, "analysis error!");
        }
    }

    [ShellComponent]
    public class CodeCompletionEventHandler : EventGeneratorBase
    {
        private CompletionEvent _event;
        private Context _context;

        public CodeCompletionEventHandler(IRSEnv env, IMessageBus messageBus, IDateUtils dateUtils)
            : base(env, messageBus, dateUtils)
        {
            _context = new Context();
        }

        public void SetContext(Context context)
        {
            _context = context;
        }

        public void HandleTriggered(string prefix, IEnumerable<ILookupItem> displayedItems)
        {
            _event = Create<CompletionEvent>();
            _event.Context2 = _context;
            _event.Prefix = prefix;
            HandleDisplayedItemsChanged(displayedItems);
        }

        public void HandleDisplayedItemsChanged(IEnumerable<ILookupItem> displayedItems)
        {
            _event.ProposalCollection = displayedItems.ToProposalCollection();
        }

        public void HandleSelectionChanged(ILookupItem selectedItem)
        {
            _event.AddSelection(selectedItem.ToProposal());
        }

        public void HandlePrefixChanged(string newPrefix, IEnumerable<ILookupItem> displayedLookupItems)
        {
            _event.TerminatedState = TerminationState.Filtered;
            _event.TerminatedAt = DateTime.Now;
            _event.TerminatedBy = IDEEvent.Trigger.Automatic;
            var lastSelection = _event.Selections.LastOrDefault();
            Fire(_event);

            _event = Create<CompletionEvent>();
            _event.Context2 = _context;
            _event.Prefix = newPrefix;
            _event.ProposalCollection = displayedLookupItems.ToProposalCollection();
            if (lastSelection != null && _event.ProposalCollection.Proposals.Contains(lastSelection.Proposal))
            {
                _event.Selections.Add(lastSelection);
            }
            _event.TriggeredBy = IDEEvent.Trigger.Automatic;
        }

        public void HandleClosed()
        {
            _event.TerminatedAt = DateTime.Now;
        }

        public void HandleApplied(IDEEvent.Trigger trigger, ILookupItem appliedItem)
        {
            _event.TerminatedState = TerminationState.Applied;
            _event.TerminatedBy = trigger;
            Fire(_event);
        }

        public void HandleCancelled(IDEEvent.Trigger trigger)
        {
            _event.TerminatedState = TerminationState.Cancelled;
            _event.TerminatedBy = trigger;
            Fire(_event);
        }
    }
}