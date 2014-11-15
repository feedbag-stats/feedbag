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
 *    - Sven Amann
 */

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Tree;
using KaVE.Model.Events.CompletionEvent;
using KaVE.Model.Names;
using KaVE.Model.SSTs;
using KaVE.VsFeedbackGenerator.Analysis;
using KaVE.VsFeedbackGenerator.Analysis.Util;
using KaVE.VsFeedbackGenerator.Generators;
using Moq;

namespace KaVE.VsFeedbackGenerator.RS8Tests.Analysis
{
    [ShellComponent, Language(typeof (CSharpLanguage))]
    public class TestAnalysisTrigger : CSharpItemsProviderBase<CSharpCodeCompletionContext>
    {
        private readonly ISSTFactory _factory;
        public IEnumerable<IMethodName> LastEntryPoints { get; private set; }
        public Context LastContext { get; private set; }
        public SST LastSST { get; private set; }
        public Tuple<Exception, string> LastException { get; private set; }

        public TestAnalysisTrigger(ISSTFactory factory)
        {
            _factory = factory;
        }

        public bool HasFailed
        {
            get { return LastException != null; }
        }

        protected override bool AddLookupItems(CSharpCodeCompletionContext context, GroupedItemsCollector collector)
        {
            LastException = null;
            LastContext = ContextAnalysis.Analyze(context, MockLogger());

            var typeDeclaration = ContextAnalysis.FindEnclosing<ITypeDeclaration>(context.NodeInFile);
            if (typeDeclaration != null)
            {
                var typeShape = new TypeShapeAnalysis().Analyze(typeDeclaration);
                var entryPoints = new EntryPointSelector(typeDeclaration, typeShape).GetEntryPoints();
                LastSST = SSTAnalysis.Analyze(context, typeDeclaration, entryPoints, _factory);
                LastEntryPoints = entryPoints.Select(ep => ep.Name);
            }
            return false;
        }

        private ILogger MockLogger()
        {
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(logger => logger.Error(It.IsAny<Exception>()))
                      .Callback<Exception>(e => LastException = NewException(e, ""));
            mockLogger.Setup(logger => logger.Error(It.IsAny<Exception>(), It.IsAny<string>()))
                      .Callback<Exception, string>((e, msg) => LastException = NewException(e, msg));
            mockLogger.Setup(logger => logger.Error(It.IsAny<string>()))
                      .Callback<string>(msg => LastException = NewException(null, msg));
            return mockLogger.Object;
        }

        private static Tuple<Exception, string> NewException(Exception exception, string msg)
        {
            return new Tuple<Exception, string>(exception, msg);
        }
    }
}