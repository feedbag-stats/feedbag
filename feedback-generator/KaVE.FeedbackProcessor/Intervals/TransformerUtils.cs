/*
 * Copyright 2014 Technische Universitšt Darmstadt
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
using KaVE.Commons.Model.Events;
using KaVE.FeedbackProcessor.Intervals.Model;

namespace KaVE.FeedbackProcessor.Intervals
{
    internal static class TransformerUtils
    {
        public static bool EventHasNoTimeData(IDEEvent ideEvent)
        {
            return !ideEvent.TriggeredAt.HasValue || !ideEvent.TerminatedAt.HasValue;
        }

        public static TIntervalType CreateIntervalFromFirstEvent<TIntervalType>(IDEEvent ideEvent)
            where TIntervalType : Interval, new()
        {
            return new TIntervalType
            {
                StartTime = ideEvent.TriggeredAt.GetValueOrDefault(),
                Duration = ideEvent.Duration.GetValueOrDefault()
            };
        }
    }
}