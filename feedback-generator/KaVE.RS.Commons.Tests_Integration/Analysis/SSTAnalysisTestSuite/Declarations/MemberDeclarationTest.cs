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

using KaVE.Commons.Model.Names.CSharp;
using KaVE.Commons.Model.SSTs.Impl.Declarations;
using KaVE.Commons.Utils.Collections;
using NUnit.Framework;

namespace KaVE.RS.Commons.Tests_Integration.Analysis.SSTAnalysisTestSuite.Declarations
{
    internal class MemberDeclarationTest : BaseSSTAnalysisTest
    {
        [Test]
        public void DelegateDeclaration()
        {
            CompleteInClass(@"
                public delegate void D(object o);
                $
            ");

            var expected = Sets.NewHashSet(
                new DelegateDeclaration
                {
                    Name =
                        DelegateTypeName.Get(
                            "d:[System.Void, mscorlib, 4.0.0.0] [N.C+D, TestProject].([System.Object, mscorlib, 4.0.0.0] o)")
                });
            Assert.AreEqual(expected, ResultSST.Delegates);
        }

        [Test]
        public void EventDeclaration()
        {
            CompleteInClass(@"
                public event int E;
                $
            ");

            var expected = Sets.NewHashSet(
                new EventDeclaration
                {
                    Name = EventName.Get("[System.Int32, mscorlib, 4.0.0.0] [N.C, TestProject].E")
                });
            Assert.AreEqual(expected, ResultSST.Events);
        }

        [Test]
        public void FieldDeclaration()
        {
            CompleteInClass(@"
                public int _f;
                $
            ");

            var expected =
                Sets.NewHashSet(
                    new FieldDeclaration
                    {
                        Name = FieldName.Get("[System.Int32, mscorlib, 4.0.0.0] [N.C, TestProject]._f")
                    });
            Assert.AreEqual(expected, ResultSST.Fields);
        }

        [Test]
        public void MethodDeclaration()
        {
            CompleteInClass(@"
                public void M() {}
                private void N() {}
                $
            ");

            var m = new MethodDeclaration
            {
                Name = MethodName.Get("[System.Void, mscorlib, 4.0.0.0] [N.C, TestProject].M()"),
                IsEntryPoint = true
            };
            var n = new MethodDeclaration
            {
                Name = MethodName.Get("[System.Void, mscorlib, 4.0.0.0] [N.C, TestProject].N()"),
                IsEntryPoint = false
            };
            var expected = Sets.NewHashSet(m, n);
            Assert.AreEqual(expected, ResultSST.Methods);
        }

        [Test]
        public void PropertyDeclaration()
        {
            CompleteInClass(@"
                public int P {get;set;}
                $
            ");

            var expected =
                Sets.NewHashSet(
                    new PropertyDeclaration
                    {
                        Name = PropertyName.Get("set get [System.Int32, mscorlib, 4.0.0.0] [N.C, TestProject].P()")
                    });
            Assert.AreEqual(expected, ResultSST.Properties);
        }
    }
}