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

using JetBrains.Util;
using KaVE.Commons.Model.Events;
using KaVE.Commons.Model.Events.CompletionEvents;
using KaVE.Commons.Utils.Naming;

namespace KaVE.VS.FeedbackGenerator.SessionManager.Anonymize.CompletionEvents
{
    internal class CompletionEventAnonymizer : IDEEventAnonymizer
    {
        public override void AnonymizeDurations(IDEEvent ideEvent)
        {
            var completionEvent = (CompletionEvent) ideEvent;
            completionEvent.Selections.ForEach(selection => selection.SelectedAfter = null);
            base.AnonymizeDurations(ideEvent);
        }

        public override void AnonymizeCodeNames(IDEEvent ideEvent)
        {
            var completionEvent = (CompletionEvent) ideEvent;
            completionEvent.ProposalCollection.Proposals.ForEach(AnonymizeCodeNames);
            completionEvent.Selections.ForEach(AnonymizeCodeNames);
            completionEvent.Context2 = AnonymizeCodeNames(completionEvent.Context2);
            base.AnonymizeCodeNames(completionEvent);
        }

        private static void AnonymizeCodeNames(IProposalSelection selection)
        {
            AnonymizeCodeNames(selection.Proposal);
        }

        private static void AnonymizeCodeNames(IProposal proposal)
        {
            proposal.Name = proposal.Name.ToAnonymousName();
        }

        private static Context AnonymizeCodeNames(Context context)
        {
            return new Context
            {
                TypeShape = new TypeShapeAnonymizer().Anonymize(context.TypeShape),
                SST = CreateSSTAnonymizer().Anonymize(context.SST)
            };
        }

        private static SSTAnonymization CreateSSTAnonymizer()
        {
            var refAnon = new SSTReferenceAnonymization();
            var exprAnon = new SSTExpressionAnonymization(refAnon);
            var stmtAnon = new SSTStatementAnonymization(exprAnon, refAnon);
            return new SSTAnonymization(stmtAnon);
        }
    }
}