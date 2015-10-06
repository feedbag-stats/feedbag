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

using KaVE.FeedbackProcessor.Activities.Intervals;
using KaVE.FeedbackProcessor.Activities.Model;
using KaVE.FeedbackProcessor.Model;

namespace KaVE.FeedbackProcessor.VsWindows
{
    internal class WindowIntervalProcessor : IntervalProcessor<string>
    {
        public const string OutsideIDEIntervalId = ":outside of ide:";

        protected override void HandleWithInterval(ActivityEvent @event)
        {
            if (@event.Activity == Activity.EnterIDE)
            {
                if (CurrentInterval.Id.Equals(OutsideIDEIntervalId))
                {
                    CurrentInterval.End = @event.GetTriggeredAt();
                }
                else
                {
                    StartInterval(CurrentInterval.End, OutsideIDEIntervalId, @event.GetTriggeredAt());
                }
            }

            var previousInterval = CurrentInterval;

            if (!previousInterval.Id.Equals(GetIntervalId(@event)))
            {
                StartInterval(@event);
                previousInterval.End = CurrentInterval.Start;
            }
            else
            {
                previousInterval.End = GetEnd(@event);
            }
        }

        protected override string GetIntervalId(ActivityEvent @event)
        {
            if (@event.Activity == Activity.LeaveIDE)
            {
                return OutsideIDEIntervalId;
            }
            return @event.ActiveWindow.Identifier;
        }
    }
}