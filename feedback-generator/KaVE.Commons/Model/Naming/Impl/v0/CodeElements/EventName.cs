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

using KaVE.Commons.Model.Naming.CodeElements;
using KaVE.Commons.Model.Naming.Types;
using KaVE.Commons.Utils.Collections;
using KaVE.JetBrains.Annotations;

namespace KaVE.Commons.Model.Naming.Impl.v0.CodeElements
{
    public class EventName : MemberName, IEventName
    {
        private static readonly WeakNameCache<EventName> Registry = WeakNameCache<EventName>.Get(
            id => new EventName(id));

        public new static IEventName UnknownName
        {
            get { return Get("[?] [?].???"); }
        }

        public override bool IsUnknown
        {
            get { return Equals(this, UnknownName); }
        }

        /// <summary>
        ///     Event names follow the scheme <code>'modifiers' ['event-handler-type name'] ['declaring-type name'].'name'</code>.
        ///     Examples of type names are:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>
        ///                 <code>[ChangeEventHandler, IO, 1.2.3.4] [TextBox, GUI, 5.6.7.8].Changed</code>
        ///             </description>
        ///         </item>
        ///     </list>
        /// </summary>
        [NotNull]
        public new static EventName Get(string identifier)
        {
            return Registry.GetOrCreate(identifier);
        }

        [NotNull]
        public static EventName Get(string id, params object[] args)
        {
            return Get(string.Format(id, args));
        }

        private EventName(string identifier) : base(identifier) {}

        public ITypeName HandlerType
        {
            get { return ValueType; }
        }
    }
}