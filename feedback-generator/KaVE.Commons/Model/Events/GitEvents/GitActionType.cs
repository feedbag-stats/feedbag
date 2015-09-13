/*
 * Copyright 2014 Technische Universitšt Darmstadt
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
using System.Text.RegularExpressions;

namespace KaVE.Commons.Model.Events.GitEvents
{
    public enum GitActionType
    {
        Unknown = 0,
        Branch,
        Checkout,
        Clone,
        Commit,
        CommitAmend,
        CommitInitial,
        Merge,
        Pull,
        Rebase,
        RebaseFinished,
        Reset
    }

    public static class GitActionTypeExtensions
    {
        private static readonly Dictionary<Regex, GitActionType> SpecialPatterns = new Dictionary
            <Regex, GitActionType>
        {
            {new Regex(@"commit \(amend\)"), GitActionType.CommitAmend},
            {new Regex(@"commit \(initial\)"), GitActionType.CommitInitial},
            {new Regex("rebase finished"), GitActionType.RebaseFinished},
            {new Regex("pull.*"), GitActionType.Pull},
            {new Regex("merge.*"), GitActionType.Merge}
        };

        public static GitActionType ToGitActionType(this string value)
        {
            try
            {
                return (GitActionType) Enum.Parse(typeof (GitActionType), value, true);
            }
            catch
            {
                return HandleSpecialCases(value);
            }
        }

        private static GitActionType HandleSpecialCases(string value)
        {
            var match = SpecialPatterns.Keys.FirstOrDefault(regex => regex.IsMatch(value));
            return match != null ? SpecialPatterns[match] : GitActionType.Unknown;
        }
    }
}