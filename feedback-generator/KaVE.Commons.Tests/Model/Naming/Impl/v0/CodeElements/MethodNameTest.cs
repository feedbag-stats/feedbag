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

using System.Collections.Generic;
using KaVE.Commons.Model.Naming.Impl.v0.CodeElements;
using KaVE.Commons.Model.Naming.Impl.v0.Types;
using KaVE.Commons.Model.Naming.Types;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Model.Naming.Impl.v0.CodeElements
{
    internal class MethodNameTest
    {
        [Test]
        public void ShouldImplementIsUnknown()
        {
            Assert.That(new MethodName().IsUnknown);
        }

        [Test]
        public void ShouldBeSimpleMethod()
        {
            var methodName = new MethodName("[System.Void, mscore, 4.0.0.0] [T, P, 1.2.3.4].MethodName()");

            Assert.AreEqual("T, P, 1.2.3.4", methodName.DeclaringType.Identifier);
            Assert.AreEqual("System.Void, mscore, 4.0.0.0", methodName.ReturnType.Identifier);
            Assert.AreEqual("MethodName", methodName.Name);
            Assert.IsEmpty(methodName.TypeParameters);
            Assert.IsFalse(methodName.HasParameters);
            Assert.IsEmpty(methodName.Parameters);
        }

        [Test]
        public void ShouldBeMethodWithOneParameter()
        {
            const string declaringTypeIdentifier = "A, B, 9.9.9.9";
            const string returnTypeIdentifier = "R, C, 7.6.5.4";
            const string parameterIdentifier = "[P, D, 3.4.3.2] n";

            var methodName =
                new MethodName(
                    "[" + returnTypeIdentifier + "] [" + declaringTypeIdentifier + "].M(" + parameterIdentifier + ")");

            Assert.AreEqual(declaringTypeIdentifier, methodName.DeclaringType.Identifier);
            Assert.AreEqual(returnTypeIdentifier, methodName.ReturnType.Identifier);
            Assert.AreEqual("M", methodName.Name);
            Assert.IsFalse(methodName.HasTypeParameters);
            Assert.AreEqual(1, methodName.Parameters.Count);
            Assert.AreEqual(parameterIdentifier, methodName.Parameters[0].Identifier);
        }

        [Test]
        public void ShouldBeMethodWithMultipleParameters()
        {
            const string declaringTypeIdentifier = "A, B, 9.9.9.9";
            const string returnTypeIdentifier = "R, C, 7.6.5.4";
            const string param1Identifier = "[P, D, 3.4.3.2] n";
            const string param2Identifier = "[Q, E, 9.1.8.2] o";
            const string param3Identifier = "[R, F, 6.5.7.4] p";

            var methodName =
                new MethodName(
                    "[" + returnTypeIdentifier + "] [" + declaringTypeIdentifier + "].DoIt(" + param1Identifier + ", " +
                    param2Identifier + ", " + param3Identifier + ")");

            Assert.AreEqual(declaringTypeIdentifier, methodName.DeclaringType.Identifier);
            Assert.AreEqual(returnTypeIdentifier, methodName.ReturnType.Identifier);
            Assert.AreEqual("DoIt", methodName.Name);
            Assert.AreEqual(3, methodName.Parameters.Count);
            Assert.AreEqual(param1Identifier, methodName.Parameters[0].Identifier);
            Assert.AreEqual(param2Identifier, methodName.Parameters[1].Identifier);
            Assert.AreEqual(param3Identifier, methodName.Parameters[2].Identifier);
        }

        [Test]
        public void ShouldIncludeMethodNameInSignature()
        {
            var methodName = new MethodName("[Value, A, 0.0.0.1] [Declarator, A, 0.0.0.1].Method()");

            Assert.AreEqual("Method()", methodName.Signature);
        }

        [Test]
        public void ShouldIncludeParametersInSignature()
        {
            var methodName =
                new MethodName("[Value, A, 1.0.0.0] [Decl, B, 2.0.0.0].A(out [A, A, 1.0.0.0] p1, [B, B, 1.0.0.0] p2)");

            Assert.AreEqual("A(out [A, A, 1.0.0.0] p1, [B, B, 1.0.0.0] p2)", methodName.Signature);
        }

        [Test]
        public void ShouldIncludeTypeParametersInSignature()
        {
            var methodName = new MethodName("[R, R, 1.2.3.4] [D, D, 5.6.7.8].M`1[[T]]([T] p)");

            Assert.AreEqual("M`1[[T]]([T] p)", methodName.Signature);
        }

        [Test]
        public void SouldHaveNoTypeParameters()
        {
            var methodName = new MethodName("[Value, A, 0.0.0.1] [Declarator, A, 0.0.0.1].Method()");

            Assert.IsFalse(methodName.HasTypeParameters);
            Assert.AreEqual(0, methodName.TypeParameters.Count);
        }

        [Test]
        public void ShouldHaveTypeParameter()
        {
            var methodName = new MethodName("[Value, A, 0.0.0.1] [Declarator, A, 0.0.0.1].Method`1[[T]]()");

            Assert.IsTrue(methodName.HasTypeParameters);
        }

        [Test]
        public void ShouldHaveTwoUnboundTypeParameters()
        {
            var methodName = new MethodName("[T] [D, D, 4.5.6.7].M`2[[T],[O]]([O] p)");

            var expected = new ITypeName[] { new TypeParameterName("T"), new TypeParameterName("O") };
            Assert.AreEqual(expected, methodName.TypeParameters);
        }

        [Test]
        public void ShouldHaveBoundTypeParameter()
        {
            var methodName = new MethodName("[A] [D, D, 1.2.3.4].M`1[[A -> System.Int32, mscorlib, 4.0.0.0]]()");

            var expected = new[] {new TypeParameterName("A -> System.Int32, mscorlib, 4.0.0.0")};
            Assert.AreEqual(expected, methodName.TypeParameters);
        }

        [Test]
        public void ShouldNotConfuseGenericParameterTypesWithTypeParameters1()
        {
            var methodName = new MethodName("[R, R, 1.2.3.4] [D, D, 5.6.7.8].M([F`1[[T -> G, G, 5.3.2.1]]] p)");

            Assert.IsFalse(methodName.HasTypeParameters);
            Assert.AreEqual(0, methodName.TypeParameters.Count);
        }

        [Test]
        public void ShouldNotConfuseGenericParameterTypesWithTypeParameters2()
        {
            var method = new MethodName("[T, A, 1.0.0.0] [T, A, 1.0.0.0].M`1[[T]]([F`1[[U]], A, 1.0.0.0] p)");

            var expected = new List<ITypeName> {new TypeParameterName("T")};
            Assert.AreEqual(expected, method.TypeParameters);
        }

        [Test]
        public void ShouldExcludeTypeParametersFromName()
        {
            var methodName = new MethodName("[R] [D, D, 9.8.7.6].M`1[[T]]()");

            Assert.AreEqual("M", methodName.Name);
        }

        [Test]
        public void ShouldNotBeConstructor()
        {
            var methodName = new MethodName("[Value, A, 0.0.0.1] [Declarator, A, 0.0.0.1].Method()");

            Assert.IsFalse(methodName.IsConstructor);
        }

        [Test]
        public void ShouldBeConstructor()
        {
            var methodName = new MethodName("[MyType, A, 0.0.0.1] [MyType, A, 0.0.0.1]..ctor()");

            Assert.IsTrue(methodName.IsConstructor);
        }

        [Test]
        public void ShouldBeConstructor_Static()
        {
            var methodName = new MethodName("[MyType, A, 0.0.0.1] [MyType, A, 0.0.0.1]..cctor()");

            Assert.IsTrue(methodName.IsConstructor);
        }

        [Test]
        public void ShouldBeInstanceMethod()
        {
            var methodName = new MethodName("[I, A, 1.0.2.0] [K, K, 0.1.0.2].m()");

            Assert.IsFalse(methodName.IsStatic);
        }

        [Test]
        public void ShouldBeStaticMethod()
        {
            var methodName = new MethodName("static [I, A, 1.0.2.0] [K, K, 0.1.0.2].m()");

            Assert.IsTrue(methodName.IsStatic);
        }

        [Test]
        public void ShouldBeUnknownMethod()
        {
            Assert.AreSame(UnknownTypeName.Instance, new MethodName().ReturnType);
            Assert.AreSame(UnknownTypeName.Instance, new MethodName().DeclaringType);
            Assert.AreEqual("???", new MethodName().Name);
            Assert.IsFalse(new MethodName().HasParameters);
        }

        [Test]
        public void ShouldHandleDelegateReturnType()
        {
            var methodName = new MethodName("[d:[R,A] [D,A].()] [D,B].M([P,B] p)");

            Assert.AreEqual("M([P,B] p)", methodName.Signature);
            Assert.AreEqual("M", methodName.Name);
        }

        [Test]
        public void ShouldHandleDelegateParameterType()
        {
            var methodName = new MethodName("[R,B] [D,B].M([d:[R,A] [D,A].()] p)");

            Assert.AreEqual("M([d:[R,A] [D,A].()] p)", methodName.Signature);
            Assert.AreEqual("M", methodName.Name);
        }

        [Test]
        public void ShouldBeExtensionMethod()
        {
            const string id = "static [T,P] [T,P].M(this [T,P] o)";
            var sut = new MethodName(id);
            Assert.True(sut.IsExtensionMethod);
        }

        [Test]
        public void ShouldBeExtensionMethod_NotIfNotStatic()
        {
            const string id = "[T,P] [T,P].M(this [T,P] o)";
            var sut = new MethodName(id);
            Assert.False(sut.IsExtensionMethod);
        }

        [Test]
        public void ShouldBeExtensionMethod_NotWithoutParameters()
        {
            const string id = "static [T,P] [T,P].M()";
            var sut = new MethodName(id);
            Assert.False(sut.IsExtensionMethod);
        }

        [Test]
        public void ShouldBeExtensionMethod_NotWithoutThisMarker()
        {
            const string id = "static [T,P] [T,P].M([T,P] o)";
            var sut = new MethodName(id);
            Assert.False(sut.IsExtensionMethod);
        }
    }
}