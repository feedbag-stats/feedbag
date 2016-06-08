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

using KaVE.Commons.Model.Names;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Model.Names.CSharp
{
    public class CsAssemblyNameTest
    {
        [TestCase("T, a, 0.0.0.0", "0.0.0.0"),
        TestCase("T, a", "???")]
        public void AssemblyVersion(string input, string expected)
        {
            var name = CsNameUtil.ParseTypeName(input);
            Assert.AreEqual(name.Assembly.AssemblyVersion.Identifier,expected);
        }

        [TestCase("T, a, 0.0.0.0", "a"),
        TestCase("T, a", "a")]
        public void AssemblyName(string input, string expected)
        {
            var name = CsNameUtil.ParseTypeName(input);
            Assert.AreEqual(name.Assembly.Name, expected);
        }

        [TestCase("T, a, 0.0.0.0", "a, 0.0.0.0"),
         TestCase("T, a", "a")]
        public void Identifier(string input, string expected)
        {
            var name = CsNameUtil.ParseTypeName(input);
            Assert.AreEqual(name.Assembly.Identifier, expected);
        }
    }
}