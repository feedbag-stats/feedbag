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

using KaVE.Commons.Model.SSTs;
using KaVE.Commons.Model.SSTs.Impl.References;
using KaVE.Commons.Model.SSTs.Impl.Statements;
using KaVE.Commons.Model.SSTs.Statements;
using KaVE.Commons.Utils.Naming;
using KaVE.VS.FeedbackGenerator.SessionManager.Anonymize.CompletionEvents;
using NUnit.Framework;

namespace KaVE.VS.FeedbackGenerator.Tests.SessionManager.Anonymize.CompletionEvents
{
    public class SSTStatementAnonymizationTest : SSTAnonymizationBaseTest
    {
        private SSTStatementAnonymization _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new SSTStatementAnonymization(ExpressionAnonymizationMock, ReferenceAnonymizationMock);
        }

        private void AssertAnonymization(IStatement statement, IStatement expected)
        {
            var actual = statement.Accept(_sut, 0);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Assignment()
        {
            AssertAnonymization(
                new Assignment
                {
                    Reference = AnyVarReference,
                    Expression = AnyExpression
                },
                new Assignment
                {
                    Reference = AnyVarReferenceAnonymized,
                    Expression = AnyExpressionAnonymized
                });
        }

        [Test]
        public void Assignment_Lambda()
        {
            AssertAnonymization(
                new Assignment
                {
                    Reference = AnyVarReference,
                    Expression = AnyLambdaExpr
                },
                new Assignment
                {
                    Reference = AnyVarReferenceAnonymized,
                    Expression = AnyLambdaExprAnonymized
                });
        }

        [Test]
        public void Assignment_DefaultSafe()
        {
            AssertAnonymization(new Assignment(), new Assignment());
        }

        [Test]
        public void BreakStatement()
        {
            AssertAnonymization(new BreakStatement(), new BreakStatement());
        }

        [Test]
        public void ContinueStatement()
        {
            AssertAnonymization(new ContinueStatement(), new ContinueStatement());
        }

        [Test]
        public void EventSubscriptionStatement()
        {
            AssertAnonymization(
                new EventSubscriptionStatement
                {
                    Reference = AnyVarReference,
                    Operation = EventSubscriptionOperation.Remove,
                    Expression = AnyExpression
                },
                new EventSubscriptionStatement
                {
                    Reference = AnyVarReferenceAnonymized,
                    Operation = EventSubscriptionOperation.Remove,
                    Expression = AnyExpressionAnonymized
                });
        }

        [Test]
        public void ExpressionStatement()
        {
            AssertAnonymization(
                new ExpressionStatement
                {
                    Expression = AnyExpression
                },
                new ExpressionStatement
                {
                    Expression = AnyExpressionAnonymized
                });
        }

        [Test]
        public void ExpressionStatement_Block()
        {
            AssertAnonymization(
                new ExpressionStatement
                {
                    Expression = AnyLambdaExpr
                },
                new ExpressionStatement
                {
                    Expression = AnyLambdaExprAnonymized
                });
        }

        [Test]
        public void ExpressionStatement_Lambda()
        {
            AssertAnonymization(
                new ExpressionStatement
                {
                    Expression = AnyLambdaExpr
                },
                new ExpressionStatement
                {
                    Expression = AnyLambdaExprAnonymized
                });
        }

        [Test]
        public void ExpressionStatement_DefaultSafe()
        {
            AssertAnonymization(new ExpressionStatement(), new ExpressionStatement());
        }

        [Test]
        public void GotoStatement()
        {
            AssertAnonymization(
                new GotoStatement
                {
                    Label = "g"
                },
                new GotoStatement
                {
                    Label = "g" // not anonymized
                });
        }

        [Test]
        public void GotoStatement_DefaultSafe()
        {
            AssertAnonymization(new GotoStatement(), new GotoStatement());
        }

        [Test]
        public void LabelledStatement()
        {
            AssertAnonymization(
                new LabelledStatement
                {
                    Label = "g",
                    Statement = AnyStatement
                },
                new LabelledStatement
                {
                    Label = "g", // not anonymized
                    Statement = AnyStatementAnonymized
                });
        }

        [Test]
        public void LabelledStatement_DefaultSafe()
        {
            AssertAnonymization(new LabelledStatement(), new LabelledStatement());
        }

        [Test]
        public void ReturnStatement()
        {
            AssertAnonymization(
                new ReturnStatement {Expression = AnyExpression},
                new ReturnStatement {Expression = AnyExpressionAnonymized});
        }

        [Test]
        public void ReturnStatement_DefaultSafe()
        {
            AssertAnonymization(new ReturnStatement(), new ReturnStatement());
        }

        [Test]
        public void ThrowStatement()
        {
            AssertAnonymization(
                new ThrowStatement {Reference = AnyVarReference},
                new ThrowStatement {Reference = AnyVarReferenceAnonymized});
        }

        [Test]
        public void ThrowStatement_DefaultSafe()
        {
            AssertAnonymization(new ThrowStatement(), new ThrowStatement());
        }

        [Test]
        public void VariableDeclaration()
        {
            AssertAnonymization(
                new VariableDeclaration
                {
                    Reference = new VariableReference
                    {
                        Identifier = "a"
                    }
                },
                new VariableDeclaration
                {
                    Reference = new VariableReference
                    {
                        Identifier = "a".ToHash()
                    }
                });
        }

        [Test]
        public void VariableDeclaration_DefaultSafe()
        {
            AssertAnonymization(new VariableDeclaration(), new VariableDeclaration());
        }

        [Test]
        public void UnknownStatement()
        {
            AssertAnonymization(new UnknownStatement(), new UnknownStatement());
        }
    }
}