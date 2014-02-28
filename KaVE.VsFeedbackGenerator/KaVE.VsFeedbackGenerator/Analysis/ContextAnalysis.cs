﻿using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using KaVE.JetBrains.Annotations;
using KaVE.Model.Events.CompletionEvent;
using KaVE.Model.Names;
using KaVE.Utils.Assertion;
using KaVE.VsFeedbackGenerator.Utils.Names;

namespace KaVE.VsFeedbackGenerator.Analysis
{
    internal class ContextAnalysis
    {
        public Context Analyze(CSharpCodeCompletionContext rsContext)
        {
            var context = new Context();

            var methodDeclaration = FindEnclosing<IMethodDeclaration>(rsContext.NodeInFile);
            var methodName = GetName(methodDeclaration);
            context.EnclosingMethodDeclaration = new MethodDeclaration(methodName);

            var typeDeclaration = methodDeclaration.GetContainingTypeDeclaration();
            context.EnclosingMethodDeclaration.Super = FindMethodInSuperTypes(
                methodDeclaration.DeclaredElement,
                typeDeclaration.DeclaredElement);
            context.EnclosingMethodDeclaration.First = FindFirstMethodInSuperTypes(
                methodDeclaration.DeclaredElement,
                typeDeclaration.DeclaredElement);

            if (context.EnclosingMethodDeclaration.First != null)
            {
                if (context.EnclosingMethodDeclaration.First.Equals(context.EnclosingMethodDeclaration.Super))
                {
                    context.EnclosingMethodDeclaration.First = null;
                }
            }

            context.EnclosingClassHierarchy = CreateTypeHierarchy(
                typeDeclaration.DeclaredElement,
                EmptySubstitution.INSTANCE);

            context.CalledMethods = FindAllCalledMethods(methodDeclaration, context.EnclosingClassHierarchy.Element);

            return context;
        }

        private IMethodName FindFirstMethodInSuperTypes(IMethod enclosingMethod,
            ITypeElement typeDeclaration)
        {
            // TODO implement GetSignature for MethodName
            var encName = GetSimpleName(enclosingMethod);

            foreach (var superType in typeDeclaration.GetSuperTypes())
            {
                var superTypeElement = superType.GetTypeElement();
                Asserts.NotNull(superTypeElement);

                var superName = FindFirstMethodInSuperTypes(enclosingMethod, superType.GetTypeElement());
                if (superName != null)
                {
                    return superName;
                }
            }

            foreach (var method in typeDeclaration.Methods)
            {
                var name = GetSimpleName(method);

                if (name.Equals(encName))
                {
                    if (method.Equals(enclosingMethod))
                    {
                        return null;
                    }
                    return (IMethodName) method.GetName(method.IdSubstitution);
                }
            }

            return null;
        }

        private IMethodName FindMethodInSuperTypes(IMethod enclosingMethod,
            ITypeElement typeDeclaration)
        {
            var encName = GetSimpleName(enclosingMethod);

            foreach (var superType in typeDeclaration.GetSuperTypes())
            {
                var superTypeElement = superType.GetTypeElement();
                Asserts.NotNull(superTypeElement);

                if (superType.IsClassType())
                {
                    foreach (var method in superTypeElement.Methods)
                    {
                        if (!method.IsAbstract)
                        {
                            var name = GetSimpleName(method);

                            if (name.Equals(encName))
                            {
                                return (IMethodName) method.GetName(method.IdSubstitution);
                            }
                        }
                    }

                    var superName = FindMethodInSuperTypes(enclosingMethod, superType.GetTypeElement());
                    if (superName != null)
                    {
                        return superName;
                    }
                }
            }

            return null;
        }

        // TODO discuss
        private string GetSimpleName(IMethod method)
        {
            var name = method.ShortName;
            var ps = string.Join(",", method.Parameters.Select(p => p.Type));
            var ret = method.ReturnType;
            return ret + " " + name + "(" + ps + ")";
        }

        private ISet<IMethodName> FindAllCalledMethods(IMethodDeclaration methodDeclaration, ITypeName enclosingType)
        {
            var methodNames = new HashSet<IMethodName>();
            if (methodDeclaration.Body != null)
            {
                methodDeclaration.Body.Accept(new MethodInvocationCollector(enclosingType), methodNames);
            }
            return methodNames;
        }

        private static TypeHierarchy CreateTypeHierarchy(ITypeElement type, ISubstitution substitution)
        {
            if (type == null || HasTypeSystemObject(type))
            {
                return null;
            }
            var typeName = type.GetName(substitution);
            var enclosingClassHierarchy = new TypeHierarchy(typeName.Identifier);

            foreach (var superType in type.GetSuperTypes())
            {
                var resolvedSuperType = superType.Resolve().DeclaredElement;

                var superTypeSubstitution = superType.GetSubstitution();

                var aClass = resolvedSuperType as IClass;
                if (aClass != null)
                {
                    enclosingClassHierarchy.Extends = CreateTypeHierarchy(aClass, superTypeSubstitution);
                }
                var anInterface = resolvedSuperType as IInterface;
                if (anInterface != null)
                {
                    enclosingClassHierarchy.Implements.Add(CreateTypeHierarchy(anInterface, superTypeSubstitution));
                }
            }
            return enclosingClassHierarchy;
        }

        private static bool HasTypeSystemObject(ITypeElement type)
        {
            return "System.Object".Equals(type.GetClrName().FullName);
        }


        private static IMethodName GetName(IMethodDeclaration methodDeclaration)
        {
            var declaredElement = methodDeclaration.DeclaredElement;
            return declaredElement.GetName<IMethodName>();
        }

        [CanBeNull]
        private static TIDeclaration FindEnclosing<TIDeclaration>(ITreeNode node)
            where TIDeclaration : class, IDeclaration
        {
            if (node == null)
            {
                return null;
            }

            var methodDeclaration = node as TIDeclaration;
            return methodDeclaration ?? FindEnclosing<TIDeclaration>(node.Parent);
        }
    }
}
