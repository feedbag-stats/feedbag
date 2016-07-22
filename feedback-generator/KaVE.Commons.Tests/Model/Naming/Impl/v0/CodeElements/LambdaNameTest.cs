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

using KaVE.Commons.Model.Naming.CodeElements;
using KaVE.Commons.Model.Naming.Impl.v0.CodeElements;
using KaVE.Commons.Model.Naming.Impl.v0.Types;
using KaVE.Commons.Utils.Assertion;
using KaVE.Commons.Utils.Collections;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Model.Naming.Impl.v0.CodeElements
{
    internal class LambdaNameTest
    {
        [Test]
        public void DefaultValues()
        {
            var sut = new LambdaName();
            Assert.AreEqual(new TypeName(), sut.ReturnType);
            Assert.True(sut.IsUnknown);
            Assert.False(sut.HasParameters);
            Assert.AreEqual(Lists.NewList<IParameterName>(), sut.Parameters);
        }

        [Test]
        public void ShouldRecognizeUnknownName()
        {
            Assert.True(new LambdaName().IsUnknown);
            Assert.True(new LambdaName("???").IsUnknown);
            Assert.False(new LambdaName("[?] ()").IsUnknown);
        }

        [Test, ExpectedException(typeof(AssertException))]
        public void ShouldAvoidNullParameters()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            new LambdaName(null);
        }

        [TestCaseSource(typeof(TestUtils), "TypeSource")]
        public void WithoutParameters(string typeId)
        {
            var name = new LambdaName("[System.String, mscorlib, 4.0.0.0] ()");

            Assert.False(name.HasParameters);
            CollectionAssert.IsEmpty(name.Parameters);
        }

        [TestCaseSource(typeof(TestUtils), "TypeSource")]
        public void WithParameters(string typeId)
        {
            var name = new LambdaName("[System.String, mscorlib, 4.0.0.0] ([C, A] p1, [C, B] p2)");

            Assert.True(name.HasParameters);
            CollectionAssert.AreEqual(
                new[] {new ParameterName("[C, A] p1"), new ParameterName("[C, B] p2")},
                name.Parameters);
        }

        [Test]
        public void ParameterParsingIsCached()
        {
            var sut = new LambdaName();
            var a = sut.Parameters;
            var b = sut.Parameters;
            Assert.AreSame(a, b);
        }
    }
}