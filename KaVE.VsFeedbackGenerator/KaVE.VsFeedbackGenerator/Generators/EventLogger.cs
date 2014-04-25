﻿using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using KaVE.Model.Events;
using KaVE.Model.Events.VisualStudio;
using KaVE.VsFeedbackGenerator.Generators.Merging;
using KaVE.VsFeedbackGenerator.MessageBus;
using KaVE.VsFeedbackGenerator.Utils.Json;
using KaVE.VsFeedbackGenerator.Utils.Logging;

namespace KaVE.VsFeedbackGenerator.Generators
{
    [ShellComponent]
    internal class EventLogger
    {
        private static readonly IList<IEventMergeStrategy> MergeStrategies = new List<IEventMergeStrategy>
        {
            new CompletionEventMergingStrategy()
        };

        private readonly IMessageBus _messageChannel;
        private readonly ILogManager<IDEEvent> _logManager;

        private IDEEvent _lastEvent;

        public EventLogger(IMessageBus messageBus, IDEEventLogFileManager logManager)
        {
            _messageChannel = messageBus;
            _logManager = logManager;
            _messageChannel.Subscribe<IDEEvent>(ProcessEvent);
        }

        private void ProcessEvent(IDEEvent @event)
        {
            lock (_messageChannel)
            {
                if (IsIDEShutdownEvent(@event))
                {
                    LogEvent(_lastEvent);
                    LogEvent(@event);
                    return;
                }

                ProcessNormalEvent(@event);
            }
        }

        private static bool IsIDEShutdownEvent(IDEEvent @event)
        {
            var ideStateEvent = @event as IDEStateEvent;
            return ideStateEvent != null && ideStateEvent.IDELifecyclePhase == IDEStateEvent.LifecyclePhase.Shutdown;
        }

        private void ProcessNormalEvent(IDEEvent @event)
        {
            if (_lastEvent == null)
            {
                _lastEvent = @event;
            }
            else
            {
                // TODO are there any merging strategies? if not, this logic should be deleted!
                var merger = MergeStrategies.FirstOrDefault(strategy => strategy.AreMergable(_lastEvent, @event));
                if (merger != null)
                {
                    _lastEvent = merger.Merge(_lastEvent, @event);
                    return;
                }
                LogEvent(_lastEvent);
                _lastEvent = @event;
            }
        }

        private void LogEvent(IDEEvent @event)
        {
            if (@event == null)
            {
                return;
            }
            var log = _logManager.TodaysLog;
            using (var logWriter = log.NewLogWriter())
            {
                logWriter.Write(@event);
            }
        }
    }
}