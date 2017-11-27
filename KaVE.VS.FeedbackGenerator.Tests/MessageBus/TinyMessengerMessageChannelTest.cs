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
using System.Threading;
using KaVE.VS.Commons;
using KaVE.VS.FeedbackGenerator.MessageBus;
using NUnit.Framework;

namespace KaVE.VS.FeedbackGenerator.Tests.MessageBus
{
    [TestFixture]
    public class TinyMessengerMessageChannelTest
    {
        private class TestMessage
        {
            public string Id { get; set; }
        }

        private ManualResetEvent _messageReceivedEvent;
        private IMessageBus _channelUnderTest;

        [SetUp]
        public void SetUpChannelUnderTest()
        {
            _channelUnderTest = new TinyMessengerMessageBus();
            _messageReceivedEvent = new ManualResetEvent(false);
        }

        [Test]
        public void ShouldSucceedToSendMessage()
        {
            _channelUnderTest.Publish(new TestMessage {Id = "DEADBEEF"});
        }

        [Test]
        public void ShouldSucceedOnListenerRegistration()
        {
            _channelUnderTest.Subscribe<TestMessage>(e => { });
        }

        [Test]
        public void ShouldTransmitMessage()
        {
            var message = new TestMessage {Id = "jodeldiplom!"};
            var received = false;

            SubscribeToChannelUnderTest<TestMessage>(
                e =>
                {
                    Assert.AreEqual(e.Id, message.Id);
                    received = true;
                });

            PublishToChannelUnderTest(message);

            Assert.IsTrue(received);
        }

        [Test]
        public void ShouldNotReceiveMessageseOfOtherType()
        {
            SubscribeToChannelUnderTest<TestMessage>(e => Assert.Fail());
            PublishToChannelUnderTest(new object());
        }

        [Test]
        public void DoesNotReceiveEventsOfDerivedType()
        {
            var received = false;
            SubscribeToChannelUnderTest<object>(e => received = true);
            PublishToChannelUnderTest(new TestMessage());
            Assert.IsFalse(received);
        }

        [Test]
        public void ShouldNotReceiveMessagesPublishedBeforeSubscription()
        {
            var message = new TestMessage {Id = "rambzamba"};
            var eventCount = 0;

            PublishToChannelUnderTest(message);
            SubscribeToChannelUnderTest<TestMessage>(e => eventCount++);
            PublishToChannelUnderTest(message);

            Assert.AreEqual(1, eventCount);
        }

        [Test]
        public void ShouldFilterMessages()
        {
            var msg1 = new TestMessage {Id = "bellcanto"};
            var msg2 = new TestMessage {Id = "growling"};
            var msg3 = new TestMessage {Id = "belting"};
            var messageCount = 0;
            SubscribeToChannelUnderTest<TestMessage>(
                e =>
                {
                    Assert.AreNotEqual("growling", e.Id);
                    messageCount++;
                },
                e => !e.Id.Equals("growling"));

            PublishToChannelUnderTest(msg1);
            PublishToChannelUnderTest(msg2);
            PublishToChannelUnderTest(msg3);
            PublishToChannelUnderTest(msg2);

            Assert.AreEqual(2, messageCount);
        }

        private void SubscribeToChannelUnderTest<TMessage>(Action<TMessage> action, Func<TMessage, bool> filter = null)
            where TMessage : class
        {
            _channelUnderTest.Subscribe(
                e =>
                {
                    action.Invoke(e);
                    _messageReceivedEvent.Set();
                },
                filter);
        }

        private void PublishToChannelUnderTest<TMessage>(TMessage message) where TMessage : class
        {
            _channelUnderTest.Publish(message);
            _messageReceivedEvent.WaitOne(200);
            _messageReceivedEvent.Reset();
        }
    }
}