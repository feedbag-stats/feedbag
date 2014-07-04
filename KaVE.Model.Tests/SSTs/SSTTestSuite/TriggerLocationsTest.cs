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
 *    - Sebastian Proksch
 */

using System;
using KaVE.Model.SSTs;
using NUnit.Framework;

namespace KaVE.Model.Tests.SSTs.SSTTestSuite
{
    [TestFixture, Ignore]
    internal class TriggerLocationsTest : AbstractSSTTest
    {
        [Test]
        public void TriggeredOutsideMethod()
        {
            WhenTriggeredIn(@"
                public void A() {}
                $
            ");

            var sst = new SST();
            sst.AddEntrypoint(NewMethodDeclaration("A"));

            AssertResult(sst);
        }

        [Test]
        public void TriggeredOutsideMethod_WithToken()
        {
            WhenTriggeredIn(@"
                public void A() {}
                B$
            ");

            var sst = new SST();
            sst.AddEntrypoint(NewMethodDeclaration("A"));

            sst.Add(new TypeTrigger {Token = "B"});

            AssertResult(sst);
        }

        [Test, ExpectedException(typeof (NotSupportedException))]
        public void TriggeredInMethodDeclaration()
        {
            WhenTriggeredIn(@"
                public void$ A() {}
            ");
        }

        [Test, ExpectedException(typeof (NotSupportedException))]
        public void TriggeredInParameterList()
        {
            WhenTriggeredIn(@"
                public void A($) {}
            ");
        }

        [Test]
        public void TriggeredInMethod()
        {
            WhenTriggeredIn(@"
                public void A()
                {
                    $
                }
            ");

            var trigger = new TypeTrigger();

            var mA = NewMethodDeclaration("A");
            mA.Body.Add(trigger);

            var sst = new SST();
            sst.AddEntrypoint(mA);

            AssertResult(sst);
        }

        [Test]
        public void TriggeredInMethod_WithToken()
        {
            WhenTriggeredIn(@"
                public void A()
                {
                    a.b$
                }
            ");

            var trigger = new MethodTrigger {Token = "a.b"};

            var mA = NewMethodDeclaration("A");
            mA.Body.Add(trigger);

            var sst = new SST();
            sst.AddEntrypoint(mA);

            AssertResult(sst);
        }
    }
}