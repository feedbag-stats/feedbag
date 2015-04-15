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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KaVE.Commons.Model.Names.CSharp;
using KaVE.Commons.Model.SSTs.Impl;
using KaVE.Commons.Model.SSTs.Impl.Expressions.Simple;
using KaVE.Commons.Model.SSTs.Impl.Statements;
using KaVE.Commons.Utils;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Utils.SSTPrintingVisitorTestSuite
{
    internal class StatementPrinterTest : SSTPrintingVisitorTestBase
    {
        [Test]
        public void BreakStatement()
        {
            var sst = new BreakStatement();
            AssertPrint(sst, "break;");
        }

        [Test]
        public void ContinueStatement()
        {
            var sst = new ContinueStatement();
            AssertPrint(sst, "continue;");
        }

        //[Test]
        //public void Assignment()
        //{
        //    var sst = SSTUtil.AssignmentToLocal("var", new ConstantValueExpression());
        //}

        [Test]
        public void GotoStatement()
        {
            var sst = new GotoStatement
            {
                Label = "L"
            };

            AssertPrint(sst, "goto L;");
        }

        [Test]
        public void LabelledStatement()
        {
            var sst = new LabelledStatement
            {
                Label = "L",
                Statement = new ContinueStatement()
            };

            AssertPrint(sst,
                "L:",
                "continue;");
        }

        [Test]
        public void ThrowStatement()
        {
            var sst = new ThrowStatement
            {
                Exception = TypeName.Get("T,P")
            };

            AssertPrint(sst, "throw new T();");
        }

        [Test]
        public void ReturnStatement()
        {
            var sst = new ReturnStatement
            {
                Expression = new ConstantValueExpression {Value = "val"}
            };

            // TODO: check: value in quotes?
            AssertPrint(sst, "return val;");
        }
    }
}
