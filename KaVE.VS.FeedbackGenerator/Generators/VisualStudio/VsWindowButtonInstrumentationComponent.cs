/*
 * Copyright 2014 Technische Universit�t Darmstadt
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
using EnvDTE;
using JetBrains.Application;
using JetBrains.Threading;
using KaVE.Commons.Utils;
using KaVE.Commons.Utils.Exceptions;
using KaVE.JetBrains.Annotations;
using KaVE.VS.Commons;
using KaVE.VS.Commons.Generators;

namespace KaVE.VS.FeedbackGenerator.Generators.VisualStudio
{
    [ShellComponent]
    internal class VsWindowButtonInstrumentationComponent
    {
        private static readonly IDictionary<Window, VsWindowButtonClickEventGenerator> WindowRegistry =
            new Dictionary<Window, VsWindowButtonClickEventGenerator>();

        private readonly IRSEnv _env;
        private readonly IMessageBus _messageBus;
        private readonly IDateUtils _dateUtils;
        private readonly ILogger _logger;
        private readonly IThreading _threading;

        [UsedImplicitly]
        private WindowEvents _windowEvents;

        public VsWindowButtonInstrumentationComponent([NotNull] IRSEnv env,
            [NotNull] IMessageBus messageBus,
            [NotNull] IDateUtils dateUtils,
            [NotNull] ILogger logger,
            [NotNull] IThreading threading)
        {
            _env = env;
            _messageBus = messageBus;
            _dateUtils = dateUtils;
            _logger = logger;
            _threading = threading;
            Initialize(env);
        }

        private void Initialize(IRSEnv env)
        {
            var dte = env.IDESession.DTE;
            if (dte.ActiveWindow != null)
            {
                OnWindowActivated(dte.ActiveWindow, null);
            }
            _windowEvents = dte.Events.WindowEvents;
            _windowEvents.WindowActivated += OnWindowActivated;
        }

        private void OnWindowActivated(Window focusedWindow, Window lostfocus)
        {
            try
            {
                VsWindowButtonClickEventGenerator listener;
                if (!WindowRegistry.TryGetValue(focusedWindow, out listener))
                {
                    listener = new VsWindowButtonClickEventGenerator(
                        focusedWindow,
                        _env,
                        _messageBus,
                        _dateUtils,
                        _logger,
                        _threading);
                    WindowRegistry[focusedWindow] = listener;
                }
                listener.WindowChanged();
            }
            catch (Exception e)
            {
                _logger.Error(e, "instrumenting buttons in window failed");
            }
        }
    }
}