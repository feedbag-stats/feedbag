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

using KaVE.Commons.Model.SSTs.Impl;
using KaVE.Commons.Model.SSTs.Impl.Expressions.Simple;
using NUnit.Framework;
using Fix = KaVE.VsFeedbackGenerator.RS8Tests.Analysis.SSTAnalysisTestSuite.SSTAnalysisFixture;

namespace KaVE.VsFeedbackGenerator.RS8Tests.Analysis.SSTAnalysisTestSuite
{
    internal class ComposedExpressionAnalysisTest : BaseSSTAnalysisTest
    {

        [Test, Ignore]
        public void CompositionOfPrimitives()
        {
            CompleteInClass(@"
                public void A()
                {
                    var i = 1;
                    var j = i + 1;
                    $
                }
            ");

            var mA = NewMethodDeclaration(Fix.Void, "A");
            mA.Body.Add(SSTUtil.Declare("i", Fix.Int));
            mA.Body.Add(SSTUtil.AssignmentToLocal("i", new ConstantValueExpression()));
            mA.Body.Add(SSTUtil.Declare("j", Fix.Int));
            mA.Body.Add(SSTUtil.AssignmentToLocal("j", SSTUtil.ComposedExpression("i")));

            AssertAllMethods(mA);
        }

        [Test, Ignore]
        public void Postfix()
        {
            CompleteInClass(@"
                public void A(int i)
                {
                    var j = i++;
                    $
                }
            ");

            var mA = NewMethodDeclaration(Fix.Void, "A", string.Format("[{0}] i", Fix.Int));
            mA.Body.Add(SSTUtil.Declare("j", Fix.Int));
            mA.Body.Add(SSTUtil.AssignmentToLocal("j", SSTUtil.ComposedExpression("i")));

            AssertAllMethods(mA);
        }

        [Test, Ignore]
        public void Prefix()
        {
            CompleteInClass(@"
                public void A(int i)
                {
                    var j = ++i;
                    $
                }
            ");

            var mA = NewMethodDeclaration(Fix.Void, "A", string.Format("[{0}] i", Fix.Int));
            mA.Body.Add(SSTUtil.Declare("j", Fix.Int));
            mA.Body.Add(SSTUtil.AssignmentToLocal("j", SSTUtil.ComposedExpression("i")));

            AssertAllMethods(mA);
        }

        [Test, Ignore]
        public void Unary()
        {
            CompleteInClass(@"
                public void A(int i)
                {
                    var j = -i;
                    $
                }
            ");

            var mA = NewMethodDeclaration(Fix.Void, "A", string.Format("[{0}] i", Fix.Int));
            mA.Body.Add(SSTUtil.Declare("j", Fix.Int));
            mA.Body.Add(SSTUtil.AssignmentToLocal("j", SSTUtil.ComposedExpression("i")));

            AssertAllMethods(mA);
        }


        [Test, Ignore]
        public void CompositionOfConstants()
        {
            CompleteInClass(@"
                public void A()
                {
                    var i = 5 + 3;
                    $
                }
            ");

            var mA = NewMethodDeclaration(Fix.Void, "A");
            mA.Body.Add(SSTUtil.Declare("i", Fix.Int));
            mA.Body.Add(SSTUtil.AssignmentToLocal("i", new ConstantValueExpression()));

            AssertAllMethods(mA);
        }
    }
}