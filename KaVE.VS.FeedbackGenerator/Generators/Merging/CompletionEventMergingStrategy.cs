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
using System.Linq;
using KaVE.Commons.Model.Events;
using KaVE.Commons.Model.Events.CompletionEvents;
using KaVE.Commons.Utils.Collections;

namespace KaVE.VS.FeedbackGenerator.Generators.Merging
{
    /// <summary>
    ///     Merges intermediate filtering events with the subsequent completion event. An intermediate filtering event, is
    ///     a completion event that was triggered automatically (due to a filtering), recorded no interactions of the user,
    ///     and was terminated by another filtering. Such event are fired, when multiple characters are typed directly
    ///     after oneanother. For example, if the user types 'get', three events are fired for 'g', 'e', and 't'. The
    ///     strategy merges them down to one event.
    /// </summary>
    /// TODO strategy currently erases information: when the user types something and then deletes (part of) it, only the final prefix is kept.
    public class CompletionEventMergingStrategy : IEventMergeStrategy
    {
        public bool AreMergable(IDEEvent @event, IDEEvent subsequentEvent)
        {
            var ce1 = @event as CompletionEvent;
            if (ce1 == null)
            {
                return false;
            }
            if (!(subsequentEvent is CompletionEvent))
            {
                return false;
            }
            return IsMergable(ce1);
        }

        private static bool IsMergable(CompletionEvent @event)
        {
            var eventIsIntermediateFilterEvent = (@event.TriggeredBy == EventTrigger.Automatic) &&
                                                 (@event.TerminatedState == TerminationState.Filtered);
            // If there is no selection, we know that there was no interaction. If there is one selection, it may be
            // the previous selection or an initial selection (if there was no selection before). We cannot distinguish
            // this here. However, since we never merge the initial event (triggeredBy != Automatic), we know whether
            // there was an inital selection or not. Therefore, we can deduce that a selection was made during or after
            // the filtering.
            var eventContainsNoInteractions = (@event.Selections.Count <= 1);
            return eventIsIntermediateFilterEvent && eventContainsNoInteractions;
        }

        public IDEEvent Merge(IDEEvent @event, IDEEvent subsequentEvent)
        {
            var evt1 = (CompletionEvent) @event;
            var evt2 = (CompletionEvent) subsequentEvent;

            return new CompletionEvent
            {
                // properties that could be taken from both events (as their values are equal)
                IDESessionUUID = evt2.IDESessionUUID,
                ActiveDocument = evt2.ActiveDocument,
                ActiveWindow = evt2.ActiveWindow,
                // properties that need be taken from first event
                TriggeredBy = evt1.TriggeredBy,
                TriggeredAt = evt1.TriggeredAt,
                // properties that need be taken from later event
                Context2 = evt2.Context2,
                ProposalCollection = evt2.ProposalCollection,
                Selections = GetRebasedSelections(evt2, evt1.TriggeredAt, evt2.TriggeredAt),
                TerminatedState = evt2.TerminatedState,
                TerminatedAt = evt2.TerminatedAt,
                TerminatedBy = evt2.TerminatedBy
            };
        }

        private static IKaVEList<IProposalSelection> GetRebasedSelections(ICompletionEvent evt2,
            DateTimeOffset? oldBaseTime,
            DateTimeOffset? newBaseTime)
        {
            var rebaseOffset = newBaseTime - oldBaseTime;
            return Lists.NewListFrom<IProposalSelection>(
                evt2.Selections.Select(
                    selection => new ProposalSelection
                    {
                        Proposal = selection.Proposal,
                        SelectedAfter = selection.SelectedAfter + rebaseOffset
                    }));
        }
    }
}