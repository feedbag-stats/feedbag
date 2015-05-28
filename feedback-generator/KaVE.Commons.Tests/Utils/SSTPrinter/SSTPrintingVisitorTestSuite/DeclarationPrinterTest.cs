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
 *    - Andreas Bauer
 */

using KaVE.Commons.Model.Names.CSharp;
using KaVE.Commons.Model.SSTs.Impl;
using KaVE.Commons.Model.SSTs.Impl.Declarations;
using KaVE.Commons.Model.SSTs.Impl.Statements;
using KaVE.Commons.Model.TypeShapes;
using KaVE.Commons.Utils.SSTPrinter;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Utils.SSTPrinter.SSTPrintingVisitorTestSuite
{
    internal class DeclarationPrinterTest : SSTPrintingVisitorTestBase
    {
        [Test]
        public void SSTDeclaration_EmptyClass()
        {
            var sst = new SST {EnclosingType = TypeName.Get("TestClass,TestProject")};

            AssertPrint(
                sst,
                "class TestClass",
                "{",
                "}");
        }

        [Test]
        public void SSTDeclaration_WithSupertypes()
        {
            var thisType = TypeName.Get("TestClass,P");
            var superType = TypeName.Get("SuperClass,P");
            var interface1 = TypeName.Get("i:IDoesSomething,P");
            var interface2 = TypeName.Get("i:IDoesSomethingElse,P");

            var sst = new SST {EnclosingType = thisType};
            var typeShape = new TypeShape
            {
                TypeHierarchy = new TypeHierarchy
                {
                    Element = thisType,
                    Extends = new TypeHierarchy {Element = superType},
                    Implements =
                    {
                        new TypeHierarchy {Element = interface1},
                        new TypeHierarchy {Element = interface2}
                    }
                }
            };

            AssertPrintSingle(
                sst,
                new SSTPrintingContext {TypeShape = typeShape},
                "class TestClass : SuperClass, IDoesSomething, IDoesSomethingElse",
                "{",
                "}");
        }

        [Test]
        public void SSTDeclaration_WithSupertypes_OnlyInterface()
        {
            var thisType = TypeName.Get("TestClass,P");
            var interface1 = TypeName.Get("i:IDoesSomething,P");

            var sst = new SST {EnclosingType = thisType};
            var typeShape = new TypeShape
            {
                TypeHierarchy = new TypeHierarchy
                {
                    Element = thisType,
                    Implements =
                    {
                        new TypeHierarchy {Element = interface1},
                    }
                }
            };

            AssertPrintSingle(
                sst,
                new SSTPrintingContext {TypeShape = typeShape},
                "class TestClass : IDoesSomething",
                "{",
                "}");
        }

        [Test]
        public void SSTDeclaration_WithSupertypes_OnlySuperclass()
        {
            var thisType = TypeName.Get("TestClass,P");
            var superType = TypeName.Get("SuperClass,P");

            var sst = new SST {EnclosingType = thisType};
            var typeShape = new TypeShape
            {
                TypeHierarchy = new TypeHierarchy
                {
                    Element = thisType,
                    Extends = new TypeHierarchy {Element = superType}
                }
            };

            AssertPrintSingle(
                sst,
                new SSTPrintingContext {TypeShape = typeShape},
                "class TestClass : SuperClass",
                "{",
                "}");
        }

        [Test]
        public void SSTDeclaration_FullClass()
        {
            var sst = new SST
            {
                EnclosingType = TypeName.Get("TestClass,P"),
                Delegates = {new DelegateDeclaration {Name = TypeName.Get("d:TestDelegate,P")}},
                Events = {new EventDeclaration {Name = EventName.Get("[EventType,P] [TestClass,P].SomethingHappened")}},
                Fields =
                {
                    new FieldDeclaration {Name = FieldName.Get("[FieldType,P] [TestClass,P].SomeField")},
                    new FieldDeclaration {Name = FieldName.Get("[FieldType,P] [TestClass,P].AnotherField")}
                },
                Properties =
                {
                    new PropertyDeclaration
                    {
                        Name = PropertyName.Get("get set [PropertyType,P] [TestClass,P].SomeProperty")
                    }
                },
                Methods =
                {
                    new MethodDeclaration {Name = MethodName.Get("[ReturnType,P] [TestClass,P].M([ParameterType,P] p)")},
                    new MethodDeclaration
                    {
                        Name = MethodName.Get("[ReturnType,P] [TestClass,P].M2()"),
                        Body = {new BreakStatement()}
                    }
                }
            };

            AssertPrint(
                sst,
                "class TestClass",
                "{",
                "    delegate TestDelegate();",
                "",
                "    event EventType SomethingHappened;",
                "",
                "    FieldType SomeField;",
                "    FieldType AnotherField;",
                "",
                "    PropertyType SomeProperty { get; set; }",
                "",
                "    ReturnType M(ParameterType p) { }",
                "",
                "    ReturnType M2()",
                "    {",
                "        break;",
                "    }",
                "}");
        }

        [Test]
        public void SSTDeclaration_Interface()
        {
            var sst = new SST {EnclosingType = TypeName.Get("i:SomeInterface,P")};

            AssertPrint(
                sst,
                "interface SomeInterface",
                "{",
                "}");
        }

        [Test]
        public void SSTDeclaration_Struct()
        {
            var sst = new SST {EnclosingType = TypeName.Get("s:SomeStruct,P")};

            AssertPrint(
                sst,
                "struct SomeStruct",
                "{",
                "}");
        }

        [Test]
        public void SSTDeclaration_Enum()
        {
            var sst = new SST {EnclosingType = TypeName.Get("e:SomeEnum,P")};

            AssertPrint(
                sst,
                "enum SomeEnum",
                "{",
                "}");
        }

        // TODO: Test may be removed after DelegateTypeName has been officially added to DelegateDeclaration
        [Test]
        public void DelegateDeclaration_Legacy()
        {
            var sst = new DelegateDeclaration {Name = TypeName.Get("d:T,P")};
            AssertPrint(sst, "delegate T();");
        }

        [Test]
        public void DelegateDeclaration_Parameterless()
        {
            var sst = new DelegateDeclaration {Name = DelegateTypeName.Get("d:[R, P] [Some.DelegateType, P].()")};
            AssertPrint(sst, "delegate DelegateType();");
        }

        [Test]
        public void DelegateDeclaration_WithParameters()
        {
            var sst = new DelegateDeclaration
            {
                Name = DelegateTypeName.Get("d:[R, P] [Some.DelegateType, P].([C, P] p1, [D, P] p2)")
            };

            AssertPrint(sst, "delegate DelegateType(C p1, D p2);");
        }

        [Test]
        public void EventDeclaration()
        {
            var sst = new EventDeclaration
            {
                Name = EventName.Get("[EventType,P] [DeclaringType,P].E")
            };

            AssertPrint(sst, "event EventType E;");
        }

        [Test]
        public void EventDeclaration_GenericEventArgsType()
        {
            var sst = new EventDeclaration
            {
                Name = EventName.Get("[EventType`1[[T -> EventArgsType,P]],P] [DeclaringType,P].E")
            };

            AssertPrint(sst, "event EventType<EventArgsType> E;");
        }

        [Test]
        public void FieldDeclaration()
        {
            var sst = new FieldDeclaration
            {
                Name = FieldName.Get("[FieldType,P] [DeclaringType,P].F")
            };

            AssertPrint(sst, "FieldType F;");
        }

        [Test]
        public void FieldDeclaration_Static()
        {
            var sst = new FieldDeclaration
            {
                Name = FieldName.Get("static [FieldType,P] [DeclaringType,P].F")
            };

            AssertPrint(sst, "static FieldType F;");
        }

        [Test]
        public void FieldDeclaration_Array()
        {
            var sst = new FieldDeclaration{Name = FieldName.Get("[d:[V, A] [N.TD, A].()[]] [DT, A]._delegatesField")};

            AssertPrint(sst, "TD[] _delegatesField;");
        }

        [Test]
        public void PropertyDeclaration_GetterOnly()
        {
            var sst = new PropertyDeclaration
            {
                Name = PropertyName.Get("get [PropertyType,P] [DeclaringType,P].P")
            };

            AssertPrint(sst, "PropertyType P { get; }");
        }

        [Test]
        public void PropertyDeclaration_SetterOnly()
        {
            var sst = new PropertyDeclaration
            {
                Name = PropertyName.Get("set [PropertyType,P] [DeclaringType,P].P")
            };

            AssertPrint(sst, "PropertyType P { set; }");
        }

        [Test]
        public void PropertyDeclaration()
        {
            var sst = new PropertyDeclaration
            {
                Name = PropertyName.Get("get set [PropertyType,P] [DeclaringType,P].P"),
            };

            AssertPrint(sst, "PropertyType P { get; set; }");
        }

        [Test]
        public void PropertyDeclaration_WithBodies()
        {
            var sst = new PropertyDeclaration
            {
                Name = PropertyName.Get("get set [PropertyType,P] [DeclaringType,P].P"),
                Get =
                {
                    new ContinueStatement(),
                    new BreakStatement()
                },
                Set =
                {
                    new BreakStatement(),
                    new ContinueStatement(),
                }
            };

            AssertPrint(
                sst,
                "PropertyType P",
                "{",
                "    get",
                "    {",
                "        continue;",
                "        break;",
                "    }",
                "    set",
                "    {",
                "        break;",
                "        continue;",
                "    }",
                "}");
        }

        [Test]
        public void PropertyDeclaration_WithOnlyGetterBody()
        {
            var sst = new PropertyDeclaration
            {
                Name = PropertyName.Get("get set [PropertyType,P] [DeclaringType,P].P"),
                Get =
                {
                    new BreakStatement()
                }
            };

            AssertPrint(
                sst,
                "PropertyType P",
                "{",
                "    get",
                "    {",
                "        break;",
                "    }",
                "    set;",
                "}");
        }

        [Test]
        public void PropertyDeclaration_WithOnlySetterBody()
        {
            var sst = new PropertyDeclaration
            {
                Name = PropertyName.Get("get set [PropertyType,P] [DeclaringType,P].P"),
                Set =
                {
                    new BreakStatement()
                }
            };

            AssertPrint(
                sst,
                "PropertyType P",
                "{",
                "    get;",
                "    set",
                "    {",
                "        break;",
                "    }",
                "}");
        }

        [Test]
        public void MethodDeclaration_EmptyMethod()
        {
            var sst = new MethodDeclaration
            {
                Name = MethodName.Get("[ReturnType,P] [DeclaringType,P].M([ParameterType,P] p)")
            };

            AssertPrint(
                sst,
                "ReturnType M(ParameterType p) { }");
        }

        [Test]
        public void MethodDeclaration_Static()
        {
            var sst = new MethodDeclaration
            {
                Name = MethodName.Get("static [ReturnType,P] [DeclaringType,P].M([ParameterType,P] p)")
            };

            AssertPrint(
                sst,
                "static ReturnType M(ParameterType p) { }");
        }

        [Test]
        public void MethodDeclaration_ParameterModifiers_PassedByReference()
        {
            var sst = new MethodDeclaration
            {
                Name = MethodName.Get("[ReturnType,P] [DeclaringType,P].M(ref [System.Int32, mscore, 4.0.0.0] p)")
            };

            AssertPrint(
                sst,
                "ReturnType M(ref Int32 p) { }");
        }

        [Test]
        public void MethodDeclaration_ParameterModifiers_Output()
        {
            var sst = new MethodDeclaration
            {
                Name = MethodName.Get("[ReturnType,P] [DeclaringType,P].M(out [ParameterType,P] p)")
            };

            AssertPrint(
                sst,
                "ReturnType M(out ParameterType p) { }");
        }

        [Test]
        public void MethodDeclaration_ParameterModifiers_Params()
        {
            var sst = new MethodDeclaration
            {
                Name = MethodName.Get("[ReturnType,P] [DeclaringType,P].M(params [ParameterType[],P] p)")
            };

            AssertPrint(
                sst,
                "ReturnType M(params ParameterType[] p) { }");
        }

        [Test]
        public void MethodDeclaration_ParameterModifiers_Optional()
        {
            var sst = new MethodDeclaration
            {
                Name = MethodName.Get("[ReturnType,P] [DeclaringType,P].M(opt [ParameterType,P] p)")
            };

            AssertPrint(
                sst,
                "ReturnType M(opt ParameterType p) { }");
        }

        [Test]
        public void MethodDeclaration_WithBody()
        {
            var sst = new MethodDeclaration
            {
                Name = MethodName.Get("[ReturnType,P] [DeclaringType,P].M([ParameterType,P] p)"),
                Body =
                {
                    new ContinueStatement(),
                    new BreakStatement()
                }
            };

            AssertPrint(
                sst,
                "ReturnType M(ParameterType p)",
                "{",
                "    continue;",
                "    break;",
                "}");
        }

        [Test]
        public void VariableDeclaration()
        {
            var sst = SSTUtil.Declare("var", TypeName.Get("T,P"));

            AssertPrint(sst, "T var;");
        }
    }
}