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

using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.Match;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.TextControl;
using JetBrains.UI.Icons;
using JetBrains.UI.RichText;

namespace KaVE.RS.Commons.Utils.LookupItems
{
    public abstract class AbstractWrappedLookupItem : IWrappedLookupItem
    {
        public ILookupItem Item { get; protected set; }

        public bool AcceptIfOnlyMatched(LookupItemAcceptanceContext itemAcceptanceContext)
        {
            return Item.AcceptIfOnlyMatched(itemAcceptanceContext);
        }

        public MatchingResult Match(PrefixMatcher prefixMatcher, ITextControl textControl)
        {
            return Item.Match(prefixMatcher, textControl);
        }

        public void Accept(ITextControl textControl,
            DocumentRange nameRange,
            LookupItemInsertType lookupItemInsertType,
            Suffix suffix,
            ISolution solution,
            bool keepCaretStill)
        {
            Item.Accept(textControl, nameRange, lookupItemInsertType, suffix, solution, keepCaretStill);
        }

        public DocumentRange GetVisualReplaceRange(DocumentRange nameRange)
        {
            return Item.GetVisualReplaceRange(nameRange);
        }

        public bool Shrink()
        {
            return Item.Shrink();
        }

        public void Unshrink()
        {
            Item.Unshrink();
        }

        public virtual LookupItemPlacement Placement
        {
            get { return Item.Placement; }
            set { Item.Placement = value; }
        }

        public virtual IconId Image
        {
            get { return Item.Image; }
        }

        public virtual RichText DisplayName
        {
            get { return Item.DisplayName; }
        }

        public RichText DisplayTypeName
        {
            get { return Item.DisplayTypeName; }
        }

        public bool CanShrink
        {
            get { return Item.CanShrink; }
        }

        public bool IsDynamic
        {
            get { return Item.IsDynamic; }
        }

        public bool IgnoreSoftOnSpace
        {
            get { return Item.IgnoreSoftOnSpace; }
        }

        public int Identity
        {
            get { return Item.Identity; }
        }
    }
}