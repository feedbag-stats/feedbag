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

using System.Runtime.Serialization;
using KaVE.Commons.Utils;

namespace KaVE.Commons.Model.Events
{
    [DataContract]
    public class InfoEvent : IDEEvent
    {
        [DataMember]
        public string Info { get; set; }

        protected bool Equals(InfoEvent other)
        {
            return base.Equals(other) && string.Equals(Info, other.Info);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj, Equals);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ (Info != null ? Info.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, Info: {1}", base.ToString(), Info);
        }
    }
}
