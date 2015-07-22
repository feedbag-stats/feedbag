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

using KaVE.VS.Achievements.Achievements.BaseClasses.AchievementTypes;
using KaVE.VS.Achievements.Achievements.Calculators;
using KaVE.VS.Achievements.Statistics.Statistics;
using KaVE.VS.Achievements.Tests.Statistics.Calculators;
using Moq;
using NUnit.Framework;

namespace KaVE.VS.Achievements.Tests.Achievements.AchievementCalculators
{
    [TestFixture]
    internal class InitialAchievementCalculatorTest : CalculatorTestBase
    {
        private InitialAchievementCalculator _uut;

        public InitialAchievementCalculatorTest() : base(InitialAchievementCalculator.Id) {}

        [SetUp]
        public void Init()
        {
            _uut = new InitialAchievementCalculator(
                AchievementListingMock.Object,
                StatisticListingMock.Object,
                ObservableMock.Object);
        }

        [Test]
        public void DoesNotReset()
        {
            var testStatistic = new StatisticCalculatorTest.TestStatistic();

            _uut.OnNext(testStatistic);

            _uut.ResetAchievement();

            VerifyCompleted();
        }

        [Test]
        public void IgnoresOtherStatistics()
        {
            _uut.OnNext(new Mock<IStatistic>().Object);

            StatisticListingMock.Verify(l => l.Update(It.IsAny<IStatistic>()), Times.Never);
        }

        [Test]
        public void InitializeTest()
        {
            AchievementListingMock.Verify(
                l =>
                    l.Update(
                        It.Is<BaseAchievement>(
                            actualAchievement =>
                                actualAchievement.Id == AchievementId &&
                                !actualAchievement.IsCompleted)));
        }

        [Test]
        public void SetsCompletedCorrectly()
        {
            var testStatistic = new StatisticCalculatorTest.TestStatistic();

            _uut.OnNext(testStatistic);

            VerifyCompleted();
        }
    }
}