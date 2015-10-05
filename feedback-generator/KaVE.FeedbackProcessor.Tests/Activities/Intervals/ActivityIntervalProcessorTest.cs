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
using KaVE.FeedbackProcessor.Activities.Intervals;
using KaVE.FeedbackProcessor.Activities.Model;
using KaVE.FeedbackProcessor.Model;
using KaVE.FeedbackProcessor.Tests.Model;
using KaVE.FeedbackProcessor.Tests.TestUtils;
using NUnit.Framework;

namespace KaVE.FeedbackProcessor.Tests.Activities.Intervals
{
    [TestFixture]
    internal class ActivityIntervalProcessorTest
    {
        private ActivityIntervalProcessor _uut;
        private DateTime _someDateTime;
        private Developer _someDeveloper;

        [SetUp]
        public void SetUp()
        {
            _uut = new ActivityIntervalProcessor();
            _someDateTime = DateTimeFactory.SomeWorkingHoursDateTime();
            _someDeveloper = TestFactory.SomeDeveloper();
        }

        [Test]
        public void Interval()
        {
            WhenStreamIsProcessed(SomeEvent(0, Activity.Other, 3));

            AssertIntervals(Interval(0, Activity.Other, 3));
        }

        [Test]
        public void IntervalWithoutEnd()
        {
            WhenStreamIsProcessed(SomeEvent(0, Activity.Other, 0));

            AssertIntervals(Interval(0, Activity.Other, 0));
        }

        [Test]
        public void Intervals()
        {
            WhenStreamIsProcessed(
                SomeEvent(0, Activity.Other, 1),
                SomeEvent(3, Activity.Navigation, 2));

            AssertIntervals(
                Interval(0, Activity.Other, 1),
                Interval(3, Activity.Navigation, 2));
        }

        [Test]
        public void IntervalsOfSameActivity()
        {
            WhenStreamIsProcessed(
                SomeEvent(0, Activity.Other, 1),
                SomeEvent(3, Activity.Other, 2));

            AssertIntervals(
                Interval(0, Activity.Other, 1),
                Interval(3, Activity.Other, 2));
        }

        [Test, ExpectedException]
        public void ConcurrentIntervals()
        {
            WhenStreamIsProcessed(
                SomeEvent(0, Activity.Development, 3),
                SomeEvent(1, Activity.Navigation, 1));
        }

        [Test]
        public void ClosesIntervalGapsBelowTimeout()
        {
            WhenStreamIsProcessed(
                SomeEvent(0, Activity.Other, 1),
                SomeEvent(3, Activity.Navigation, 1));

            AssertCorrectedIntervals(
                TimeSpan.FromSeconds(3),
                TimeSpan.FromSeconds(42),
                Interval(0, Activity.Other, 3),
                Interval(3, Activity.Navigation, 4));
        }

        [Test]
        public void InsertsInactivityIfGapExceedsTimeout()
        {
            WhenStreamIsProcessed(
                SomeEvent(0, Activity.Other, 1),
                SomeEvent(3, Activity.Navigation, 1));

            AssertCorrectedIntervals(
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(42),
                Interval(0, Activity.Other, 2),
                Interval(2, Activity.Inactive, 1),
                Interval(3, Activity.Navigation, 2));
        }

        [Test]
        public void InsertsLongInactivityIfGapExceedsThreshold()
        {
            WhenStreamIsProcessed(
                SomeEvent(0, Activity.Other, 1),
                SomeEvent(5, Activity.Navigation, 1));

            AssertCorrectedIntervals(
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(2),
                Interval(0, Activity.Other, 2),
                Interval(2, Activity.InactiveLong, 3),
                Interval(5, Activity.Navigation, 2));
        }

        [Test]
        public void InsertsAwayFromLeaveIDEToNextActivity()
        {
            WhenStreamIsProcessed(
                SomeEvent(0, Activity.LeaveIDE, 1),
                SomeEvent(10, Activity.Development, 1));

            AssertIntervals(
                Interval(0, Activity.Away, 10),
                Interval(10, Activity.Development, 1));
        }

        [Test]
        public void InsertsAwayFromLastActivityUntilEnterIDE()
        {
            WhenStreamIsProcessed(
                SomeEvent(0, Activity.Development, 1),
                SomeEvent(6, Activity.EnterIDE, 1));

            AssertIntervals(
                Interval(0, Activity.Development, 1),
                Interval(1, Activity.Away, 5),
                Interval(6, Activity.Other, 1));
        }

        [Test]
        public void InsertsAwayBetweenEnterAndLeaveIDE()
        {
            WhenStreamIsProcessed(
                SomeEvent(0, Activity.LeaveIDE, 1),
                SomeEvent(10, Activity.EnterIDE, 1));

            AssertIntervals(
                Interval(0, Activity.Away, 10),
                Interval(10, Activity.Other, 1));
        }

        [Test]
        public void AnyActivityProlongsCurrentInterval()
        {
            WhenStreamIsProcessed(
                SomeEvent(0, Activity.Development, 1),
                SomeEvent(2, Activity.Any, 1));

            AssertIntervals(
                Interval(0, Activity.Development, 1),
                Interval(2, Activity.Development, 1));
        }

        [Test]
        public void AnyActivityStartsOtherInterval()
        {
            WhenStreamIsProcessed(
                SomeEvent(0, Activity.Any, 1));

            AssertIntervals(
                Interval(0, Activity.Other, 1));
        }

        [Test]
        public void AnyDoesNotShortenInterval()
        {
            WhenStreamIsProcessed(
                SomeEvent(0, Activity.Development, 3),
                SomeEvent(1, Activity.Any, 0));

            AssertIntervals(
                Interval(0, Activity.Development, 3));
        }

        [Test]
        public void IgnoresAnyIfConcurrentToSpecificActivity()
        {
            WhenStreamIsProcessed(
                SomeEvent(0, Activity.Any, 0),
                SomeEvent(0, Activity.Development, 1));

            AssertIntervals(
                Interval(0, Activity.Development, 1));
        }

        [Test]
        public void EnsuresSequentialIntervals()
        {
            WhenStreamIsProcessed(
                SomeEvent(0, Activity.Waiting, 5),
                SomeEvent(3, Activity.Development, 1));

            AssertIntervals(
                Interval(0, Activity.Waiting, 3),
                Interval(3, Activity.Development, 1),
                Interval(4, Activity.Waiting, 1));
        }

        private void WhenStreamIsProcessed(params ActivityEvent[] stream)
        {
            _uut.OnStreamStarts(_someDeveloper);
            foreach (var @event in stream)
            {
                _uut.OnEvent(@event);
            }
            _uut.OnStreamEnds();
        }

        private void AssertIntervals(params ActivityIntervalProcessor.Interval[] expecteds)
        {
            var actuals = _uut.Intervals[_someDeveloper];
            Assert.AreEqual(expecteds, actuals);
        }

        private void AssertCorrectedIntervals(TimeSpan activityTimeout,
            TimeSpan shortInactivityTimeout,
            params ActivityIntervalProcessor.Interval[] expecteds)
        {
            var correctedIntervals = _uut.GetIntervalsWithCorrectTimeouts(activityTimeout, shortInactivityTimeout);
            var actuals = correctedIntervals[_someDeveloper];
            Assert.AreEqual(expecteds, actuals);
        }

        private ActivityIntervalProcessor.Interval Interval(int startOffsetInSeconds,
            Activity activity,
            int durationInSeconds)
        {
            var start = _someDateTime.AddSeconds(startOffsetInSeconds);
            return new ActivityIntervalProcessor.Interval
            {
                Start = start,
                Activity = activity,
                End = start + TimeSpan.FromSeconds(durationInSeconds)
            };
        }

        private ActivityEvent SomeEvent(int triggerOffsetInSeconds, Activity activity, int durationInSeconds)
        {
            return new ActivityEvent
            {
                TriggeredAt = _someDateTime.AddSeconds(triggerOffsetInSeconds),
                Activity = activity,
                Duration = TimeSpan.FromSeconds(durationInSeconds)
            };
        }
    }
}