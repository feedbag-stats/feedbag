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
using System.Text;
using System.Windows.Documents;
using System.Windows.Threading;
using KaVE.Commons.Utils;
using KaVE.Commons.Utils.Exceptions;
using KaVE.JetBrains.Annotations;
using KaVE.VS.Commons;
using KaVE.VS.FeedbackGenerator.SessionManager.Presentation;
using Moq;
using NUnit.Framework;

namespace KaVE.VS.FeedbackGenerator.Tests.SessionManager.Presentation
{
    internal class XamlBindableRichTextBoxTest
    {
        [SetUp]
        public void SetUp()
        {
            Registry.RegisterComponent(new Mock<ILogger>().Object);
        }

        [TearDown]
        public void TearDown()
        {
            Registry.Clear();
        }

        [Test]
        public void ShouldDisplaySimpleXaml()
        {
            TestDisplayXaml(
                "<Bold>Test</Bold>",
                par =>
                {
                    var bold = GetFirstChild<Bold>(par);
                    var text = GetFirstChild<Run>(bold);
                    Assert.AreEqual("Test", text.Text);
                });
        }

        [Test(Description = "Xaml with more than 65534 nodes cannot be parsed")]
        public void ShouldDisplayXamlWithMoreThan65534NodesWithoutSyntaxHighlighting()
        {
            var xaml = CreateXamlWithMoreThan65534NodesStartingWith("<Bold>A</Bold>\n<Bold>B</Bold>\n");

            TestDisplayXaml(
                xaml,
                par =>
                {
                    var text = GetFirstChild<Run>(par);
                    StringAssert.StartsWith("A\nB\n", text.Text);
                });
        }

        [Test]
        public void KeepsEscapedSpecialCharacterWhenStrippingSyntaxHighlighting()
        {
            var xaml = CreateXamlWithMoreThan65534NodesStartingWith("A&lt;B&gt;");

            TestDisplayXaml(
                xaml,
                par =>
                {
                    var text = GetFirstChild<Run>(par);
                    StringAssert.StartsWith("A&lt;B&gt;", text.Text);
                });
        }

        private static string CreateXamlWithMoreThan65534NodesStartingWith(string prefix)
        {
            var xaml = new StringBuilder(prefix);
            for (var i = 0; i < 65535; i++)
            {
                xaml.Append("<Bold>A</Bold>");
            }
            return xaml.ToString();
        }

        [NotNull]
        private static TI GetFirstChild<TI>(Span span) where TI : Inline
        {
            return GetFirstChild<TI>(span.Inlines);
        }

        [NotNull]
        private static TI GetFirstChild<TI>(Paragraph block) where TI : Inline
        {
            return GetFirstChild<TI>(block.Inlines);
        }

        [NotNull]
        private static TI GetFirstChild<TI>(InlineCollection inlines) where TI : Inline
        {
            var inline = inlines.FirstInline as TI;
            Assert.NotNull(inline);
            return inline;
        }

        private static void TestDisplayXaml(string xaml, Action<Paragraph> assertion)
        {
            Invoke.OnSTA(
                () =>
                {
                    var uut = new XamlBindableRichTextBox {Xaml = xaml};

                    // wait for asynchronous task on dispatcher to finish
                    uut.Dispatcher.Invoke(() => { }, DispatcherPriority.Input);

                    var paragraph = uut.Document.Blocks.FirstBlock as Paragraph;
                    Assert.NotNull(paragraph);
                    assertion(paragraph);
                });
        }
    }
}