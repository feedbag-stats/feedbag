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
using EnvDTE;
using JetBrains.Application;
using JetBrains.Threading;
using KaVE.Commons.Model.Events.VisualStudio;
using KaVE.Commons.Utils;
using KaVE.Commons.Utils.Collections;
using KaVE.RS.Commons.Utils;
using KaVE.VS.Commons;
using KaVE.VS.Commons.Generators;
using KaVE.VS.Commons.Naming;

namespace KaVE.VS.FeedbackGenerator.Generators.VisualStudio
{
    [ShellComponent]
    public class WindowEventGenerator : EventGeneratorBase
    {
        private const int SignificantMoveDistanceLowerBound = 25;
        private const int WindowMoveTimeout = 200;

        private class WindowDescriptor
        {
            public int Top;
            public int Left;
            public int Width;
            public int Height;
        }

        private readonly ICallbackManager _callbackManager;
        private readonly IDateUtils _dateUtils;
        private readonly IDictionary<Window, WindowDescriptor> _knownWindows;
        private readonly IDictionary<Window, WindowEvent> _delayedMoveEvents;
        private readonly IDictionary<Window, ScheduledAction> _delayedMoveEventFireActions;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly WindowEvents _windowEvents;

        public WindowEventGenerator(IRSEnv env,
            IMessageBus messageBus,
            ICallbackManager callbackManager,
            IDateUtils dateUtils,
            IThreading threading)
            : base(env, messageBus, dateUtils, threading)
        {
            _callbackManager = callbackManager;
            _knownWindows = new Dictionary<Window, WindowDescriptor>();
            _delayedMoveEvents = new Dictionary<Window, WindowEvent>();
            _delayedMoveEventFireActions =
                new ThreadSafeDictionary<Window, ScheduledAction>(name => ScheduledAction.NoOp);
            _dateUtils = dateUtils;

            _windowEvents = DTE.Events.WindowEvents;
            _windowEvents.WindowCreated += OnWindowCreated;
            _windowEvents.WindowActivated += OnWindowActivated;
            _windowEvents.WindowMoved += OnWindowMoved;
            _windowEvents.WindowClosing += OnWindowClosed;
        }

        private void OnWindowCreated(Window window)
        {
            RememberWindow(window);
            Fire(window, WindowAction.Create);
        }

        private void OnWindowActivated(Window window, Window lostFocus)
        {
            RememberWindow(window);
            Fire(window, WindowAction.Activate);
            // We don't fire lostFocus events, since we track the active window in every event and know that the
            // previously active window looses the focus whenever some other window gains it.
        }

        private void OnWindowMoved(Window window, int top, int left, int width, int height)
        {
            if (IsUnknownWindow(window))
            {
                // If the window is unknown, we don't know how much its position or size changed. Therefore, we
                // ignore the current event, but remember the window for subsequent move events.
                RememberWindow(window);
            }
            else if (IsMovedSignificantly(window))
            {
                // Insignificant moves are frequently caused by rerendering. To avoid bloating the event stream, we
                // only handle significant moves here. Most real moves start with a number of insignificant events,
                // followed by a number of significant ones.
                UpdateAndScheduleEvent(window);
                RememberWindow(window);
            }
        }

        private bool IsUnknownWindow(Window window)
        {
            return !_knownWindows.ContainsKey(window);
        }

        private void RememberWindow(Window window)
        {
            _knownWindows[window] = CreateDescriptor(window);
        }

        private static WindowDescriptor CreateDescriptor(Window window)
        {
            return new WindowDescriptor
            {
                Top = window.Top,
                Left = window.Left,
                Width = window.Width,
                Height = window.Height
            };
        }

        private bool IsMovedSignificantly(Window window)
        {
            var knownWindow = _knownWindows[window];
            var diffHeight = Math.Abs(knownWindow.Height - window.Height);
            var diffWidth = Math.Abs(knownWindow.Width - window.Width);
            var downwards = Math.Abs(knownWindow.Top - window.Top);
            var leftwards = Math.Abs(knownWindow.Left - window.Left);
            return downwards > SignificantMoveDistanceLowerBound || leftwards > SignificantMoveDistanceLowerBound ||
                   diffHeight > SignificantMoveDistanceLowerBound || diffWidth > SignificantMoveDistanceLowerBound;
        }

        private void UpdateAndScheduleEvent(Window window)
        {
            _delayedMoveEventFireActions[window].Cancel();
            lock (_delayedMoveEvents)
            {
                if (!_delayedMoveEvents.ContainsKey(window))
                {
                    _delayedMoveEvents[window] = CreateWindowEvent(window, WindowAction.Move);
                }
                _delayedMoveEvents[window].TerminatedAt = _dateUtils.Now;
                _delayedMoveEventFireActions[window] = _callbackManager.RegisterCallback(
                    () => FireDelayedMoveEvent(window),
                    WindowMoveTimeout);
            }
        }

        private void FireDelayedMoveEvent(Window window)
        {
            lock (_delayedMoveEvents)
            {
                Fire(_delayedMoveEvents[window]);
                _delayedMoveEvents.Remove(window);
            }
        }

        private void OnWindowClosed(Window window)
        {
            Fire(window, WindowAction.Close);
        }

        private void Fire(Window window, WindowAction action)
        {
            var windowEvent = CreateWindowEvent(window, action);
            FireNow(windowEvent);
        }

        private WindowEvent CreateWindowEvent(Window window, WindowAction action)
        {
            var windowEvent = Create<WindowEvent>();
            windowEvent.Window = window.GetName();
            windowEvent.Action = action;
            return windowEvent;
        }
    }
}