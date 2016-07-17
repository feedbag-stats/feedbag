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
using System.Text.RegularExpressions;
using KaVE.Commons.Model.Naming.Types;

namespace KaVE.Commons.Model.Naming.Impl.v0.Types
{
    public class AssemblyName : BaseName, IAssemblyName
    {
        private readonly Regex _isValidVersionRegex = new Regex("^\\d\\.\\d\\.\\d\\.\\d$");


        public AssemblyName() : base(UnknownNameIdentifier) {}
        public AssemblyName(string identifier) : base(identifier) {}

        public IAssemblyVersion Version
        {
            get
            {
                var fragments = GetFragments();
                return fragments.Length <= 1
                    ? new AssemblyVersion()
                    : new AssemblyVersion(fragments[1]);
            }
        }

        public string Name
        {
            get { return GetFragments()[0]; }
        }

        public bool IsLocalProject
        {
            get { return Version.Equals(Names.UnknownAssembly.Version); }
        }

        private string[] GetFragments()
        {
            var split = Identifier.LastIndexOf(",", StringComparison.Ordinal);
            if (split == -1)
            {
                return new[] {Identifier};
            }
            var name = Identifier.Substring(0, split).Trim();
            var version = Identifier.Substring(split + 1).Trim();


            return _isValidVersionRegex.IsMatch(version)
                ? new[] {name, version}
                : new[] {Identifier};
        }

        public override bool IsUnknown
        {
            get { throw new NotImplementedException(); }
        }
    }
}