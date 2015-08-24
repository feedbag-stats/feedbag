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
using KaVE.Commons.Utils;
using KaVE.VS.FeedbackGenerator.MessageBus;

namespace KaVE.VS.FeedbackGenerator.Generators.ReSharper
{
    public class EventGeneratingActionWrapper : CommandEventGeneratorBase
    {
        private readonly Action _originalAction;

        public EventGeneratingActionWrapper(Action originalAction,
            IRSEnv env,
            IMessageBus messageBus,
            IDateUtils dateUtils)
            : base(env, messageBus, dateUtils)
        {
            _originalAction = originalAction;
        }

        public void Execute()
        {
            var commandId = _originalAction.Target.ToString();
            FireActionEvent(commandId);
            InvokeOriginalCommand();
        }

        protected void InvokeOriginalCommand()
        {
            _originalAction.Invoke();
        }
    }
}