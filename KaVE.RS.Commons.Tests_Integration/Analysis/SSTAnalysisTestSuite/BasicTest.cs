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

using System.Linq;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using KaVE.Commons.Model.Naming;
using KaVE.Commons.Model.SSTs.Impl.Blocks;
using KaVE.Commons.Model.SSTs.Impl.Expressions.Assignable;
using KaVE.RS.Commons.Analysis.CompletionTarget;
using NUnit.Framework;
using Fix = KaVE.RS.Commons.Tests_Integration.Analysis.SSTAnalysisTestSuite.SSTAnalysisFixture;
using IVariableDeclaration = KaVE.Commons.Model.SSTs.Statements.IVariableDeclaration;

namespace KaVE.RS.Commons.Tests_Integration.Analysis.SSTAnalysisTestSuite
{
    // only covers basic cases of declarations, assignments, constant int values, and different completion points
    internal class BasicTest : BaseSSTAnalysisTest
    {
        [Test]
        public void LonelyVariableDeclaration()
        {
            CompleteInMethod(
                @"
                int i;
                $
            ");

            AssertNodeIsVariableDeclaration("i", LastCompletionMarker.HandlingNode);
            AssertCompletionCase(CompletionCase.EmptyCompletionAfter);

            AssertBody(
                VarDecl("i", Fix.Int),
                ExprStmt(new CompletionExpression()));
        }

        [Test]
        public void ShouldResolveTypeAliases()
        {
            CompleteInNamespace(
                @"
                using X = System.String;
                class C
                {
                    void M()
                    {
                        X x;
                        $
                    }
                }
            ");

            Assert.AreEqual(1, ResultSST.Methods.Count);
            var m = ResultSST.Methods.First();
            Assert.AreEqual(2, m.Body.Count);
            var v = m.Body.First() as IVariableDeclaration;
            Assert.NotNull(v);
            Assert.AreEqual(Fix.String, v.Type);
        }

        [Test]
        public void BlocksAreAnalyzed()
        {
            CompleteInMethod(
                @"
                { int i; }
                $
            ");

            // TODO NameUpdate: Add target test and add separate test file for blocks
            AssertCompletionCase(CompletionCase.EmptyCompletionAfter);

            AssertBody(
                VarDecl("i", Fix.Int),
                ExprStmt(new CompletionExpression()));
        }

        [Test]
        public void AssignmentOfConstantValue()
        {
            CompleteInMethod(
                @"
                int i = 1;
                $
            ");

            AssertNodeIsVariableDeclaration("i", LastCompletionMarker.HandlingNode);
            AssertCompletionCase(CompletionCase.EmptyCompletionAfter);

            AssertBody(
                VarDecl("i", Fix.Int),
                Assign("i", Const("1")),
                ExprStmt(new CompletionExpression()));
        }

        [Test]
        public void PlainCompletion()
        {
            CompleteInClass(
                @"
                public void A() {
                    $
                }
            ");

            AssertNodeIsMethodDeclaration("A", LastCompletionMarker.HandlingNode);
            AssertCompletionCase(CompletionCase.InBody);

            AssertBody(
                ExprStmt(new CompletionExpression()));
        }

        [Test]
        public void CompletionBeforeDeclaration()
        {
            CompleteInMethod(
                @"
                $
                int i;
            ");

            AssertNodeIsVariableDeclaration("i", LastCompletionMarker.HandlingNode);
            AssertCompletionCase(CompletionCase.EmptyCompletionBefore);

            AssertBody(
                ExprStmt(new CompletionExpression()),
                VarDecl("i", Fix.Int));
        }

        [Test]
        public void CompletionAfterDeclaration()
        {
            CompleteInMethod(
                @"
                int i;
                $
            ");

            AssertNodeIsVariableDeclaration("i", LastCompletionMarker.HandlingNode);
            AssertCompletionCase(CompletionCase.EmptyCompletionAfter);

            AssertBody(
                VarDecl("i", Fix.Int),
                ExprStmt(new CompletionExpression()));
        }

        [Test]
        public void CompletionInBetweenDeclarations()
        {
            CompleteInMethod(
                @"
                int i;
                $
                int j;
            ");

            AssertNodeIsVariableDeclaration("i", LastCompletionMarker.HandlingNode);
            AssertCompletionCase(CompletionCase.EmptyCompletionAfter);

            AssertBody(
                VarDecl("i", Fix.Int),
                ExprStmt(new CompletionExpression()),
                VarDecl("j", Fix.Int));
        }

        [Test]
        public void CompletionBeforeExpressionStatement()
        {
            CompleteInMethod(
                @"
                $
                int i = 1;
            ");

            AssertNodeIsVariableDeclaration("i", LastCompletionMarker.HandlingNode);
            AssertCompletionCase(CompletionCase.EmptyCompletionBefore);

            AssertBody(
                ExprStmt(new CompletionExpression()),
                VarDecl("i", Fix.Int),
                Assign("i", Const("1")));
        }

        [Test]
        public void CompletionAfterExpressionStatement()
        {
            CompleteInMethod(
                @"
                int i = 1;
                $
            ");

            AssertNodeIsVariableDeclaration("i", LastCompletionMarker.HandlingNode);
            AssertCompletionCase(CompletionCase.EmptyCompletionAfter);

            AssertBody(
                VarDecl("i", Fix.Int),
                Assign("i", Const("1")),
                ExprStmt(new CompletionExpression()));
        }

        [Test]
        public void CompletionInDeclarationInitializer()
        {
            CompleteInMethod(
                @"
                int i = $
            ");

            AssertNodeIsVariableDeclaration("i", LastCompletionMarker.HandlingNode);
            AssertCompletionCase(CompletionCase.Undefined);

            AssertBody(
                VarDecl("i", Fix.Int),
                Assign("i", new CompletionExpression()));
        }

        [Test]
        public void CompletionInDeclarationInitializerWithoutSpace()
        {
            TestAnalysisTrigger.IsPrintingType = true;

            CompleteInMethod(
                @"
                int i=$
            ");

            AssertNodeIsVariableDeclaration("i", LastCompletionMarker.HandlingNode);
            AssertCompletionCase(CompletionCase.Undefined);

            AssertBody(
                VarDecl("i", Fix.Int),
                Assign("i", new CompletionExpression()));
        }

        [Test]
        public void CompletionInDeclarationInitializerWithReference()
        {
            CompleteInMethod(
                @"
                int i = t$
            ");

            AssertNodeIsReference("t", LastCompletionMarker.HandlingNode);
            AssertCompletionCase(CompletionCase.Undefined);

            AssertBody(
                VarDecl("i", Fix.Int),
                Assign(
                    "i",
                    new CompletionExpression
                    {
                        Token = "t"
                    }));
        }

        [Test]
        public void CompletionInAssignment()
        {
            CompleteInMethod(
                @"
                int i;
                i = $
            ");

            AssertNodeIsAssignment("i", LastCompletionMarker.HandlingNode);
            AssertCompletionCase(CompletionCase.Undefined);

            AssertBody(
                VarDecl("i", Fix.Int),
                Assign("i", new CompletionExpression()));
        }

        [Test]
        public void CompletionInIfBody()
        {
            CompleteInMethod(
                @"
                if(true)
                {
                    $
                }
            ");

            AssertCompletionMarker<IIfStatement>(CompletionCase.InBody);

            AssertBody(
                new IfElseBlock
                {
                    Condition = Const("true"),
                    Then =
                    {
                        ExprStmt(new CompletionExpression())
                    }
                });
        }

        [Test]
        public void MethodCall()
        {
            CompleteInMethod(
                @"
                this.GetHashCode();
                $
            ");

            AssertCompletionMarker<IInvocationExpression>(CompletionCase.EmptyCompletionAfter);

            AssertBody(
                InvokeStmt("this", Fix.Object_GetHashCode),
                ExprStmt(new CompletionExpression()));
        }

        [Test]
        public void TriggeredOutsideOfMethods()
        {
            CompleteInClass(
                @"
                public static void M() {}
                $
            ");

            AssertCompletionMarker<IMethodDeclaration>(CompletionCase.EmptyCompletionAfter);
        }

        [Test]
        public void EnclosingTypeAndPartialClassIdentifier_nonPartial()
        {
            CompleteInCSharpFile(
                @"
                namespace N {
                    class C {
                        void M() {
                            $
                        }
                    }
                }
            ");

            Assert.AreEqual(Names.Type("N.C, TestProject"), ResultSST.EnclosingType);
            Assert.Null(ResultSST.PartialClassIdentifier);
        }

        [Test]
        public void EnclosingTypeAndPartialClassIdentifier_Partial()
        {
            CompleteInCSharpFile(
                @"
                namespace N {
                    partial class C {
                        void M() {
                            $
                        }
                    }
                }
            ");

            Assert.AreEqual(Names.Type("N.C, TestProject"), ResultSST.EnclosingType);
            Assert.That(ResultSST.PartialClassIdentifier.StartsWith("<TestProject>"));
            // name of test file is randomized and is unknown before
            Assert.That(ResultSST.PartialClassIdentifier.EndsWith(".cs"));
        }
    }
}