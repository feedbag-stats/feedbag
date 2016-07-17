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

using KaVE.Commons.Model.Naming;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Model.Naming.Impl.v0.CodeElements
{
    internal class LambdaNameTest
    {
        [Test]
        public void ReturnType()
        {
            var name = Names.GetLambdaName("[System.String, mscorlib, 4.0.0.0] ()");

            Assert.AreEqual("System.String, mscorlib, 4.0.0.0", name.ReturnType.Identifier);
        }

        [Test]
        public void WithoutParameters()
        {
            var name = Names.GetLambdaName("[System.String, mscorlib, 4.0.0.0] ()");

            Assert.False(name.HasParameters);
            CollectionAssert.IsEmpty(name.Parameters);
            Assert.AreEqual("()", name.Signature);
        }

        [Test]
        public void WithParameters()
        {
            var name = Names.GetLambdaName("[System.String, mscorlib, 4.0.0.0] ([C, A] p1, [C, B] p2)");

            Assert.True(name.HasParameters);
            CollectionAssert.AreEqual(
                new[] {Names.Parameter("[C, A] p1"), Names.Parameter("[C, B] p2")},
                name.Parameters);
            Assert.AreEqual("([C, A] p1, [C, B] p2)", name.Signature);
        }
    }
}