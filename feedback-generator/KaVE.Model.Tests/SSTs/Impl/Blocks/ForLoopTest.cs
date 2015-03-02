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

using KaVE.Model.Collections;
using KaVE.Model.SSTs.Blocks;
using KaVE.Model.SSTs.Impl.Blocks;
using KaVE.Model.SSTs.Impl.Expressions.Simple;
using KaVE.Model.SSTs.Impl.Statements;
using KaVE.Model.SSTs.Impl.Visitor;
using NUnit.Framework;

namespace KaVE.Model.Tests.SSTs.Impl.Blocks
{
    internal class ForLoopTest
    {
        [Test]
        public void DefaultValues()
        {
            var sut = new ForLoop();
            Assert.NotNull(sut.Init);
            Assert.AreEqual(0, sut.Init.Count);
            Assert.Null(sut.Condition);
            Assert.NotNull(sut.Step);
            Assert.AreEqual(0, sut.Step.Count);
            Assert.NotNull(sut.Body);
            Assert.AreEqual(0, sut.Body.Count);
            Assert.AreNotEqual(0, sut.GetHashCode());
            Assert.AreNotEqual(1, sut.GetHashCode());
        }

        [Test]
        public void SettingValues()
        {
            var sut = new ForLoop {Condition = new ConstantValueExpression()};
            sut.Init.Add(new GotoStatement());
            sut.Step.Add(new BreakStatement());
            sut.Body.Add(new ContinueStatement());

            Assert.AreEqual(new ConstantValueExpression(), sut.Condition);
            Assert.AreEqual(Lists.NewList(new GotoStatement()), sut.Init);
            Assert.AreEqual(Lists.NewList(new BreakStatement()), sut.Step);
            Assert.AreEqual(Lists.NewList(new ContinueStatement()), sut.Body);
        }

        [Test]
        public void Equality_Default()
        {
            var a = new ForLoop();
            var b = new ForLoop();
            Assert.AreEqual(a, b);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_ReallyTheSame()
        {
            var a = new ForLoop {Condition = new ConstantValueExpression()};
            a.Init.Add(new GotoStatement());
            a.Step.Add(new BreakStatement());
            a.Body.Add(new ContinueStatement());
            var b = new ForLoop {Condition = new ConstantValueExpression()};
            b.Init.Add(new GotoStatement());
            b.Step.Add(new BreakStatement());
            b.Body.Add(new ContinueStatement());
            Assert.AreEqual(a, b);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_DifferentCondition()
        {
            var a = new ForLoop {Condition = new ConstantValueExpression()};
            var b = new ForLoop();
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_DifferentInit()
        {
            var a = new ForLoop();
            a.Init.Add(new GotoStatement());
            var b = new ForLoop();
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_DifferentStep()
        {
            var a = new ForLoop();
            a.Step.Add(new BreakStatement());
            var b = new ForLoop();
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_DifferentBody()
        {
            var a = new ForLoop();
            a.Body.Add(new ContinueStatement());
            var b = new ForLoop();
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void VisitorIsImplemented()
        {
            var sut = new ForLoop();
            var visitor = new TestVisitor();
            sut.Accept(visitor, 5);

            Assert.AreEqual(sut, visitor.Statement);
            Assert.AreEqual(5, visitor.Context);
        }

        internal class TestVisitor : AbstractNodeVisitor<int>
        {
            public IForLoop Statement { get; private set; }
            public int Context { get; private set; }

            public override void Visit(IForLoop stmt, int context)
            {
                Statement = stmt;
                Context = context;
            }
        }
    }
}