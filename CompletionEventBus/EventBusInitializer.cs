﻿using System.ComponentModel.Composition;
using System.IO;
using CodeCompletion.Model.CompletionEvent;
using CompletionEventSerializer;

namespace CompletionEventBus
{
    public class EventBusInitializer
    {
        // TODO ensure this is getting called at "the beginning"...
        [ImportingConstructor]
        public EventBusInitializer(IMessageChannel messageChannel, ISerializer serializer)
        {
            messageChannel.Subscribe<CompletionEvent>(ce =>
                {
                    // TODO where to write to?
                    using (var eventLog = File.Open("logFilePath", FileMode.Append))
                    {
                        serializer.AppendTo(eventLog, ce);
                    }
                });
        }
    }
}
