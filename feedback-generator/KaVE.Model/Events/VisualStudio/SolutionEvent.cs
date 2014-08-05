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

using System.Runtime.Serialization;
using KaVE.Model.Names.VisualStudio;
using KaVE.Utils;

namespace KaVE.Model.Events.VisualStudio
{
    [DataContract]
    public class SolutionEvent : IDEEvent
    {
        public enum SolutionAction
        {
            OpenSolution,
            RenameSolution,
            CloseSolution,
            AddSolutionItem,
            RenameSolutionItem,
            RemoveSolutionItem,
            AddProject,
            RenameProject,
            RemoveProject,
            AddProjectItem,
            RenameProjectItem,
            RemoveProjectItem
        }

        [DataMember]
        public SolutionAction Action { get; set; }

        [DataMember]
        public IIDEComponentName Target { get; set; }

        protected bool Equals(SolutionEvent other)
        {
            return base.Equals(other) && Action == other.Action && Equals(Target, other.Target);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj, Equals);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ (int) Action;
                hashCode = (hashCode*397) ^ (Target != null ? Target.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, Action: {1}, Target: {2}", base.ToString(), Action, Target);
        }
    }
}