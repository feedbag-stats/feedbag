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
using KaVE.CompletionTraceGenerator.Model;
using NUnit.Framework;

namespace KaVE.CompletionTraceGenerator.Tests.Model
{
    [TestFixture]
    internal class CompletionActionTest
    {
        [Test]
        public void ShouldCreateAbortAction()
        {
            var actual = CompletionAction.NewCancel();
            Assert.AreEqual(ActionType.Cancel, actual.Type);

            Assert.IsNull(actual.Direction);
            Assert.IsNull(actual.Index);
            Assert.IsNull(actual.Token);
        }

        [Test]
        public void ShouldCreateCompleteAction()
        {
            var actual = CompletionAction.NewApply();
            Assert.AreEqual(ActionType.Apply, actual.Type);

            Assert.IsNull(actual.Direction);
            Assert.IsNull(actual.Index);
            Assert.IsNull(actual.Token);
        }

        [TestCase("asd"), TestCase("sdf")]
        public void ShouldCreateFilterAction(string testToken)
        {
            var actual = CompletionAction.NewFilter(testToken);
            Assert.AreEqual(ActionType.Filter, actual.Type);
            Assert.AreEqual(testToken, actual.Token);

            Assert.IsNull(actual.Direction);
            Assert.IsNull(actual.Index);
        }

        [TestCase(123), TestCase(243)]
        public void ShouldCreateMouseGotoAction(int testIndex)
        {
            var actual = CompletionAction.NewMouseGoto(testIndex);
            Assert.AreEqual(ActionType.MouseGoto, actual.Type);
            Assert.AreEqual(testIndex, actual.Index);

            Assert.IsNull(actual.Direction);
            Assert.IsNull(actual.Token);
        }

        [TestCase(Direction.Down), TestCase(Direction.Up)]
        public void ShouldCreatePageStepAction(Direction testDirection)
        {
            var actual = CompletionAction.NewPageStep(testDirection);
            Assert.AreEqual(ActionType.PageStep, actual.Type);
            Assert.AreEqual(testDirection, actual.Direction);

            Assert.IsNull(actual.Index);
            Assert.IsNull(actual.Token);
        }

        [TestCase(1623), TestCase(243)]
        public void ShouldCreateScrollAction(int testIndex)
        {
            var actual = CompletionAction.NewScroll(testIndex);
            Assert.AreEqual(ActionType.Scroll, actual.Type);
            Assert.AreEqual(testIndex, actual.Index);

            Assert.IsNull(actual.Direction);
            Assert.IsNull(actual.Token);
        }

        [TestCase(Direction.Down), TestCase(Direction.Up)]
        public void ShouldCreateStepAction(Direction testDirection)
        {
            var actual = CompletionAction.NewStep(testDirection);
            Assert.AreEqual(ActionType.Step, actual.Type);
            Assert.AreEqual(testDirection, actual.Direction);

            Assert.IsNull(actual.Index);
            Assert.IsNull(actual.Token);
        }
    }
}