﻿using System;
using KaVE.CompletionTraceGenerator.Model;
using KaVE.JetBrains.Annotations;
using KaVE.Model.Events;
using KaVE.Model.Events.CompletionEvent;
using KaVE.VsFeedbackGenerator.Utils;
using KaVE.VsFeedbackGenerator.Utils.Logging;

namespace KaVE.CompletionTraceGenerator
{
    public class CompletionEventToTraceConverter
    {
        // TODO Writer is never disposed, potentially causes loss of information
        private readonly ILogWriter<CompletionTrace> _writer;
        private CompletionTrace _trace;

        public CompletionEventToTraceConverter(ILogWriter<CompletionTrace> writer)
        {
            _writer = writer;
        }

        public void Process([NotNull] CompletionEvent completionEvent)
        {
            var trace = GetNewTraceOrFilterContinuation(completionEvent);
            trace.DurationInMillis += completionEvent.ComputeDuration();
            trace.AppendSelectionChangeActions(completionEvent);

            // TODO how to extract this into separate methods with only one responsibility?
            switch (completionEvent.TerminatedAs)
            {
                case CompletionEvent.TerminationState.Applied:
                    trace.AppendAction(CompletionAction.NewApply());
                    _writer.Write(trace);
                    break;
                case CompletionEvent.TerminationState.Cancelled:
                    trace.AppendAction(CompletionAction.NewCancel());
                    _writer.Write(trace);
                    break;
                case CompletionEvent.TerminationState.Filtered:
                    // Filtering is not a termination of code completion. Moreover, we know the changed prefix
                    // only from the subsequent completion event. Therefore, no action is appended here.
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private CompletionTrace GetNewTraceOrFilterContinuation(CompletionEvent completionEvent)
        {
            if (_trace != null && completionEvent.IsContinuationAfterFiltering())
            {
                _trace.AppendAction(CompletionAction.NewFilter(completionEvent.Prefix));
            }
            else
            {
                _trace = new CompletionTrace {DurationInMillis = 0};
            }
            return _trace;
        }
    }

    static class CompletionEventToTraceConverterUtils
    {
        internal static bool IsContinuationAfterFiltering(this CompletionEvent completionEvent)
        {
            return completionEvent.TriggeredBy == IDEEvent.Trigger.Automatic;
        }

        internal static long ComputeDuration(this CompletionEvent completionEvent)
        {
            var duration = completionEvent.Duration.GetValueOrDefault(TimeSpan.FromSeconds(0));
            return (long)Math.Ceiling(duration.TotalMilliseconds);
        }

        internal static void AppendSelectionChangeActions([NotNull] this CompletionTrace trace, CompletionEvent completionEvent)
        {
            if (!completionEvent.HasSelections())
            {
                return;
            }

            var initialSelection = completionEvent.Selections[0];
            var oldPos = completionEvent.ProposalCollection.GetPosition(initialSelection.Proposal);

            foreach (var newSelection in completionEvent.Selections)
            {
                var newPos = completionEvent.ProposalCollection.GetPosition(newSelection.Proposal);
                var stepSize = Math.Abs(oldPos - newPos);

                if (stepSize == 1)
                {
                    var direction = (newPos > oldPos) ? Direction.Down : Direction.Up;
                    trace.AppendAction(CompletionAction.NewStep(direction));
                }
                else if (stepSize > 1)
                {
                    trace.AppendAction(CompletionAction.NewMouseGoto(newPos));
                }

                oldPos = newPos;
            }
        }

        private static bool HasSelections(this CompletionEvent completionEvent)
        {
            return completionEvent.Selections.Count > 1;
        }
    }
}