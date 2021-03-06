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

using KaVE.Commons.Model.SSTs.Impl.Expressions.Assignable;
using NUnit.Framework;
using Fix = KaVE.RS.Commons.Tests_Integration.Analysis.SSTAnalysisTestSuite.SSTAnalysisFixture;

namespace KaVE.RS.Commons.Tests_Integration.Analysis.SSTAnalysisTestSuite.Expressions
{
    internal class NullCoalescingExpressionAnalysisTest : BaseSSTAnalysisTest
    {
        [Test]
        public void Basic()
        {
            CompleteInMethod(@"
                var o = GetType() ?? typeof(object);
                $
            ");

            AssertBody(
                VarDecl("o", Fix.Type),
                VarDecl("$0", Fix.Type),
                Assign("$0", Invoke("this", Fix.Object_GetType)),
                VarDecl("$1", Fix.Bool),
                Assign("$1", new ComposedExpression {References = {VarRef("$0")}}),
                Assign(
                    "o",
                    new IfElseExpression
                    {
                        Condition = RefExpr("$1"),
                        ThenExpression = RefExpr("$0"),
                        ElseExpression = Const("typeof")
                    }),
                Fix.EmptyCompletion);
        }
    }
}