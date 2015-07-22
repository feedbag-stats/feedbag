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
using System.ComponentModel;
using System.Numerics;
using KaVE.Commons.Model.Events;
using KaVE.Commons.Model.Events.VisualStudio;
using KaVE.VS.Achievements.LogCollector;
using KaVE.VS.Achievements.Statistics.Calculators;
using KaVE.VS.Achievements.Statistics.Calculators.BaseClasses;
using KaVE.VS.Achievements.Statistics.Listing;
using KaVE.VS.Achievements.Statistics.Statistics;
using KaVE.VS.Achievements.UI;
using KaVE.VS.Achievements.Util;
using KaVE.VS.FeedbackGenerator.MessageBus;
using KaVE.VS.FeedbackGenerator.Utils.Logging;
using Moq;
using NUnit.Framework;

namespace KaVE.VS.Achievements.Tests.LogCollector
{
    [TestFixture]
    public class LogReplayTest
    {
        [SetUp]
        public void SetUp()
        {
            // Initialize Mocks
            var messageBusMock = new Mock<IMessageBus>();
            var errorHandlerMock = new Mock<IErrorHandler>();
            _statisticListingMock = new Mock<IStatisticListing>();
            _logManagerMock = new Mock<ILogManager>();
            var uiDelegatorMock = new Mock<IUiDelegator>();
            SetupLogManagerMock();

            SetupStatisticListingMock();

            // Initialize Calculators
            _calculatorList = new List<StatisticCalculator>
            {
                new BuildCalculator(_statisticListingMock.Object, messageBusMock.Object, errorHandlerMock.Object),
                new GlobalCalculator(_statisticListingMock.Object, messageBusMock.Object, errorHandlerMock.Object),
                new CommandCalculator(_statisticListingMock.Object, messageBusMock.Object, errorHandlerMock.Object)
            };
            Registry.RegisterComponents(_calculatorList);
            Registry.RegisterComponent(uiDelegatorMock.Object);

            _uut = new LogReplay(_logManagerMock.Object, _statisticListingMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            Registry.Clear();
        }

        private LogReplay _uut;
        private Mock<IStatisticListing> _statisticListingMock;
        private Mock<ILogManager> _logManagerMock;

        private List<StatisticCalculator> _calculatorList;

        private BuildStatistic _buildStatistic;
        private CommandStatistic _commandStatistic;
        private GlobalStatistic _globalStatistic;

        private void SetupStatisticListingMock()
        {
            _buildStatistic = new BuildStatistic();
            _commandStatistic = new CommandStatistic();
            _globalStatistic = new GlobalStatistic();

            _statisticListingMock.Setup(listing => listing.GetStatistic(typeof (BuildStatistic)))
                                 .Returns(_buildStatistic);
            _statisticListingMock.Setup(listing => listing.GetStatistic(typeof (CommandStatistic)))
                                 .Returns(_commandStatistic);
            _statisticListingMock.Setup(listing => listing.GetStatistic(typeof (GlobalStatistic)))
                                 .Returns(_globalStatistic);
        }

        private static readonly CommandEvent[] CommandEvents =
        {
            new CommandEvent {CommandId = "File.SaveSelectedItems"},
            new CommandEvent {CommandId = "View.CallHierarchy"},
            new CommandEvent {CommandId = "Edit.LineDelete"},
            new CommandEvent {CommandId = "Rename"},
            new CommandEvent {CommandId = "Edit.LineEnd"},
            new CommandEvent {CommandId = "Edit.FormatSelection"},
            new CommandEvent {CommandId = "File.OpenFile"},
            new CommandEvent {CommandId = "Edit.Paste"},
            new CommandEvent {CommandId = "Tools.Options"},
            new CommandEvent {CommandId = "Edit.Undo"},
            new CommandEvent {CommandId = "Edit.LineEndExtend"},
            new CommandEvent {CommandId = "View.ShowSmartTag"},
            new CommandEvent {CommandId = "View.ObjectBrowser"},
            new CommandEvent {CommandId = "View.SolutionExplorer"},
            new CommandEvent {CommandId = "View.TfsTeamExplorer"},
            new CommandEvent {CommandId = "View.ClassView"},
            new CommandEvent {CommandId = "View.Output"},
            new CommandEvent {CommandId = "Edit.Copy"},
            new CommandEvent {CommandId = "Edit.LineStart"},
            new CommandEvent {CommandId = "File.Exit"},
            new CommandEvent {CommandId = "Edit.WordPrevious"},
            new CommandEvent {CommandId = "Edit.WordNext"}
        };

        private static readonly BuildTarget SuccessfulBuildTarget = new BuildTarget {Successful = true};
        private static readonly BuildTarget FailedBuildTarget = new BuildTarget {Successful = false};
        private static readonly List<BuildTarget> SuccessfulTargetList = new List<BuildTarget> {SuccessfulBuildTarget};
        private static readonly List<BuildTarget> FailedTargetList = new List<BuildTarget> {FailedBuildTarget};

        private static readonly BuildEvent[] BuildEvents =
        {
            new BuildEvent {Targets = FailedTargetList},
            new BuildEvent {Targets = SuccessfulTargetList},
            new BuildEvent {Targets = FailedTargetList},
            new BuildEvent {Targets = FailedTargetList}
        };

        private void SetupLogManagerMock()
        {
            var commandEventList = CommandEvents;
            var commandEventLog = new Mock<ILog>();
            commandEventLog.Setup(log => log.ReadAll()).Returns(commandEventList);

            var buildEventList = BuildEvents;
            var buildEventLog = new Mock<ILog>();
            buildEventLog.Setup(log => log.ReadAll()).Returns(buildEventList);

            var logsList = new List<ILog>
            {
                commandEventLog.Object,
                buildEventLog.Object
            };

            _logManagerMock.Setup(logManager => logManager.Logs).Returns(logsList);
        }

        private void VerifyThatCalculatorsProcessedCorrectly()
        {
            VerifyBuildStatistic();
            VerifyCommandStatistic();
            VerifyGlobalStatistic();
        }

        private void VerifyGlobalStatistic()
        {
            Assert.AreEqual(BigInteger.Parse("26"), _globalStatistic.TotalEvents);
        }

        private void VerifyCommandStatistic()
        {
            var commandIdList = new List<string>
            {
                "File.SaveSelectedItems",
                "View.CallHierarchy",
                "Edit.LineDelete",
                "Rename",
                "Edit.LineEnd",
                "Edit.FormatSelection",
                "File.OpenFile",
                "Edit.Paste",
                "Tools.Options",
                "Edit.Undo",
                "Edit.LineEndExtend",
                "View.ShowSmartTag",
                "View.ObjectBrowser",
                "View.SolutionExplorer",
                "View.TfsTeamExplorer",
                "View.ClassView",
                "View.Output",
                "Edit.Copy",
                "Edit.LineStart",
                "File.Exit",
                "Edit.WordPrevious",
                "Edit.WordNext"
            };
            foreach (var commandId in commandIdList)
            {
                var commandTypeValues = _commandStatistic.CommandTypeValues;
                Assert.True(commandTypeValues.ContainsKey(commandId));
                Assert.AreEqual(1, commandTypeValues[commandId]);
            }
        }

        private void VerifyBuildStatistic()
        {
            Assert.AreEqual(4, _buildStatistic.TotalBuilds);
            Assert.AreEqual(1, _buildStatistic.SuccessfulBuilds);
            Assert.AreEqual(3, _buildStatistic.FailedBuilds);
        }

        [Test]
        public void CollectEventsFromLogTest()
        {
            var backgroundWorker = new BackgroundWorker {WorkerReportsProgress = true};

            _uut.CollectEventsFromLog(backgroundWorker);

            VerifyThatCalculatorsProcessedCorrectly();

            _statisticListingMock.VerifySet(listing => listing.BlockUpdateToObservers = true);
            _statisticListingMock.VerifySet(listing => listing.BlockUpdateToObservers = false);
            _statisticListingMock.Verify(listing => listing.SendUpdateToObserversWithAllStatistics());
        }
    }
}