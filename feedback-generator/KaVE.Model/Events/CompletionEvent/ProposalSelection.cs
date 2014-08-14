/*
 * Copyright 2014 Technische Universit�t Darmstadt
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
using System.Runtime.Serialization;
using KaVE.JetBrains.Annotations;
using KaVE.Utils;

namespace KaVE.Model.Events.CompletionEvent
{
    [DataContract]
    public class ProposalSelection
    {
        public ProposalSelection([NotNull] Proposal proposal)
        {
            Proposal = proposal;
        }

        [DataMember]
        public TimeSpan? SelectedAfter { get; set; }

        [DataMember, NotNull]
        public Proposal Proposal { get; private set; }

        public override string ToString()
        {
            return Proposal + "@" + SelectedAfter;
        }

        protected bool Equals(ProposalSelection other)
        {
            return SelectedAfter.Equals(other.SelectedAfter) && Equals(Proposal, other.Proposal);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj, Equals);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = SelectedAfter.GetHashCode();
                hashCode = (hashCode*397) ^ Proposal.GetHashCode();
                return hashCode;
            }
        }
    }
}