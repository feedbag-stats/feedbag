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
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.AspectLookupItems.BaseInfrastructure;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.AspectLookupItems.Info;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Features.Intellisense.CodeCompletion.CSharp.AspectLookupItems;
using KaVE.Commons.Model.Events.CompletionEvents;
using KaVE.Commons.Model.Names;
using KaVE.Commons.Model.Names.CSharp;
using KaVE.RS.Commons.Utils.Names;

namespace KaVE.RS.Commons.Utils
{
    public static class LookupItemUtils
    {
        [NotNull]
        public static ProposalCollection ToProposalCollection([NotNull] this IEnumerable<ILookupItem> items)
        {
            return new ProposalCollection(items.Select(ToProposal).ToList());
        }

        [NotNull]
        public static Proposal ToProposal([CanBeNull] this ILookupItem lookupItem)
        {
            var name = lookupItem == null ? Name.UnknownName : lookupItem.GetName();
            return new Proposal {Name = name};
        }

        [NotNull]
        public static IEnumerable<Proposal> ToProposals([CanBeNull] this LookupItem<MethodsInfo> lookupItem)
        {
            var result = new List<Proposal>();

            if (lookupItem != null)
            {
                lookupItem.Info.Candidates.ToList()
                          .ForEach(
                              candidate => result.Add(new Proposal {Name = candidate.GetName()}));
            }

            return result;
        }

        private static IName GetName([NotNull] this ILookupItem lookupItem)
        {
            return TryGetNameFromLookupItem<CSharpDeclaredElementInfo>(lookupItem) ??
                   TryGetNameFromLookupItem<DeclaredElementInfo>(lookupItem) ??
                   TryGetNameFromLookupItem<MethodsInfo>(lookupItem) ??
                   TryGetNameFromConstructorInfoLookupItem(lookupItem) ??
                   TryGetNameFromCombinedLookupItem(lookupItem) ??
                   GetNameFromLookupItemIdentity(lookupItem);
        }

        private static IName TryGetNameFromLookupItem<T>(ILookupItem lookupItem) where T : class, ILookupItemInfo
        {
            var li = lookupItem as LookupItem<T>;
            if (li == null || li.Info == null)
            {
                return null;
            }
            var de = li.GetAllDeclaredElements().FirstOrDefault();
            return de == null ? null : de.GetName();
        }

        private static IName TryGetNameFromConstructorInfoLookupItem(ILookupItem lookupItem)
        {
            var li = lookupItem as LookupItem<ConstructorInfo>;
            if (li == null || li.Info == null)
            {
                return null;
            }
            var des = li.GetAllDeclaredElements();
            var de = des.FirstOrDefault();
            if (de == null)
            {
                return null;
            }

            // strangely, the DE points to the class instead of the constructor. As we don't get
            // the method, we introduce an artificial constructor name to at least capture the
            // information that a constructor was selected
            var typeName = de.GetName();
            return MethodName.Get(string.Format("[{0}] [{0}]..ctor()", typeName));
        }

        private static IName TryGetNameFromCombinedLookupItem(ILookupItem lookupItem)
        {
            var cli = lookupItem as CombinedLookupItem;
            if (cli == null)
            {
                return null;
            }
            return Name.Get(String.Format("CombinedLookupItem:{0}", cli.DisplayName.Text));
        }

        private static IName GetNameFromLookupItemIdentity(ILookupItem item)
        {
            return Name.Get(GetPossiblyGenericTypeName(item) + ":" + item.Identity);
        }

        private static string GetPossiblyGenericTypeName(ILookupItem item)
        {
            var type = item.GetType();

            return CreateName(type);
        }

        private static string CreateName(Type type)
        {
            var sb = new StringBuilder();
            sb.Append(type.Name);
            if (type.IsGenericType)
            {
                sb.Append('[');
                var argTypes = type.GetGenericArguments().Select(t => "[" + CreateName(t) + "]");
                sb.Append(string.Join(", ", argTypes));
                sb.Append(']');
            }

            return sb.ToString();
        }
    }
}