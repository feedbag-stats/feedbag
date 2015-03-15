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

using System.Linq;
using KaVE.Model.Collections;
using KaVE.Model.Names;
using KaVE.Model.Names.CSharp;
using KaVE.Model.Names.CSharp.MemberNames;
using KaVE.Model.SSTs.Expressions;
using KaVE.Model.SSTs.Impl;
using KaVE.Model.SSTs.Impl.Blocks;
using KaVE.Model.SSTs.Impl.Declarations;
using KaVE.Model.SSTs.Impl.Expressions.Assignable;
using KaVE.Model.SSTs.Impl.Expressions.Simple;
using KaVE.Model.SSTs.Impl.References;
using KaVE.Model.SSTs.Impl.Statements;
using KaVE.Model.SSTs.References;
using KaVE.Utils.Assertion;
using NUnit.Framework;

namespace KaVE.Model.Tests.SSTs.Impl
{
    public class SSTUtilTest
    {
        [Test]
        public void Declare()
        {
            var actual = SSTUtil.Declare("a", TypeName.UnknownName);
            var expected = new VariableDeclaration
            {
                Reference = Ref("a"),
                Type = TypeName.UnknownName
            };
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Return()
        {
            var actual = SSTUtil.Return(new ConstantValueExpression());
            var expected = new ReturnStatement
            {
                Expression = new ConstantValueExpression()
            };
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReturnVariable()
        {
            var actual = SSTUtil.ReturnVariable("a");
            var expected = new ReturnStatement
            {
                Expression = new ReferenceExpression {Reference = new VariableReference {Identifier = "a"}}
            };
            Assert.AreEqual(expected, actual);
        }

        private static IVariableReference Ref(string id)
        {
            return new VariableReference {Identifier = id};
        }


        [Test]
        public void ReferenceExprToVariable()
        {
            var actual = SSTUtil.ReferenceExprToVariable("a");
            var expected = new ReferenceExpression
            {
                Reference = new VariableReference {Identifier = "a"}
            };
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ComposedExpression()
        {
            var actual = SSTUtil.ComposedExpression("a", "b");
            var expected = new ComposedExpression {References = Lists.NewList(Ref("a"), Ref("b"))};
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SettingValues()
        {
            var a = SSTUtil.InvocationExpression("a1", GetMethod("A2"), VarRefExpr("a3"));
            Assert.AreEqual(new VariableReference {Identifier = "a1"}, a.Reference);
            Assert.AreEqual(GetMethod("A2"), a.MethodName);
            Assert.AreEqual(Refs("a3"), a.Parameters);
        }

        [Test]
        public void InvocationExpression_Static()
        {
            var a = SSTUtil.InvocationExpression(GetStaticMethod("B2"), VarRefExpr("c2"));
            Assert.AreEqual(new VariableReference(), a.Reference);
            Assert.AreEqual(GetStaticMethod("B2"), a.MethodName);
            Assert.AreEqual(Refs("c2"), a.Parameters);
        }

        [Test]
        public void InvocationExpression_NonStatic()
        {
            var a = SSTUtil.InvocationExpression("a1", GetMethod("B1"), VarRefExpr("c1"));
            Assert.AreEqual(SSTUtil.VariableReference("a1"), a.Reference);
            Assert.AreEqual(GetMethod("B1"), a.MethodName);
            Assert.AreEqual(Refs("c1"), a.Parameters);
        }

        [Test]
        public void InvocationStatement_Static()
        {
            var actual = SSTUtil.InvocationStatement(GetStaticMethod("B2"), VarRefExpr("c2"));
            var expected = new ExpressionStatement
            {
                Expression = new InvocationExpression
                {
                    MethodName = GetStaticMethod("B2"),
                    Parameters = {VarRefExpr("c2")}
                }
            };
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void InvocationStatement_NonStatic()
        {
            var actual = SSTUtil.InvocationStatement("a", GetMethod("B2"), VarRefExpr("c2"));
            var expected = new ExpressionStatement
            {
                Expression = new InvocationExpression
                {
                    Reference = new VariableReference {Identifier = "a"},
                    MethodName = GetMethod("B2"),
                    Parameters = {VarRefExpr("c2")}
                }
            };
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void LockBlock()
        {
            var actual = SSTUtil.LockBlock("a");
            var expected = new LockBlock
            {
                Reference = new VariableReference {Identifier = "a"}
            };
            Assert.AreEqual(expected, actual);
        }

        [Test, ExpectedException(typeof (AssertException))]
        public void CustomConstructor_NonStaticAssert()
        {
            SSTUtil.InvocationExpression("a1", GetStaticMethod("B1"), VarRefExpr("c1"));
        }

        [Test, ExpectedException(typeof (AssertException))]
        public void CustomConstructor_StaticAssert()
        {
            SSTUtil.InvocationExpression(GetMethod("B2"), VarRefExpr("c2"));
        }

        private static IMethodName GetMethod(string simpleName)
        {
            var methodName = string.Format(
                "[System.String, mscore, 4.0.0.0] [System.String, mscore, 4.0.0.0].{0}()",
                simpleName);
            return MethodName.Get(methodName);
        }

        private static IMethodName GetStaticMethod(string simpleName)
        {
            var methodName = string.Format(
                "static [System.String, mscore, 4.0.0.0] [System.String, mscore, 4.0.0.0].{0}()",
                simpleName);
            return MethodName.Get(methodName);
        }

        private static ISimpleExpression VarRefExpr(string id)
        {
            return new ReferenceExpression {Reference = new VariableReference {Identifier = id}};
        }

        private static ISimpleExpression[] Refs(params string[] ids)
        {
            return
                ids.Select<string, ISimpleExpression>(
                    id => new ReferenceExpression {Reference = new VariableReference {Identifier = id}}).ToArray();
        }
    }
}