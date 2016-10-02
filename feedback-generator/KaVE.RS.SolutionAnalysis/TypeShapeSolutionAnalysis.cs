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
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.Util;
using KaVE.Commons.Model.TypeShapes;
using KaVE.Commons.Utils.Assertion;
using KaVE.RS.Commons.Analysis;
using ILogger = KaVE.Commons.Utils.Exceptions.ILogger;

namespace KaVE.RS.SolutionAnalysis
{
    public class TypeShapeSolutionAnalysis
    {
        private readonly ISolution _solution;
        private readonly ILogger _logger;
        private readonly Func<TypeShape, bool> _cbTypeShape;

        public TypeShapeSolutionAnalysis(ISolution solution, ILogger logger, Func<TypeShape, bool> cbTypeShape)
        {
            _solution = solution;
            _logger = logger;
            _cbTypeShape = cbTypeShape;
        }

        /// <summary>
        ///     Requires re-entrency guard (ReentrancyGuard.Current.Execute) and read lock (ReadLockCookie.Execute).
        /// </summary>
        public void AnalyzeAllProjects()
        {
            var projects = _solution.GetAllProjects();
            projects.Remove(_solution.MiscFilesProject);
            projects.Remove(_solution.SolutionProject);
            foreach (var project in projects)
            {
                AnalyzeProject(project);
            }
        }

        private void AnalyzeProject(IProject project)
        {
            _logger.Info("");
            _logger.Info("");
            _logger.Info("###### Analyzing project '{0}'... ################################", project.Name);

            var psiModules = _solution.PsiModules();
            var primaryPsiModule = psiModules.GetPrimaryPsiModule(project, TargetFrameworkId.Default);
            Asserts.NotNull(primaryPsiModule, "no psi module");
            
            var psiServices = primaryPsiModule.GetPsiServices();
            var symbolScope = psiServices.Symbols.GetSymbolScope(primaryPsiModule, true, true);
            var globalNamespace = symbolScope.GlobalNamespace;
            var nestedNamespaces = globalNamespace.GetNestedNamespaces(symbolScope);
            AnalyzeNamespaces(nestedNamespaces, symbolScope);
        }

        private void AnalyzeNamespaces(ICollection<INamespace> namespaces, ISymbolScope symbolScope)
        {
            foreach (var nestedNamespace in namespaces)
            {
                var nestedTypeElements = nestedNamespace.GetNestedTypeElements(symbolScope);
                foreach (var typeElement in nestedTypeElements)
                {
                    if (IsRootType(typeElement))
                    {
                        continue;
                    }
                    var typeShapeAnalysis = new TypeShapeAnalysis();
                    var typeShape = typeShapeAnalysis.Analyze(typeElement);
                    var assemblyAlreadyAnalyzed = _cbTypeShape(typeShape);
                    if (assemblyAlreadyAnalyzed)
                    {
                        break;
                    }
                }
                var nestedNamespaces = nestedNamespace.GetNestedNamespaces(symbolScope);
                if (!nestedNamespaces.IsEmpty())
                {
                    AnalyzeNamespaces(nestedNamespaces, symbolScope);
                }
                
            }
        }

        private static bool IsRootType(ITypeElement type)
        {
            var fn = type.GetClrName().FullName;
            return "System.Object".Equals(fn) || "System.ValueType".Equals(fn) || "System.Enum".Equals(fn);
        }
    }
}