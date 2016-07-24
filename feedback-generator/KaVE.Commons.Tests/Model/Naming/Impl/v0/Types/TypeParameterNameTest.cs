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
using KaVE.Commons.Model.Naming.Impl.v0.Types;
using KaVE.Commons.Model.Naming.Impl.v0.Types.Organization;
using KaVE.Commons.Model.Naming.Types;
using KaVE.Commons.Utils;
using KaVE.Commons.Utils.Assertion;
using KaVE.Commons.Utils.Collections;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Model.Naming.Impl.v0.Types
{
    internal class TypeParameterNameTest
    {
        [Test, ExpectedException(typeof(AssertException))]
        public void CannotBeInitializedAsUnknownType()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new TypeParameterName("?");
            Assert.IsFalse(TypeParameterName.IsTypeParameterIdentifier("?"));
        }

        [TestCaseSource("TypeParametersSource")]
        public void ShouldIdentifyValidTypeParameterNames(string typeParameter, string shortName, string boundType)
        {
            Assert.IsTrue(TypeParameterName.IsTypeParameterIdentifier(typeParameter));
            Assert.IsFalse(TypeUtils.IsDelegateTypeIdentifier(typeParameter));
            Assert.IsFalse(ArrayTypeName.IsArrayTypeNameIdentifier(typeParameter));
        }

        [TestCaseSource("TypeParametersSource")]
        public void ShouldParseShortName(string typeParameter, string shortName, string boundType)
        {
            var sut = TypeUtils.CreateTypeName(typeParameter).AsTypeParameterName;
            Assert.AreEqual(shortName, sut.TypeParameterShortName);
        }

        [TestCaseSource("TypeParametersSource")]
        public void ShouldParseTypeParameterType(string typeParameter, string shortName, string boundType)
        {
            var sut = TypeUtils.CreateTypeName(typeParameter).AsTypeParameterName;
            Assert.AreEqual(boundType, sut.TypeParameterType.Identifier);
        }

        [TestCaseSource("TypeParametersSource")]
        public void ShouldParseShortAndFullName(string typeParameter, string shortName, string boundType)
        {
            foreach (var expectedShortName in new[] {"T[,]", "T[]", "T"})
            {
                if (typeParameter.StartsWith(expectedShortName))
                {
                    var sut = TypeUtils.CreateTypeName(typeParameter).AsTypeParameterName;
                    Assert.AreEqual(expectedShortName, sut.Name);
                    Assert.AreEqual(expectedShortName, sut.FullName);
                    return;
                }
            }
            Assert.Fail();
        }

        [TestCaseSource("TypeParametersSource")]
        public void ShouldParseArrayRank(string typeParameter, string shortName, string boundType)
        {
            if (typeParameter.StartsWith("T[,]"))
            {
                var sut = TypeUtils.CreateTypeName(typeParameter).AsArrayTypeName;
                Assert.AreEqual(2, sut.Rank);
            }
            if (typeParameter.StartsWith("T[]"))
            {
                var sut = TypeUtils.CreateTypeName(typeParameter).AsArrayTypeName;
                Assert.AreEqual(1, sut.Rank);
            }
        }

        [TestCaseSource("TypeParametersSource")]
        public void ShouldParseArrayBaseType(string typeParameter, string shortName, string boundType)
        {
            var isBound = typeParameter.Contains(" -> ");
            var baseType = isBound
                ? TypeUtils.CreateTypeName("T -> {0}".FormatEx(boundType))
                : TypeUtils.CreateTypeName("T");
            foreach (var tp in new[] {"T[,]", "T[]"})
            {
                if (typeParameter.StartsWith(tp))
                {
                    var sut = TypeUtils.CreateTypeName(typeParameter).AsArrayTypeName;
                    Assert.AreEqual(baseType, sut.ArrayBaseType);
                }
            }
        }

        [TestCaseSource("TypeParametersSource")]
        public void AllChecksShouldBeFalse(string typeParameter, string shortName, string boundType)
        {
            var sut = TypeUtils.CreateTypeName(typeParameter).AsTypeParameterName;

            Assert.IsFalse(sut.IsUnknown);
            Assert.IsFalse(sut.IsHashed);

            Assert.IsFalse(sut.HasTypeParameters);
            Assert.IsFalse(sut.IsClassType);
            Assert.IsFalse(sut.IsDelegateType);
            Assert.IsFalse(sut.IsEnumType);
            Assert.IsFalse(sut.IsInterfaceType);
            Assert.IsFalse(sut.IsNestedType);
            Assert.IsFalse(sut.IsNullableType);
            if (!sut.IsArray)
            {
                Assert.IsFalse(sut.IsReferenceType);
            }
            Assert.IsFalse(sut.IsSimpleType);
            Assert.IsFalse(sut.IsStructType);
            Assert.IsTrue(sut.IsTypeParameter);
            Assert.IsFalse(sut.IsValueType);
            Assert.IsFalse(sut.IsVoidType);

            Assert.AreEqual(Lists.NewList<ITypeParameterName>(), sut.TypeParameters);

            Assert.AreEqual(new AssemblyName(), sut.Assembly);
            Assert.AreEqual(new NamespaceName(), sut.Namespace);
            Assert.Null(sut.DeclaringType);
        }

        public string[] ValidTypeParameterNamesSource()
        {
            return new[] {"T", "T[]", "T[,]"};
        }

        public string[] Types()
        {
            return new[] {"?", "T", "T,P", "s:n.S,P", "i:n:I,P", "n.T+T2,P", "n.T`1[[T -> ?]]", "d:[?] [?].()", "T[],P"};
        }

        public IEnumerable<string[]> TypeParametersSource()
        {
            // typeParameter, shortName, boundType
            ISet<string[]> tps = new HashSet<string[]>();
            foreach (var tp in ValidTypeParameterNamesSource())
            {
                tps.Add(new[] {tp, tp, "?"});

                foreach (var type in Types())
                {
                    tps.Add(new[] {"{0} -> {1}".FormatEx(tp, type), tp, type});
                }
            }
            return tps;
        }

        [Test]
        public void ShouldParseIsBound()
        {
            Assert.IsFalse(new TypeParameterName("T").IsBound);
            Assert.IsTrue(new TypeParameterName("T -> ?").IsBound);
        }

        [TestCaseSource("ValidTypeParamIds")]
        public void IsTypeParameterId(string typeId)
        {
            Assert.IsTrue(TypeParameterName.IsTypeParameterIdentifier(typeId));
        }

        [TestCaseSource("InvalidTypeParamIds")]
        public void IsNoTypeParameterId(string typeId)
        {
            Assert.IsFalse(TypeParameterName.IsTypeParameterIdentifier(typeId));
        }

        internal static string[] ValidTypeParamIds()
        {
            return new[]
            {
                "T",
                "t",
                "T1",
                "T -> ?",
                "T -> T,P",
                "T[] -> T,P",
                "T[,] -> T,P"
            };
        }

        internal static string[] InvalidTypeParamIds()
        {
            return new[]
            {
                "?",
                "1",
                "a-z",
                " ",
                ",",
                "(",
                ")",
                "[",
                "]",
                "<",
                ">",
                "{",
                "}"
            };
        }
    }
}