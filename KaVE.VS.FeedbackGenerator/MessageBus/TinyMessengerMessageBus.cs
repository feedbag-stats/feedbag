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
using JetBrains.Application;
using KaVE.VS.Commons;

namespace KaVE.VS.FeedbackGenerator.MessageBus
{
    [ShellComponent]
    public class TinyMessengerMessageBus : IMessageBus
    {
        private readonly ITinyMessengerHub _hub = new TinyMessengerHub();

        public void Publish<TMessage>(TMessage evt) where TMessage : class
        {
            _hub.PublishAsync(new GenericTinyMessage<TMessage>(this, evt));
        }

        public void Subscribe<TMessage>(Action<TMessage> action, Func<TMessage, bool> filter)
            where TMessage : class
        {
            Action<GenericTinyMessage<TMessage>> delegateAction = m => action.Invoke(m.Content);

            if (filter == null)
            {
                Subscribe(delegateAction);
            }
            else
            {
                SubscribeWithFilter(delegateAction, m => filter.Invoke(m.Content));
            }
        }

        private void Subscribe<TMessage>(Action<TMessage> action) where TMessage : class, ITinyMessage
        {
            _hub.Subscribe(action, true);
        }

        private void SubscribeWithFilter<TMessage>(Action<TMessage> action, Func<TMessage, bool> filter)
            where TMessage : class, ITinyMessage
        {
            _hub.Subscribe(action, filter, true);
        }
    }
}
