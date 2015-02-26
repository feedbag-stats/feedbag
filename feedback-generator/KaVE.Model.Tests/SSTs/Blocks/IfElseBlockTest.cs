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
using KaVE.Model.SSTs;
using KaVE.Model.SSTs.Blocks;
using KaVE.Model.SSTs.Expressions;
using KaVE.Model.SSTs.Impl.Expressions.Simple;
using KaVE.Model.SSTs.Statements;
using NUnit.Framework;

namespace KaVE.Model.Tests.SSTs.Blocks
{
    internal class IfElseBlockTest
    {
        [Test]
        public void DefaultValues()
        {
            var sut = new IfElseBlock();
            Assert.Null(sut.Condition);
            Assert.NotNull(sut.Then);
            Assert.NotNull(sut.Else);
            Assert.AreNotEqual(0, sut.GetHashCode());
            Assert.AreNotEqual(1, sut.GetHashCode());
        }

        [Test]
        public void SettingValues()
        {
            var sut = new IfElseBlock {Condition = new ConstantValueExpression()};
            sut.Then.Add(new ReturnStatement());
            sut.Else.Add(new ContinueStatement());
            Assert.AreEqual(new ConstantValueExpression(), sut.Condition);
            Assert.AreEqual(Lists.NewList(new ReturnStatement()), sut.Then);
            Assert.AreEqual(Lists.NewList(new ContinueStatement()), sut.Else);
        }

        [Test]
        public void Equality_Default()
        {
            var a = new IfElseBlock();
            var b = new IfElseBlock();
            Assert.AreEqual(a, b);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_ReallyTheSame()
        {
            var a = new IfElseBlock {Condition = new ConstantValueExpression()};
            a.Then.Add(new ReturnStatement());
            a.Else.Add(new ContinueStatement());
            var b = new IfElseBlock {Condition = new ConstantValueExpression()};
            b.Then.Add(new ReturnStatement());
            b.Else.Add(new ContinueStatement());
            Assert.AreEqual(a, b);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_DifferentCondition()
        {
            var a = new IfElseBlock {Condition = new ConstantValueExpression()};
            var b = new IfElseBlock();
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_DifferentThen()
        {
            var a = new IfElseBlock();
            a.Then.Add(new ReturnStatement());
            var b = new IfElseBlock();
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void Equality_DifferentElse()
        {
            var a = new IfElseBlock();
            a.Else.Add(new ContinueStatement());
            var b = new IfElseBlock();
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }
    }
}