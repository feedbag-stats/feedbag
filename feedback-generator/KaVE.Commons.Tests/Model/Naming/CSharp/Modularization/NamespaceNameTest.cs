/*
 * Copyright 2014 Technische Universit�t Darmstadt
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

using KaVE.Commons.Model.Naming.Impl.v0.Types;
using KaVE.Commons.Model.Naming.Types;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Model.Naming.CSharp.Modularization
{
    public class NamespaceNameTest
    {
        private const string TestNamespaceIdentifier = "foo.bar";

        private INamespaceName _testNamespaceName;
        private INamespaceName _testNamespaceParentName;

        [SetUp]
        public void SetUpTestNamespace()
        {
            _testNamespaceName = NamespaceName.Get(TestNamespaceIdentifier);
            _testNamespaceParentName = _testNamespaceName.ParentNamespace;
        }

        [Test]
        public void ShouldImplementIsUnknown()
        {
            Assert.That(NamespaceName.UnknownName.IsUnknown);
        }

        [Test]
        public void ShouldHaveLastIdentifierSegmentAsName()
        {
            Assert.AreEqual("bar", _testNamespaceName.Name);
        }

        [Test]
        public void ShouldNotBeGlobalNamespace()
        {
            Assert.IsFalse(_testNamespaceName.IsGlobalNamespace);
        }

        [Test]
        public void ShouldHaveFullqualifiedIdentifier()
        {
            Assert.AreEqual(TestNamespaceIdentifier, _testNamespaceName.Identifier);
        }

        [Test]
        public void ShouldHaveParentNamespaceName()
        {
            Assert.IsNotNull(_testNamespaceParentName);
        }

        [Test]
        public void ShouldHaveParentNamespaceFoo()
        {
            Assert.AreEqual("foo", _testNamespaceParentName.Name);
            Assert.AreEqual("foo", _testNamespaceParentName.Identifier);
            Assert.IsFalse(_testNamespaceParentName.IsGlobalNamespace);
        }

        [Test]
        public void ShouldHaveGlobalNamespaceAsGrandParent()
        {
            Assert.AreEqual(NamespaceName.GlobalNamespace, _testNamespaceParentName.ParentNamespace);
        }

        [Test]
        public void ShouldBeUnknownNamespace()
        {
            var uut = NamespaceName.UnknownName;

            Assert.AreEqual("???", uut.Identifier);
            Assert.AreSame(NamespaceName.GlobalNamespace, uut.ParentNamespace);
        }
    }
}