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
using KaVE.Commons.Model.Events.TestRunEvents;
using KaVE.FeedbackProcessor.Intervals.Exporter;
using KaVE.FeedbackProcessor.Intervals.Model;
using NUnit.Framework;

namespace KaVE.FeedbackProcessor.Tests.Intervals.Exporter
{
    /// <summary>
    ///     this test class is disabled by default, because it creates files that need to be manually checked
    /// </summary>
    internal class SvgExportTest
    {
        private const string FileOut = @"C:\Users\seb2\Desktop\SvgExportTest.svg";

        private IList<Interval> _input;
        private SvgExport _sut;

        [SetUp]
        public void Setup()
        {
            _input = new List<Interval>();
            _sut = new SvgExport();
        }

        [Test]
        public void IdeOpen()
        {
            _input.Add(
                new VisualStudioOpenedInterval
                {
                    StartTime = Date(0),
                    Duration = Dur(1)
                });
            Render();
        }

        [Test]
        public void IdeActive()
        {
            _input.Add(
                new VisualStudioOpenedInterval
                {
                    StartTime = Date(0),
                    Duration = Dur(5)
                });
            _input.Add(
                new VisualStudioActiveInterval
                {
                    StartTime = Date(1),
                    Duration = Dur(1.5)
                });
            _input.Add(
                new VisualStudioActiveInterval
                {
                    StartTime = Date(3),
                    Duration = Dur(1.5)
                });
            Render();
        }

        [Test]
        public void IdeOpen_MakeSureMarkersAreNicelyPrintedForVeryShortIntervals()
        {
            _input.Add(
                new VisualStudioOpenedInterval
                {
                    StartTime = Date(0),
                    Duration = Dur(3)
                });
            _input.Add(
                new UserActiveInterval
                {
                    StartTime = Date(1),
                    Duration = Dur(0.000000000001)
                });
            Render();
        }

        [Test]
        public void IdeOpenTwice()
        {
            _input.Add(
                new VisualStudioOpenedInterval
                {
                    StartTime = Date(0),
                    Duration = Dur(2)
                });
            _input.Add(
                new VisualStudioOpenedInterval
                {
                    StartTime = Date(3),
                    Duration = Dur(4)
                });
            Render();
        }

        [Test]
        public void Perspectives()
        {
            _input.Add(
                new PerspectiveInterval
                {
                    StartTime = Date(0),
                    Duration = Dur(2),
                    Perspective = PerspectiveType.Debug
                });
            _input.Add(
                new PerspectiveInterval
                {
                    StartTime = Date(3),
                    Duration = Dur(4),
                    Perspective = PerspectiveType.Production
                });
            Render();
        }

        [Test]
        public void UserActive()
        {
            _input.Add(
                new UserActiveInterval
                {
                    StartTime = Date(0),
                    Duration = Dur(2.2)
                });
            _input.Add(
                new UserActiveInterval
                {
                    StartTime = Date(2.3),
                    Duration = Dur(10)
                });
            Render();
        }

        [Test]
        public void FileInteractions()
        {
            _input.Add(
                new FileInteractionInterval
                {
                    StartTime = Date(0),
                    Duration = Dur(3),
                    Type = FileInteractionType.Reading,
                    FileName = @"a\b\c\x.cs"
                });
            _input.Add(
                new FileInteractionInterval
                {
                    StartTime = Date(3),
                    Duration = Dur(2),
                    Type = FileInteractionType.Typing,
                    FileName = @"a\b\c\x.cs"
                });
            _input.Add(
                new FileInteractionInterval
                {
                    StartTime = Date(5),
                    Duration = Dur(1),
                    Type = FileInteractionType.Reading,
                    FileName = @"a\b\c\y.cs"
                });
            _input.Add(
                new FileInteractionInterval
                {
                    StartTime = Date(7),
                    Duration = Dur(1),
                    Type = FileInteractionType.Reading,
                    FileName = @"a\b\c\x.cs"
                });
            _input.Add(
                new FileInteractionInterval
                {
                    StartTime = Date(9),
                    Duration = Dur(2),
                    Type = FileInteractionType.Reading,
                    FileName = @"a\b\c\y.cs"
                });
            _input.Add(
                new FileInteractionInterval
                {
                    StartTime = Date(11),
                    Duration = Dur(5),
                    Type = FileInteractionType.Typing,
                    FileName = @"a\b\c\y.cs"
                });
            _input.Add(
                new FileInteractionInterval
                {
                    StartTime = Date(16),
                    Duration = Dur(2),
                    Type = FileInteractionType.Reading,
                    FileName = @"some\file\with\a\very\long\file\name.cs"
                });
            Render();
        }

        [Test]
        public void TestRun()
        {
            // just to see margin 
            _input.Add(
                new UserActiveInterval
                {
                    StartTime = Date(0),
                    Duration = Dur(10)
                });
            _input.Add(
                new TestRunInterval
                {
                    StartTime = Date(0),
                    Duration = Dur(10),
                    Result = TestResult.Error,
                    TestClasses =
                    {
                        new TestRunInterval.TestClassResult
                        {
                            Duration = Dur(6),
                            Result = TestResult.Error,
                            TestMethods =
                            {
                                new TestRunInterval.TestMethodResult
                                {
                                    Duration = Dur(1),
                                    Result = TestResult.Error
                                },
                                new TestRunInterval.TestMethodResult
                                {
                                    Duration = Dur(2),
                                    Result = TestResult.Success
                                },
                                new TestRunInterval.TestMethodResult
                                {
                                    Duration = Dur(3),
                                    Result = TestResult.Failed
                                }
                            }
                        },
                        new TestRunInterval.TestClassResult
                        {
                            Duration = Dur(4),
                            Result = TestResult.Error,
                            TestMethods =
                            {
                                new TestRunInterval.TestMethodResult
                                {
                                    Duration = Dur(2),
                                    Result = TestResult.Ignored
                                },
                                new TestRunInterval.TestMethodResult
                                {
                                    Duration = Dur(2),
                                    Result = TestResult.Unknown
                                }
                            }
                        }
                    }
                });
            // just to see margin
            _input.Add(
                new FileInteractionInterval
                {
                    StartTime = Date(0),
                    Duration = Dur(10),
                    Type = FileInteractionType.Reading,
                    FileName = @"a\b\c\y.cs"
                });
            Render();
        }

        private void Render()
        {
            _sut.Run(_input, FileOut);
        }

        private static TimeSpan Dur(double s)
        {
            Assert.That(s > 0);
            var now = DateTime.Now;
            var then = now.AddSeconds(s);
            return then - now;
        }

        private static DateTime Date(double s)
        {
            return DateTime.MinValue.AddSeconds(s);
        }
    }
}