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

using JetBrains.ReSharper.Psi.CSharp.Tree;
using KaVE.Commons.Model.SSTs.Impl.Blocks;
using KaVE.RS.Commons.Analysis.CompletionTarget;
using NUnit.Framework;
using Fix = KaVE.RS.Commons.Tests_Integration.Analysis.SSTAnalysisTestSuite.SSTAnalysisFixture;

namespace KaVE.RS.Commons.Tests_Integration.Analysis.SSTAnalysisTestSuite.Blocks
{
    internal class IfElseBlockAnalysisTest : BaseSSTAnalysisTest
    {
        [Test]
        public void BasicWithCompletionBefore()
        {
            CompleteInMethod(@"
                $
                if (true) {}
                else {}
            ");

            AssertCompletionMarker<IIfStatement>(CompletionCase.EmptyCompletionBefore);

            AssertBody(
                Fix.EmptyCompletion,
                new IfElseBlock
                {
                    Condition = Const("true")
                });
        }

        [Test]
        public void BasicWithCompletionInThen_Empty()
        {
            CompleteInMethod(@"
                if (true) {
                    $
                }
                else {}
            ");

            AssertCompletionMarker<IIfStatement>(CompletionCase.InBody);

            AssertBody(
                new IfElseBlock
                {
                    Condition = Const("true"),
                    Then = {Fix.EmptyCompletion}
                });
        }

        [Test]
        public void BasicWithCompletionInThen_AfterStatement()
        {
            CompleteInMethod(@"
                if (true) {
                    int i;
                    $
                }
                else {}
            ");

            AssertCompletionMarker<ILocalVariableDeclaration>(CompletionCase.EmptyCompletionAfter);

            AssertBody(
                new IfElseBlock
                {
                    Condition = Const("true"),
                    Then =
                    {
                        VarDecl("i", Fix.Int),
                        Fix.EmptyCompletion
                    }
                });
        }

        [Test]
        public void BasicWithCompletionInElse_Empty()
        {
            CompleteInMethod(@"
                if (true) {}
                else {
                    $
                }
            ");

            AssertCompletionMarker<IIfStatement>(CompletionCase.InElse);

            AssertBody(
                new IfElseBlock
                {
                    Condition = Const("true"),
                    Else = {Fix.EmptyCompletion}
                });
        }

        [Test]
        public void BasicWithCompletionInElse_AfterStatement()
        {
            CompleteInMethod(@"
                if (true) {}
                else {
                    int j;
                    $
                }
            ");

            AssertCompletionMarker<ILocalVariableDeclaration>(CompletionCase.EmptyCompletionAfter);

            AssertBody(
                new IfElseBlock
                {
                    Condition = Const("true"),
                    Else =
                    {
                        VarDecl("j", Fix.Int),
                        Fix.EmptyCompletion
                    }
                });
        }

        [Test]
        public void BasicWithCompletionAfter()
        {
            CompleteInMethod(@"
                if (true) {}
                else {}
                $
            ");

            AssertCompletionMarker<IIfStatement>(CompletionCase.EmptyCompletionAfter);

            AssertBody(
                new IfElseBlock
                {
                    Condition = Const("true")
                },
                Fix.EmptyCompletion);
        }

        // TODO: completion in condition

        [Test]
        public void NoElseBlock()
        {
            CompleteInMethod(@"
                if (true) {}
                $
            ");

            AssertCompletionMarker<IIfStatement>(CompletionCase.EmptyCompletionAfter);

            AssertBody(
                new IfElseBlock
                {
                    Condition = Const("true")
                },
                Fix.EmptyCompletion);
        }

        [Test]
        public void WithStatements()
        {
            CompleteInMethod(@"
                if (true) {
                    int i;
                } else {
                    int j;
                }
                $
            ");

            AssertCompletionMarker<IIfStatement>(CompletionCase.EmptyCompletionAfter);

            AssertBody(
                new IfElseBlock
                {
                    Condition = Const("true"),
                    Then = {VarDecl("i", Fix.Int)},
                    Else = {VarDecl("j", Fix.Int)}
                },
                Fix.EmptyCompletion);
        }
    }
}