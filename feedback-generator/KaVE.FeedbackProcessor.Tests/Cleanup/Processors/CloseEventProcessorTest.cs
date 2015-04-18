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
 *    - Mattis Manfred Kämmerer
 *    - Markus Zimmermann
 */

using KaVE.Commons.Model.Events;
using KaVE.Commons.Model.Events.VisualStudio;
using KaVE.Commons.TestUtils.Model.Events;
using KaVE.FeedbackProcessor.Cleanup.Processors;
using NUnit.Framework;

namespace KaVE.FeedbackProcessor.Tests.Cleanup.Processors
{
    [TestFixture]
    internal class CloseEventProcessorTest
    {
        private CloseEventProcessor _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new CloseEventProcessor();
        }

        [Test]
        public void ShouldNotFilterOtherEvents()
        {
            var anyEvent = new TestIDEEvent();

            var processedEvent = _uut.Process(anyEvent);

            Assert.AreEqual(anyEvent, processedEvent);
        }

        [Test]
        public void ShouldNotFilterNonClosingDocumentEvents()
        {
            var openedDocumentEvent = new DocumentEvent{Action = DocumentEvent.DocumentAction.Opened};

            var processedEvent = _uut.Process(openedDocumentEvent);

            Assert.AreEqual(openedDocumentEvent, processedEvent);
        }

        [Test]
        public void ReplacesDocumentClosingEventWithCloseEventTest()
        {
            var closingEvent = new DocumentEvent {Action = DocumentEvent.DocumentAction.Closing};

            var processedEvent = (CommandEvent) _uut.Process(closingEvent);

            Assert.AreEqual("Close", processedEvent.CommandId);
        }

        [Test]
        public void FiltersWindowCloseEvents()
        {
            var windowClosingEvent = new WindowEvent {Action = WindowEvent.WindowAction.Close};

            var processedEvent = _uut.Process(windowClosingEvent);

            Assert.IsNull(processedEvent);
        }

        [Test]
        public void ShouldNotFilterNonClosingWindowEvents()
        {
            var nonClosingWindowEvent = new WindowEvent {Action = WindowEvent.WindowAction.Activate};

            var processedEvent = _uut.Process(nonClosingWindowEvent);

            Assert.AreEqual(nonClosingWindowEvent, processedEvent);
        }

        [Test]
        public void FiltersDocumentCloseAfterCommandClose()
        {
            var commandCloseEvent = new CommandEvent {CommandId = "Close"};
            _uut.Process(commandCloseEvent);

            var documentEvent = new DocumentEvent {Action = DocumentEvent.DocumentAction.Closing};
            var processedEvent = _uut.Process(documentEvent);

            Assert.IsNull(processedEvent);
        }
    }
}