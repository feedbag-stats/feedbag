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
 * 
 * Contributors:
 *    - Sebastian Proksch
 */

using KaVE.Commons.Model.Events;
using KaVE.Commons.Model.Events.VisualStudio;
using KaVE.Commons.Utils.Collections;
using KaVE.FeedbackProcessor.Activities.Model;
using KaVE.FeedbackProcessor.Cleanup.Processors;

namespace KaVE.FeedbackProcessor.Activities
{
    internal class IDEStateEventProcessor : BaseProcessor
    {
        public IDEStateEventProcessor()
        {
            RegisterFor<IDEStateEvent>(ProcessIDEStateEvent);
        }

        private IKaVESet<IDEEvent> ProcessIDEStateEvent(IDEStateEvent @event)
        {
            if (IsIntermediateRuntimeEvent(@event))
            {
                return AnswerDrop();
            }

            const Activity activity = Activity.InIDE;
            var phase = IsStartup(@event) ? ActivityPhase.Start : ActivityPhase.End;

            return AnswerActivity(@event, activity, phase);
        }

        private static bool IsIntermediateRuntimeEvent(IDEStateEvent @event)
        {
            return @event.IDELifecyclePhase == IDEStateEvent.LifecyclePhase.Runtime;
        }

        private static bool IsStartup(IDEStateEvent @event)
        {
            return @event.IDELifecyclePhase == IDEStateEvent.LifecyclePhase.Startup;
        }
    }
}