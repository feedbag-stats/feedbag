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
 *    - Uli Fahrer
 *    - Dennis Albrecht
 */

using System.Text.RegularExpressions;
using KaVE.Utils.Assertion;

namespace KaVE.Model.ObjectUsage
{
    public abstract class CoReName
    {
        protected CoReName(string name, string validationPattern)
        {
            var regex = new Regex("^" + validationPattern + "$");
            Asserts.That(regex.IsMatch(name));
            Name = name;
        }

        public string Name { get; private set; }

        public override bool Equals(object obj)
        {
            var name = obj as CoReName;
            return name != null && Name.Equals(name.Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + "[" + Name + "]";
        }
    }
}
