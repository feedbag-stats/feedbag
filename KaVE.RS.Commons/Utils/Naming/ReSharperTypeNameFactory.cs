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
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using KaVE.Commons.Model.Naming;
using KaVE.Commons.Model.Naming.Types;
using KaVE.Commons.Utils.Assertion;

namespace KaVE.RS.Commons.Utils.Naming
{
    public static class ReSharperTypeNameFactory
    {
        [NotNull]
        public static ITypeName GetName(this IType type)
        {
            return type.GetName(new Dictionary<DeclaredElementInstance, IName>());
        }

        [NotNull]
        internal static ITypeName GetName(this IType type, IDictionary<DeclaredElementInstance, IName> seenElements)
        {
            return IfTypeIs<IDeclaredType>(type, GetName, seenElements) ??
                   IfTypeIs<IArrayType>(type, GetName, seenElements) ??
                   IfTypeIs<IAnonymousType>(type, GetName, seenElements) ??
                   IfTypeIs<IMultitype>(type, GetName, seenElements) ??
                   IfTypeIs<IPointerType>(type, GetName, seenElements) ??
                   Asserts.Fail<ITypeName>("unknown IType case: {0}", type.GetType());
        }

        private static ITypeName IfTypeIs<TE>(IType element,
            Func<TE, IDictionary<DeclaredElementInstance, IName>, ITypeName> map,
            IDictionary<DeclaredElementInstance, IName> seenElements) where TE : class, IType
        {
            var specificElement = element as TE;
            return specificElement != null ? map(specificElement, seenElements) : null;
        }

        [NotNull]
        private static ITypeName GetName(this IDeclaredType type,
            IDictionary<DeclaredElementInstance, IName> seenElements)
        {
            var typeElement = type.GetTypeElement();
            // typeElement can be null, for example when resolving the second
            // parameter type in the incomplete method declaration
            // > public void M(int i, )
            return typeElement == null
                ? Names.UnknownType
                : (ITypeName) typeElement.GetName(type.GetSubstitution(), seenElements);
        }

        [NotNull]
        private static ITypeName GetName(this IArrayType arrayType,
            IDictionary<DeclaredElementInstance, IName> seenElements)
        {
            var baseType = arrayType.ElementType.GetName(seenElements);
            return Names.ArrayType(arrayType.Rank, baseType);
        }

        [NotNull]
        private static ITypeName GetName(this IAnonymousType type,
            IDictionary<DeclaredElementInstance, IName> seenElements)
        {
            return Names.UnknownType;
        }

        [NotNull]
        private static ITypeName GetName(this IMultitype type, IDictionary<DeclaredElementInstance, IName> seenElements)
        {
            return Names.UnknownType;
        }

        [NotNull]
        private static ITypeName GetName(this IPointerType type,
            IDictionary<DeclaredElementInstance, IName> seenElements)
        {
            return type.ElementType.GetName(seenElements);
        }
    }
}