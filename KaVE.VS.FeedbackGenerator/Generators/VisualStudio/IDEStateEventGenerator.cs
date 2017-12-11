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

using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using JetBrains.Application;
using JetBrains.Application.Threading;
using JetBrains.DataFlow;
using KaVE.Commons.Model.Events.VisualStudio;
using KaVE.Commons.Utils;
using KaVE.VS.Commons;
using KaVE.VS.Commons.Generators;
using KaVE.VS.Commons.Naming;

namespace KaVE.VS.FeedbackGenerator.Generators.VisualStudio
{
    [ShellComponent]
    public class IDEStateEventGenerator : EventGeneratorBase
    {
        private readonly IRSEnv _env;
        private readonly IEventLogger _logger;

        public IDEStateEventGenerator(IRSEnv env,
            IMessageBus messageBus,
            Lifetime lifetime,
            IDateUtils dateUtils,
            IEventLogger logger,
            IThreading threading)
            : base(env, messageBus, dateUtils, threading)
        {
            _env = env;
            _logger = logger;
            FireIDEStateEvent(IDELifecyclePhase.Startup);
            lifetime.AddAction(FireShutdownEvent);
        }

        private void FireShutdownEvent()
        {
            var shutdownEvent = CreateIDEStateEvent(IDELifecyclePhase.Shutdown);
            shutdownEvent.IDESessionUUID = _env.IDESession.UUID;
            _logger.Shutdown(shutdownEvent);
        }

        private void FireIDEStateEvent(IDELifecyclePhase phase)
        {
            FireNow(CreateIDEStateEvent(phase));
        }

        private IDEStateEvent CreateIDEStateEvent(IDELifecyclePhase phase)
        {
            var ideStateEvent = Create<IDEStateEvent>();
            ideStateEvent.IDELifecyclePhase = phase;

            ideStateEvent.OpenWindows = GetVisibleWindows().GetNames();
            ideStateEvent.OpenDocuments = DTE.Documents.GetNames();
            return ideStateEvent;
        }

        private IEnumerable<Window> GetVisibleWindows()
        {
            return from Window window in DTE.Windows where window.Visible select window;
        }
    }
}