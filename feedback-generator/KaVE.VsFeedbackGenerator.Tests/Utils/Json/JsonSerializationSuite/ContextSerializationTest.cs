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
 *    - Sven Amann
 *    - Dennis Albrecht
 */

using System.Collections.Generic;
using KaVE.Model.Events.CompletionEvent;
using KaVE.TestUtils.Model.Events.CompletionEvent;
using KaVE.TestUtils.Model.Names;
using NUnit.Framework;

namespace KaVE.VsFeedbackGenerator.Tests.Utils.Json.JsonSerializationSuite
{
    [TestFixture]
    internal class ContextSerializationTest : SerializationTestBase
    {
        [Test]
        public void ShouldSerializeEmptyContext()
        {
            var context = Context.Empty;
            JsonAssert.SerializationPreservesData(context);
        }

        [Test]
        public void ShouldSerializeAllFieldsOfContext()
        {
            var context = new Context
            {
                TypeShape = new TypeShape
                {
                    TypeHierarchy = CompletionEventTestFactory.GetAnonymousTypeHierarchy(),
                    MethodHierarchies = new HashSet<MethodHierarchy>
                    {
                        new MethodHierarchy(TestNameFactory.GetAnonymousMethodName())
                        {
                            First = TestNameFactory.GetAnonymousMethodName(),
                            Super = TestNameFactory.GetAnonymousMethodName(),
                        }
                    }
                }
            };
            JsonAssert.SerializationPreservesData(context);
        }

        [Test]
        public void ShouldSerializeToString()
        {
            var context = new Context
            {
                TypeShape = new TypeShape
                {
                    MethodHierarchies = CompletionEventTestFactory.GetAnonymousMethodHierarchies(2),
                    TypeHierarchy = CompletionEventTestFactory.GetAnonymousTypeHierarchy()
                }
            };
            const string expected =
                "{\"$type\":\"KaVE.Model.Events.CompletionEvent.Context, KaVE.Model\",\"TypeShape\":{\"$type\":\"KaVE.Model.Events.CompletionEvent.TypeShape, KaVE.Model\",\"TypeHierarchy\":{\"$type\":\"KaVE.Model.Events.CompletionEvent.TypeHierarchy, KaVE.Model\",\"Element\":\"CSharp.TypeName:SomeType11, SomeAssembly12, 9.8.7.6\",\"Implements\":[]},\"MethodHierarchies\":[{\"$type\":\"KaVE.Model.Events.CompletionEvent.MethodHierarchy, KaVE.Model\",\"Element\":\"CSharp.MethodName:[SomeType1, SomeAssembly2, 9.8.7.6] [SomeType3, SomeAssembly4, 9.8.7.6].Method5()\"},{\"$type\":\"KaVE.Model.Events.CompletionEvent.MethodHierarchy, KaVE.Model\",\"Element\":\"CSharp.MethodName:[SomeType6, SomeAssembly7, 9.8.7.6] [SomeType8, SomeAssembly9, 9.8.7.6].Method10()\"}]},\"SST\":{\"$type\":\"KaVE.Model.SSTs.SST, KaVE.Model\",\"Fields\":[],\"Properties\":[],\"Methods\":[],\"Events\":[],\"Delegates\":[],\"EntryPoints\":[],\"NonEntryPoints\":[]}}";

            JsonAssert.SerializesTo(context, expected);
        }
    }
}