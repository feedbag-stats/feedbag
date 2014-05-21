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
 *    - Sven Amann
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Metadata.Utils;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.Model2.Assemblies.Interfaces;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.DeclaredElements;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.Util;
using KaVE.JetBrains.Annotations;
using KaVE.Model.Names;
using KaVE.Model.Names.CSharp;
using KaVE.Utils;
using KaVE.Utils.Assertion;

namespace KaVE.VsFeedbackGenerator.Utils.Names
{
    public static class ReSharperDeclaredElementNameFactory
    {
        [NotNull]
        public static TName GetName<TName>([NotNull] this DeclaredElementInstance instance) where TName : class, IName
        {
            return (TName) instance.GetName();
        }

        [NotNull]
        public static IName GetName([NotNull] this DeclaredElementInstance instance)
        {
            return instance.Element.GetName(instance.Substitution);
        }

        [CanBeNull]
        public static TName GetName<TName>([NotNull] this IClrDeclaredElement element) where TName : class, IName
        {
            return element.GetName() as TName;
        }

        [CanBeNull]
        public static TName GetName<TName>([NotNull] this IDeclaredElement element, [NotNull] ISubstitution substitution)
            where TName : class, IName
        {
            return element.GetName(substitution) as TName;
        }

        [NotNull]
        public static IName GetName([NotNull] this IClrDeclaredElement element)
        {
            return element.GetName(element.IdSubstitution);
        }

        [NotNull]
        public static IName GetName([NotNull] this IDeclaredElement element, [NotNull] ISubstitution substitution)
        {
            if (element.ShortName == SharedImplUtil.MISSING_DECLARATION_NAME)
            {
                // TODO @Seb: is this a sensible return value?
                return Name.Get(SharedImplUtil.MISSING_DECLARATION_NAME);
            }
            return IfElementIs<INamespaceName, INamespace>(element, GetName, substitution) ??
                   IfElementIs<ITypeName, ITypeParameter>(element, GetName, substitution) ??
                   IfElementIs<ITypeName, ITypeElement>(element, GetName, substitution) ??
                   IfElementIs<IMethodName, IFunction>(element, GetName, substitution) ??
                   IfElementIs<IParameterName, IParameter>(element, GetName, substitution) ??
                   IfElementIs<IFieldName, IField>(element, GetName, substitution) ??
                   IfElementIs<IPropertyName, IProperty>(element, GetName, substitution) ??
                   IfElementIs<IEventName, IEvent>(element, GetName, substitution) ??
                   IfElementIs<IName, ITypeOwner>(element, GetName, substitution) ??
                   IfElementIs<IName, IAlias>(element, GetName, substitution) ??
                   Asserts.Fail<IName>("unknown kind of declared element: {0}", element.GetType());
        }

        private static TN IfElementIs<TN, TE>(IDeclaredElement element,
            DeclaredElementToName<TN, TE> map,
            ISubstitution substitution)
            where TE : class, IDeclaredElement
            where TN : class, IName
        {
            var specificElement = element as TE;
            return specificElement != null ? map(specificElement, substitution) : null;
        }

        private delegate TN DeclaredElementToName<out TN, in TE>(TE element, ISubstitution substitution)
            where TE : class, IDeclaredElement
            where TN : class, IName;

        [NotNull]
        private static ITypeName GetName(this ITypeElement typeElement, ISubstitution substitution)
        {
            return IfElementIs<ITypeName, IDelegate>(typeElement, GetName, substitution) ??
                   IfElementIs<ITypeName, IEnum>(typeElement, GetName, substitution) ??
                   IfElementIs<ITypeName, IInterface>(typeElement, GetName, substitution) ??
                   IfElementIs<ITypeName, IStruct>(typeElement, GetName, substitution) ??
                   TypeName.Get(typeElement.GetAssemblyQualifiedName(substitution));
        }

        [NotNull]
        private static ITypeName GetName(this IDelegate delegateElement, ISubstitution substitution)
        {
            return TypeName.Get("d:" + delegateElement.GetAssemblyQualifiedName(substitution));
        }

        [NotNull]
        private static ITypeName GetName(this IEnum enumElement, ISubstitution substitution)
        {
            return TypeName.Get("e:" + enumElement.GetAssemblyQualifiedName(substitution));
        }

        [NotNull]
        private static ITypeName GetName(this IInterface interfaceElement, ISubstitution substitution)
        {
            return TypeName.Get("i:" + interfaceElement.GetAssemblyQualifiedName(substitution));
        }

        [NotNull]
        private static ITypeName GetName(this IStruct structElement, ISubstitution substitution)
        {
            var structName = structElement.GetAssemblyQualifiedName(substitution);
            var typeNameCandidate = TypeName.Get(structName);
            // predefined structs are recognized as such without flagging them
            var isPredefinedStruct = typeNameCandidate.IsStructType;
            return isPredefinedStruct ? typeNameCandidate : TypeName.Get("s:" + structName);
        }

        [NotNull]
        private static ITypeName GetName(this ITypeParameter typeParameter, ISubstitution substitution)
        {
            return TypeParameterName.Get(
                typeParameter.ShortName,
                typeParameter.GetAssemblyQualifiedNameFromActualType(substitution));
        }

        private static string GetAssemblyQualifiedNameFromActualType(this ITypeParameter typeParameter,
            ISubstitution substitution)
        {
            return substitution.Domain.Contains(typeParameter)
                ? substitution[typeParameter].GetName().Identifier
                : UnknownTypeName.Identifier;
        }

        [NotNull]
        private static INamespaceName GetName(this INamespace ns, ISubstitution substitution)
        {
            return NamespaceName.Get(ns.QualifiedName);
        }

        [NotNull]
        private static IParameterName GetName(this IParameter parameter, ISubstitution substitution)
        {
            var identifier = new StringBuilder();
            identifier.AppendIf(parameter.IsParameterArray, ParameterName.VarArgsModifier + " ");
            identifier.AppendIf(parameter.Kind == ParameterKind.OUTPUT, ParameterName.OutputModifier + " ");
            identifier.AppendIf(parameter.IsOptional, ParameterName.OptionalModifier + " ");
            identifier.AppendIf(parameter.Kind == ParameterKind.REFERENCE, ParameterName.PassByReferenceModifier + " ");
            identifier.AppendType(parameter.Type).Append(" ").Append(parameter.ShortName);
            return ParameterName.Get(identifier.ToString());
        }

        [NotNull]
        private static IMethodName GetName(this IFunction function, ISubstitution substitution)
        {
            var identifier = new StringBuilder();
            identifier.Append(function.GetMemberIdentifier(substitution, function.ReturnType));
            var functionWithTypeParameters = function as ITypeParametersOwner;
            if (functionWithTypeParameters != null)
            {
                identifier.Append(functionWithTypeParameters.GetTypeParametersList(substitution));
            }
            identifier.AppendParameters(function, substitution);
            return MethodName.Get(identifier.ToString());
        }

        [NotNull]
        private static IFieldName GetName(this IField field, ISubstitution substitution)
        {
            return FieldName.Get(field.GetMemberIdentifier(substitution, field.Type));
        }

        [NotNull]
        private static IEventName GetName(this IEvent evt, ISubstitution substitution)
        {
            return EventName.Get(evt.GetMemberIdentifier(substitution, evt.Type));
        }

        [NotNull]
        private static IPropertyName GetName(this IProperty property, ISubstitution substitution)
        {
            var identifier = new StringBuilder();
            identifier.AppendIf(property.IsWritable, PropertyName.SetterModifier + " ");
            identifier.AppendIf(property.IsReadable, PropertyName.GetterModifier + " ");
            identifier.Append(property.GetMemberIdentifier(substitution, property.ReturnType));
            identifier.AppendParameters(property, substitution);
            return PropertyName.Get(identifier.ToString());
        }

        private static string GetMemberIdentifier(this ITypeMember member, ISubstitution substitution, IType valueType)
        {
            var identifier = new StringBuilder();
            identifier.AppendIf(member.IsStatic, MemberName.StaticModifier + " ");
            identifier.AppendMemberBase(member, substitution, valueType);
            return identifier.ToString();
        }

        [NotNull]
        private static LocalVariableName GetName(this ITypeOwner variable, ISubstitution substitution)
        {
            var identifier = new StringBuilder();
            identifier.AppendType(variable.Type).Append(' ').Append(variable.ShortName);
            return LocalVariableName.Get(identifier.ToString());
        }

        [NotNull]
        private static IName GetName(this IAlias alias, ISubstitution substitution)
        {
            return AliasName.Get(alias.ShortName);
        }

        private static void AppendMemberBase(this StringBuilder identifier,
            IClrDeclaredElement member,
            ISubstitution substitution,
            IType valueType)
        {
            identifier.AppendType(valueType)
                      .Append(' ')
                      .AppendType(member.GetContainingType(), substitution)
                      .Append('.')
                      .Append(member.ShortName);
        }

        private static StringBuilder AppendType(this StringBuilder identifier, IType type)
        {
            return identifier.Append('[').Append(type.GetName().Identifier).Append(']');
        }

        [NotNull]
        private static StringBuilder AppendType(this StringBuilder identifier,
            ITypeElement type,
            ISubstitution substitution)
        {
            return identifier.Append('[').Append(type.GetName(substitution).Identifier).Append(']');
        }

        [NotNull]
        private static String GetAssemblyQualifiedName(this ITypeElement type, ISubstitution substitution)
        {
            if (type == null)
            {
                return UnknownTypeName.Identifier;
            }
            var containingModule = type.Module.ContainingProjectModule;
            Asserts.NotNull(containingModule, "module is null");
            return String.Format(
                "{0}{1}, {2}",
                type.GetClrName().FullName,
                type.GetTypeParametersList(substitution),
                containingModule.GetQualifiedName());
        }

        private static String GetTypeParametersList(this ITypeParametersOwner typeParametersOwner,
            ISubstitution substitution)
        {
            return typeParametersOwner.TypeParameters.IsEmpty()
                ? ""
                : "[[" +
                  typeParametersOwner.TypeParameters.Select(tp => tp.GetName(substitution).Identifier).Join("],[") +
                  "]]";
        }

        /// <summary>
        /// Retrieves the module's assembly-qualified name (including the assembly name and version). If the module
        /// is a project and that project is currently not compilable (and has not been compiled ever or since the
        /// last clear) the returned name will only contain the project's name and not its version. According to
        /// http://devnet.jetbrains.com/message/5503864#5503864 this is a restriction of ReSharper. Note that the
        /// project's name may differ from the project's output-assembly name.
        /// </summary>
        [NotNull]
        private static string GetQualifiedName([NotNull] this IModule module)
        {
            AssemblyNameInfo assembly = null;
            var containingProject = module as IProject;
            if (containingProject != null)
            {
                var assemblyInfo = containingProject.GetOutputAssemblyInfo();
                if (assemblyInfo != null)
                {
                    assembly = assemblyInfo.AssemblyNameInfo;
                }
            }
            var containingAssembly = module as IAssembly;
            if (containingAssembly != null)
            {
                assembly = containingAssembly.AssemblyName;
            }
            return assembly != null ? assembly.NameAndVersion() : module.Name;
        }

        [NotNull]
        private static string NameAndVersion([NotNull] this AssemblyNameInfo assemblyName)
        {
            return string.Format("{0}, {1}", assemblyName.Name, assemblyName.Version);
        }

        private static void AppendParameters(this StringBuilder identifier,
            IParametersOwner parametersOwner,
            ISubstitution substitution)
        {
            identifier.Append('(')
                      .Append(parametersOwner.Parameters.GetNames(substitution).Select(p => p.Identifier).Join(", "))
                      .Append(')');
        }

        [NotNull]
        private static IEnumerable<IParameterName> GetNames(this IEnumerable<IParameter> parameters,
            ISubstitution substitution)
        {
            return parameters.Select(param => param.GetName(substitution));
        }
    }
}