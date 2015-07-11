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
using KaVE.Commons.TestUtils.Utils;
using KaVE.FeedbackProcessor.Activities.Model;
using KaVE.FeedbackProcessor.Activities.SlidingWindow;
using NUnit.Framework;

namespace KaVE.FeedbackProcessor.Tests.Activities.SlidingWindow
{
    [TestFixture]
    internal class EvaluateActivityStreamTest
    {
        private static readonly TimeSpan WindowSpan = TimeSpan.FromSeconds(2);
        private static readonly TimeSpan SomeTimeSpan = TimeSpan.FromSeconds(5);

        [Test]
        public void CountsOccurrancesOfActivityTimesWindowSpan()
        {
            var uut = Stream(Activity.Development, Activity.Development, Activity.Development);

            var statistic = uut.Evaluate(TimeSpan.MaxValue);

            Assert.AreEqual(WindowSpan.Times(3), statistic[Activity.Development]);
        }

        [Test]
        public void CountsShortInactivePeriod()
        {
            var uut = Stream(Activity.Inactive, Activity.Inactive);

            var statistic = uut.Evaluate(WindowSpan.Times(3));

            Assert.AreEqual(WindowSpan.Times(2), statistic[Activity.Inactive]);
        }

        [Test(Description = "A Inactive window can only occur in between other windows (see WindowComputationTest)")]
        public void CountsLongInactiveIfInactivePeriodExceedsThreshold()
        {
            var uut = Stream(Activity.Any, Activity.Inactive, Activity.Inactive, Activity.Inactive, Activity.Development);

            var statistic = uut.Evaluate(WindowSpan.Times(2));

            Assert.AreEqual(WindowSpan.Times(3), statistic[Activity.InactiveLong]);
            Assert.AreEqual(TimeSpan.Zero, statistic[Activity.Inactive]);
        }

        [Test]
        public void CountsInactiveIfIndividualPeriodsAreShorterThanThreshold()
        {
            var uut = Stream(Activity.Inactive, Activity.Inactive, Activity.Development, Activity.Inactive, Activity.Inactive);

            var statistic = uut.Evaluate(WindowSpan.Times(3));

            Assert.AreEqual(WindowSpan.Times(4), statistic[Activity.Inactive]);
        }

        private static ActivityStream Stream(params Activity[] activities)
        {
            var activityStream = new ActivityStream(WindowSpan);
            activityStream.AddAll(activities);
            return activityStream;
        }
    }
}