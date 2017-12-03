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
using EnvDTE;
using JetBrains.DocumentManagers;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.Util;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.TextControl;
using JetBrains.Threading;
using JetBrains.Util;
using KaVE.Commons.Model.Events.CompletionEvents;
using KaVE.JetBrains.Annotations;
using KaVE.RS.Commons.Analysis;
using ILogger = KaVE.Commons.Utils.Exceptions.ILogger;

namespace KaVE.VS.FeedbackGenerator.Generators.VisualStudio.EditEventGenerators.EventContext
{
    [SolutionComponent]
    internal class ContextGenerator
    {
        private Context CurrentContext { get; set; }

        private readonly TextControlManager _textControlManager;
        private readonly DocumentManager _documentManager;
        private readonly ISolution _solution;
        private readonly ILogger _logger;

        public ContextGenerator(TextControlManager textControlManager,
            IntellisenseManager intellisenseManager,
            ILogger logger)
        {
            _textControlManager = textControlManager;
            _documentManager = intellisenseManager.DocumentManager;
            _logger = logger;
            _solution = intellisenseManager.Solution;
        }

        public Context GetCurrentContext([NotNull] Document vsDocument)
        {
            return ComputeNewContextByFilePath(vsDocument.FullName);
        }

        public Context GetCurrentContext([NotNull] TextPoint startPoint)
        {
            // activeDocument is sometimes null, e.g., when IDE shuts down
            var activeDocument = startPoint.DTE.ActiveDocument;
            return activeDocument != null ? ComputeNewContextByFilePath(activeDocument.FullName) : new Context();
        }

        private Context ComputeNewContextByFilePath([NotNull] string filePath)
        {
            CurrentContext = Context.Default;

            var document = GetDocument(filePath);
            if (document != null)
            {
                FindEntryNode(RunAnalysis);
            }

            return CurrentContext;
        }

        private IDocument GetDocument(string filePath)
        {
            try
            {
                var path = FileSystemPath.Parse(filePath);
                return _documentManager.GetOrCreateDocument(path);
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "failed to access the current document");
            }
            return null;
        }

        private void FindEntryNode(Action<ITreeNode> process)
        {
            ReentrancyGuard.Current.ExecuteOrQueue(
                "context-generator",
                () =>
                {
                    ReadLockCookie.Execute(
                        () =>
                        {
                            var node = FindCurrentTreeNode();

                            if (node == null)
                            {
                                return;
                            }

                            if (!HasSourroundingMethod(node))
                            {
                                node = FindSourroundingClassDeclaration(node);
                            }

                            if (node != null)
                            {
                                process(node);
                            }
                        });
                });
        }

        private ITreeNode FindCurrentTreeNode()
        {
            var textControl = _textControlManager.FocusedTextControl.Value;
            return textControl == null ? null : TextControlToPsi.GetElement<ITreeNode>(_solution, textControl);
        }

        private static bool HasSourroundingMethod(ITreeNode node)
        {
            var method = node.GetContainingNode<IMethodDeclaration>(true);
            return method != null;
        }

        private static ICSharpTypeDeclaration FindSourroundingClassDeclaration(ITreeNode psiFile)
        {
            return psiFile.GetContainingNode<ICSharpTypeDeclaration>(true);
        }

        private void RunAnalysis(ITreeNode node)
        {
            ContextAnalysis.Analyse(
                node,
                null,
                _logger,
                context => { CurrentContext = context; },
                delegate { },
                delegate { });
        }
    }
}