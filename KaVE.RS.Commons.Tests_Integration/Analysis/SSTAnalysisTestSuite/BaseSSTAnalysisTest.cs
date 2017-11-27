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

using System;
using System.Collections.Generic;
using System.Linq;
using KaVE.Commons.Model.Naming;
using KaVE.Commons.Model.Naming.CodeElements;
using KaVE.Commons.Model.Naming.Types;
using KaVE.Commons.Model.SSTs;
using KaVE.Commons.Model.SSTs.Declarations;
using KaVE.Commons.Model.SSTs.Expressions;
using KaVE.Commons.Model.SSTs.Expressions.Simple;
using KaVE.Commons.Model.SSTs.Impl;
using KaVE.Commons.Model.SSTs.Impl.Declarations;
using KaVE.Commons.Model.SSTs.Impl.Expressions.Assignable;
using KaVE.Commons.Model.SSTs.Impl.Expressions.Simple;
using KaVE.Commons.Model.SSTs.Impl.References;
using KaVE.Commons.Model.SSTs.Impl.Statements;
using KaVE.Commons.Model.SSTs.References;
using KaVE.Commons.Model.SSTs.Statements;
using KaVE.Commons.Utils.Assertion;
using KaVE.Commons.Utils.Collections;
using KaVE.Commons.Utils.Exceptions;
using KaVE.RS.Commons.Analysis.CompletionTarget;
using KaVE.VS.Commons;
using NUnit.Framework;
using JB = JetBrains.ReSharper.Psi.CSharp.Tree;
using Fix = KaVE.RS.Commons.Tests_Integration.Analysis.SSTAnalysisTestSuite.SSTAnalysisFixture;

namespace KaVE.RS.Commons.Tests_Integration.Analysis.SSTAnalysisTestSuite
{
    internal abstract class BaseSSTAnalysisTest : BaseCSharpCodeCompletionTest
    {
        internal readonly IList<string> Log = new List<string>();

        [SetUp]
        public void RegisterLogger()
        {
            var logger = new KaVE.Commons.TestUtils.Utils.Exceptions.TestLogger(false);
            logger.InfoLogged += Log.Add;
            Registry.RegisterComponent<ILogger>(logger);
        }

        [TearDown]
        public void ClearRegistry()
        {
            Registry.Clear();
            var logCopy = new List<string>(Log);
            Log.Clear();
            Assert.IsEmpty(logCopy);
            TestAnalysisTrigger.IsPrintingType = false;
        }

        protected SST NewSST()
        {
            return new SST
            {
                EnclosingType = Names.Type("N.C, TestProject")
            };
        }

        protected MethodDeclaration NewMethodDeclaration(ITypeName returnType, string simpleName)
        {
            return NewMethodDeclaration(returnType, simpleName, new string[0]);
        }

        protected MethodDeclaration NewMethodDeclaration(ITypeName returnType, string simpleName, params string[] args)
        {
            const string package = "N.C, TestProject";
            return new MethodDeclaration
            {
                Name = Names.Method(
                    "[{0}] [{1}].{2}({3})",
                    returnType,
                    package,
                    simpleName,
                    string.Join(", ", args)),
                IsEntryPoint = true
            };
        }

        protected void AssertResult(SST expected)
        {
            Assert.AreEqual(expected, ResultSST);
        }

        protected void AssertMethod(IMethodDeclaration expected)
        {
            if (Enumerable.Contains(ResultSST.Methods, expected))
            {
                return;
            }
            Assert.Fail("method not found");
        }


        protected IEnumerable<ISimpleExpression> RefExprs(params string[] ids)
        {
            return Lists.NewListFrom(ids.Select(RefExpr));
        }

        protected static ISimpleExpression RefExpr(string id)
        {
            return new ReferenceExpression {Reference = new VariableReference {Identifier = id}};
        }

        protected ISimpleExpression RefExpr(IReference reference)
        {
            return new ReferenceExpression {Reference = reference};
        }

        #region custom asserts

        protected void AssertAllMethods(params IMethodDeclaration[] expectedDecls)
        {
            var actualDecls = Lists.NewListFrom(ResultSST.Methods);
            if (expectedDecls.Length != actualDecls.Count)
            {
                Console.WriteLine("\nexpected {0} declarations, but was:\n", expectedDecls.Length);
                foreach (var m in actualDecls)
                {
                    Console.WriteLine("-----");
                    Console.WriteLine(m);
                }
                Assert.Fail("incorrect number of method declarations");
            }

            foreach (var expectedDecl in expectedDecls)
            {
                if (!actualDecls.Contains(expectedDecl))
                {
                    Console.WriteLine("\nexpected:\n");
                    Console.WriteLine(expectedDecl);
                    Console.WriteLine("\nbut was:\n");
                    foreach (var m in actualDecls)
                    {
                        Console.WriteLine("-----");
                        Console.WriteLine(m);
                    }
                    Assert.Fail("expected method not found in actual list of method declarations");
                }
            }
        }

        protected void AssertBody(IMethodDeclaration method, IKaVEList<IStatement> body)
        {
            if (!body.Equals(method.Body))
            {
                Console.WriteLine("AssertBody failed!");
                Console.WriteLine("\n-- expected body --\n");
                Console.WriteLine(body.ToString());
                Console.WriteLine("\n-- actual body --\n");
                Console.WriteLine(method.Body.ToString());
                Assert.Fail();
            }
        }

        protected void AssertBody(IKaVEList<IStatement> body)
        {
            Assert.AreEqual(1, ResultSST.Methods.Count);
            var m = ResultSST.Methods.First();
            AssertBody(m, body);
        }

        protected void AssertBody(params IStatement[] bodyArr)
        {
            AssertBody(Lists.NewListFrom(bodyArr));
        }

        protected void AssertBody(string methodName, params IStatement[] bodyArr)
        {
            AssertBody(ResultSST.Methods.Single(m => m.Name.Name == methodName), Lists.NewListFrom(bodyArr));
        }

        protected static void AssertNodeIsMethodDeclaration(string simpleMethodName, JB.ICSharpTreeNode node)
        {
            var decl = node as JB.IMethodDeclaration;
            Assert.NotNull(decl);
            Assert.AreEqual(simpleMethodName, decl.NameIdentifier.Name);
        }

        protected static void AssertNodeIsVariableDeclaration(string varName, JB.ICSharpTreeNode node)
        {
            var decl = node as JB.ILocalVariableDeclaration;
            Assert.NotNull(decl);
            Assert.AreEqual(varName, decl.NameIdentifier.Name);
        }

        protected static void AssertNodeIsReference(string refName, JB.ICSharpTreeNode node)
        {
            var expr = node as JB.IReferenceExpression;
            Assert.NotNull(expr);
            Assert.AreEqual(refName, expr.NameIdentifier.Name);
        }

        protected static void AssertNodeIsAssignment(string varName, JB.ICSharpTreeNode node)
        {
            var ass = node as JB.IAssignmentExpression;
            Assert.NotNull(ass);
            var dest = ass.Dest as JB.IReferenceExpression;
            Assert.NotNull(dest);
            Assert.AreEqual(varName, dest.NameIdentifier.Name);
        }

        protected void AssertCompletionCase(CompletionCase expectedCase)
        {
            Assert.AreEqual(expectedCase, LastCompletionMarker.Case);
        }

        protected void AssertNodeIsIf(JB.ICSharpTreeNode node)
        {
            Assert.True(node is JB.IIfStatement);
        }

        protected void AssertNodeIsCall(string expectedName, JB.ICSharpTreeNode node)
        {
            var call = node as JB.IInvocationExpression;
            Assert.NotNull(call);
            var actualName = call.InvocationExpressionReference.GetName();
            Assert.AreEqual(expectedName, actualName);
        }

        protected void AssertCompletionMarker<TNodeType>(CompletionCase expectedCase)
        {
            var node = LastCompletionMarker.AffectedNode;
            if (!(node is TNodeType))
            {
                Assert.Fail("expected {0}, but was {1}", typeof(TNodeType), node.GetType());
            }
            Assert.AreEqual(expectedCase, LastCompletionMarker.Case);
        }

        #endregion

        #region instantiation helper

        protected static IVariableReference VarRef(string id = "")
        {
            return new VariableReference {Identifier = id};
        }

        protected static PropertyReference PropRef(string name, ITypeName type, string target = "this")
        {
            var propertyName = Names.Property("set get " + MemberName(name, type) + "()");
            return new PropertyReference {Reference = VarRef(target), PropertyName = propertyName};
        }

        protected static FieldReference FieldRef(string name, ITypeName type, string target = "this")
        {
            var fieldName = Names.Field(MemberName(name, type));
            return new FieldReference {Reference = VarRef(target), FieldName = fieldName};
        }

        protected static EventReference EventRef(string name, ITypeName type, string target = "this")
        {
            var eventName = Names.Event(MemberName(name, type));
            return new EventReference {Reference = VarRef(target), EventName = eventName};
        }

        private static string MemberName(string name, ITypeName type)
        {
            return string.Format("[{0}] [N.C, TestProject].{1}", type, name);
        }

        protected static FieldReference FieldRef(IFieldName fieldName, IVariableReference declTypeRef)
        {
            return new FieldReference {FieldName = fieldName, Reference = declTypeRef};
        }

        protected static EventReference EventRef(IEventName eventName, IVariableReference declTypeRef)
        {
            return new EventReference {EventName = eventName, Reference = declTypeRef};
        }

        protected static PropertyReference PropertyRef(IPropertyName propertyName, IVariableReference declTypeRef)
        {
            return new PropertyReference {PropertyName = propertyName, Reference = declTypeRef};
        }

        protected static MethodReference MethodRef(IMethodName methodName, IVariableReference declTypeRef)
        {
            return new MethodReference {MethodName = methodName, Reference = declTypeRef};
        }

        protected static MethodReference MethodRef(string name,
            ITypeName retType,
            ITypeName declType,
            string varRef = "this")
        {
            return MethodRef(Method("[{0}] [{1}].{2}()", retType, declType, name), VarRef(varRef));
        }

        protected IVariableDeclaration VarDecl(string varName, ITypeName type)
        {
            return new VariableDeclaration
            {
                Reference = VarRef(varName),
                Type = type
            };
        }

        protected static IAssignment Assign(string id, IAssignableExpression expr)
        {
            return new Assignment
            {
                Reference = VarRef(id),
                Expression = expr
            };
        }

        protected static IAssignment Assign(IAssignableReference reference, IAssignableExpression expr)
        {
            return new Assignment {Reference = reference, Expression = expr};
        }

        protected static IConstantValueExpression Const(string v)
        {
            return new ConstantValueExpression {Value = v};
        }

        protected static IStatement InvokeStmt(string id, IMethodName methodName, params ISimpleExpression[] parameters)
        {
            Asserts.That(!methodName.IsStatic);
            return ExprStmt(
                new InvocationExpression
                {
                    Reference = VarRef(id),
                    MethodName = methodName,
                    Parameters = Lists.NewList(parameters)
                });
        }

        protected static IStatement InvokeStaticStmt(IMethodName methodName, params ISimpleExpression[] parameters)
        {
            Asserts.That(methodName.IsStatic);
            return ExprStmt(
                new InvocationExpression
                {
                    MethodName = methodName,
                    Parameters = Lists.NewList(parameters)
                });
        }

        protected static IExpressionStatement ExprStmt(IAssignableExpression expr)
        {
            return new ExpressionStatement
            {
                Expression = expr
            };
        }

        public static InvocationExpression Invoke(string id,
            IMethodName methodName,
            params ISimpleExpression[] parameters)
        {
            Assert.False(methodName.IsStatic);
            return new InvocationExpression
            {
                Reference = VarRef(id),
                MethodName = methodName,
                Parameters = Lists.NewList(parameters)
            };
        }

        public static InvocationExpression InvokeStatic(IMethodName methodName, params ISimpleExpression[] parameters)
        {
            Assert.True(methodName.IsStatic);
            return new InvocationExpression
            {
                MethodName = methodName,
                Parameters = Lists.NewList(parameters)
            };
        }

        protected InvocationExpression InvokeCtor(IMethodName methodName, params ISimpleExpression[] parameters)
        {
            Assert.That(methodName.IsConstructor);
            return new InvocationExpression
            {
                MethodName = methodName,
                Parameters = Lists.NewList(parameters)
            };
        }

        protected static IMethodName Method(string methodDef, params object[] args)
        {
            return Names.Method(methodDef, args);
        }

        protected static MethodDeclaration ConstructorDecl(string type, params IParameterName[] parameters)
        {
            return new MethodDeclaration
            {
                Name = Constructor(type, parameters),
                IsEntryPoint = true
            };
        }

        protected static IMethodName Constructor(string type, params IParameterName[] parameters)
        {
            var paramList = string.Join<IParameterName>(",", parameters);
            return Method("[{0}] [{1}]..ctor({2})", Fix.Void, type, paramList);
        }

        protected static IParameterName Param(ITypeName type, string name)
        {
            return Names.Parameter("[{0}] {1}", type, name);
        }

        protected static ITypeName Type(string shortName)
        {
            return Names.Type("N.{0}, TestProject", shortName);
        }

        protected static IAssignableExpression ComposedExpr(params string[] ids)
        {
            return new ComposedExpression
            {
                References = Lists.NewListFrom(ids.Select(VarRef))
            };
        }

        #endregion
    }
}