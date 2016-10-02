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
using KaVE.Commons.Model.Events;
using KaVE.Commons.Model.Events.CompletionEvents;
using KaVE.Commons.Model.Events.TestRunEvents;
using KaVE.Commons.Model.Events.UserProfiles;

namespace KaVE.FeedbackProcessor.StatisticsUltimate
{
    public class StatisticsExtractor
    {
        public UserStatistics CreateStatistics(IEnumerable<IDEEvent> events)
        {
            var stats = new UserStatistics();
            var days = new HashSet<DateTime>();
            var months = new HashSet<DateTime>();

            foreach (var e in events)
            {
                stats.NumEvents++;

                if (!e.TriggeredAt.HasValue)
                {
                    e.TriggeredAt = DateTime.MinValue;
                }

                var date = e.TriggeredAt.Value.Date;

                days.Add(date);
                var firstDayOfMonth = date.AddDays(-(date.Day - 1));
                months.Add(firstDayOfMonth);

                if (stats.DayFirst == DateTime.MinValue || date < stats.DayFirst)
                {
                    stats.DayFirst = date;
                }

                if (stats.DayLast == DateTime.MinValue || date > stats.DayLast)
                {
                    stats.DayLast = date;
                }

                var upe = e as UserProfileEvent;
                if (upe != null)
                {
                    if (upe.Education != Educations.Unknown)
                    {
                        stats.Education = upe.Education;
                    }
                    if (upe.Position != Positions.Unknown)
                    {
                        stats.Position = upe.Position;
                    }
                }

                var cce = e as CompletionEvent;
                if (cce != null)
                {
                    stats.NumCodeCompletion++;
                }

                var tre = e as TestRunEvent;
                if (tre != null)
                {
                    stats.NumTestRuns++;
                }
            }

            stats.NumDays = days.Count;
            stats.NumMonth = months.Count;
            return stats;
        }
    }
}