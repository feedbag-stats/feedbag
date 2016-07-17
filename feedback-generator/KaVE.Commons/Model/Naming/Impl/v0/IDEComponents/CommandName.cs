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

using KaVE.Commons.Model.Naming.IDEComponents;

namespace KaVE.Commons.Model.Naming.Impl.v0.IDEComponents
{
    public class CommandName : BaseName, ICommandName
    {
        public CommandName() : base(UnknownNameIdentifier) {}
        public CommandName(string identifier) : base(identifier) {}

        public string Guid
        {
            get { return Identifier.Substring(0, Identifier.IndexOf(':')); }
        }

        public int Id
        {
            get
            {
                var parts = Identifier.Split(':');
                return int.Parse(parts[1]);
            }
        }

        public string Name
        {
            get
            {
                var parts = Identifier.Split(':');
                return parts.Length == 3 ? parts[2] : null;
            }
        }

        public override bool IsUnknown
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}