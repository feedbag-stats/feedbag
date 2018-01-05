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
using KaVE.Commons.Model.Naming;
using KaVE.Commons.Model.SSTs.Expressions.Assignable;
using KaVE.Commons.Model.SSTs.Impl.Expressions.Assignable;
using KaVE.Commons.Model.SSTs.Impl.Expressions.Simple;
using KaVE.Commons.Model.SSTs.Impl.Statements;
using KaVE.RS.Commons.Analysis.CompletionTarget;
using NUnit.Framework;
using Fix = KaVE.RS.Commons.Tests_Integration.Analysis.SSTAnalysisTestSuite.SSTAnalysisFixture;
using ILambdaExpression = JetBrains.ReSharper.Psi.CSharp.Tree.ILambdaExpression;
using NFix = KaVE.Commons.TestUtils.Model.Naming.NameFixture;

namespace KaVE.RS.Commons.Tests_Integration.Analysis.SSTAnalysisTestSuite.Expressions
{
    internal class LambdaExpressionAnalysisTest : BaseSSTAnalysisTest
    {
        [Test]
        public void StandardAction_EmptyBody()
        {
            CompleteInMethod(
                @"
                Action<int> a = (int i) => {  };
                $");

            AssertBody(
                VarDecl("a", Fix.ActionOfInt),
                Assign(
                    "a",
                    new LambdaExpression
                    {
                        Name = Names.Lambda("[{0}] ([{1}] i)", NFix.Void, NFix.Int)
                    }),
                Fix.EmptyCompletion);
        }

        [Test]
        public void StandardAction()
        {
            CompleteInMethod(
                @"
                Action<int> a = (int i) => { return; };
                $");

            AssertBody(
                VarDecl("a", Fix.ActionOfInt),
                Assign(
                    "a",
                    new LambdaExpression
                    {
                        Name = Names.Lambda("[{0}] ([{1}] i)", NFix.Void, NFix.Int),
                        Body = {new ReturnStatement {IsVoid = true}}
                    }),
                Fix.EmptyCompletion);
        }

        [Test]
        public void StandardFunc()
        {
            CompleteInMethod(
                @"
                Func<int, int> a = (int i) => { return i + 1; };
                $");

            AssertBody(
                VarDecl("a", Fix.FuncOfIntAndInt),
                Assign(
                    "a",
                    new LambdaExpression
                    {
                        Name = Names.Lambda("[{0}] ([{0}] i)", NFix.Int),
                        Body =
                        {
                            VarDecl("$0", Fix.Int),
                            Assign(
                                "$0",
                                new BinaryExpression
                                {
                                    LeftOperand = RefExpr("i"),
                                    Operator = BinaryOperator.Plus,
                                    RightOperand = Const("1")
                                }),
                            new ReturnStatement
                            {
                                IsVoid = false,
                                Expression = new ReferenceExpression {Reference = VarRef("$0")}
                            }
                        }
                    }),
                Fix.EmptyCompletion);
        }

        [Test]
        public void StandardFunc_ShorthandSyntax()
        {
            CompleteInMethod(
                @"
                Func<int, int> a = i => i + 1;
                $");

            AssertBody(
                VarDecl("a", Fix.FuncOfIntAndInt),
                Assign(
                    "a",
                    new LambdaExpression
                    {
                        Name = Names.Lambda("[{0}] ([{0}] i)", NFix.Int),
                        Body =
                        {
                            VarDecl("$0", Fix.Int),
                            Assign(
                                "$0",
                                new BinaryExpression
                                {
                                    LeftOperand = RefExpr("i"),
                                    Operator = BinaryOperator.Plus,
                                    RightOperand = Const("1")
                                }),
                            new ReturnStatement
                            {
                                IsVoid = false,
                                Expression = new ReferenceExpression {Reference = VarRef("$0")}
                            }
                        }
                    }),
                Fix.EmptyCompletion);
        }

        [Test]
        public void LambdaWithBodyBlock_CompletionWithin()
        {
            CompleteInMethod(@"Action<int> a = (int i) => { $ };");

            AssertBody(
                VarDecl("a", Fix.ActionOfInt),
                Assign(
                    "a",
                    new LambdaExpression
                    {
                        Name = Names.Lambda("[{0}] ([{1}] i)", NFix.Void, NFix.Int),
                        Body = {Fix.EmptyCompletion}
                    }));
        }

        [Test]
        public void Delegate()
        {
            CompleteInMethod(@"Action<int> a = delegate(int i) { return; }; $");

            AssertBody(
                VarDecl("a", Fix.ActionOfInt),
                Assign(
                    "a",
                    new LambdaExpression
                    {
                        Name = Names.Lambda("[{0}] ([{1}] i)", NFix.Void, NFix.Int),
                        Body = {Fix.Return}
                    }),
                Fix.EmptyCompletion);
        }

        [Test]
        public void Delegate_NoParam()
        {
            CompleteInMethod(@"Action<int> a = delegate { $ };");

            AssertCompletionMarker<IAnonymousMethodExpression>(CompletionCase.InBody);
            AssertBody(
                VarDecl("a", Fix.ActionOfInt),
                Assign(
                    "a",
                    new LambdaExpression
                    {
                        Name = Names.Lambda("[{0}] ()", Fix.Void),
                        Body = {Fix.EmptyCompletion}
                    }));
        }

        [Test]
        public void AssignMethodToActionVariable()
        {
            CompleteInClass(
                @"
                public void L(int i) { }
                public void M() { Action<int> a = L; $ }");

            var methodName =
                Names.Method(
                    "[{0}] [N.C, TestProject].L([{1}] i)",
                    NFix.Void,
                    NFix.Int);
            AssertBody(
                "M",
                VarDecl("a", Fix.ActionOfInt),
                Assign("a", RefExpr(MethodRef(methodName, VarRef("this")))),
                Fix.EmptyCompletion);
        }

        [Test]
        public void Delegate_Reassign()
        {
            CompleteInMethod(@"Action<int> a = delegate { $ }; Action<int> b = a;");

            AssertCompletionMarker<IAnonymousMethodExpression>(CompletionCase.InBody);
            AssertBody(
                VarDecl("a", Fix.ActionOfInt),
                Assign(
                    "a",
                    new LambdaExpression
                    {
                        Name = Names.Lambda("[{0}] ()", NFix.Void),
                        Body = {Fix.EmptyCompletion}
                    }),
                VarDecl("b", Fix.ActionOfInt),
                Assign("b", RefExpr("a")));
        }

        [Test, Ignore]
        public void StandardLambdaWithBodyExpression_CompletionWithin()
        {
            // AffectedNode is the variable declaration (case: in body)
            CompleteInMethod(@"Func<int, int> a = i => $;");

            AssertBody(
                VarDecl("a", Fix.FuncOfIntAndInt),
                Assign(
                    "a",
                    new LambdaExpression
                    {
                        Name = Names.Lambda("[{0}] ([{0}] i)", NFix.Int),
                        Body =
                        {
                            Fix.EmptyCompletion
                        }
                    }));
        }
    }
}