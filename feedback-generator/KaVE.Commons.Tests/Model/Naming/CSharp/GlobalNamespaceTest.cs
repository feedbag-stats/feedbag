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

namespace KaVE.Commons.Tests.Model.Naming.CSharp
{
    public class GlobalNamespaceTest
    {
        private readonly INamespaceName _globalNamespace = NamespaceName.GlobalNamespace;

        [Test]
        public void ShouldHaveEmptyName()
        {
            Assert.IsEmpty(_globalNamespace.Name);
        }

        [Test]
        public void ShouldBeGlobalNamespace()
        {
            Assert.IsTrue(_globalNamespace.IsGlobalNamespace);
        }

        [Test]
        public void ShouldHaveEmptyIdentifier()
        {
            Assert.IsEmpty(_globalNamespace.Identifier);
        }

        [Test]
        public void ShouldHaveNoParentNamespace()
        {
            Assert.IsNull(_globalNamespace.ParentNamespace);
        }
    }
}