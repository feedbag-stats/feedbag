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

using JetBrains.ActionManagement;
using JetBrains.Application;
using JetBrains.Application.DataContext;
using JetBrains.UI.ActionsRevised;

namespace KaVE.VS.FeedbackGenerator.Menu
{
    [Action(Id, "Intelligent Code Completion...", Id = 8213945)]
    public class IntelligentCodeCompletionAction : MenuActionBase
    {
        public const string Id = "KaVE.VsFeedbackGenerator.IntelligentCodeCompletion";
    }

    [ShellComponent]
    public class IntelligentCodeCompletionActionHandler : MenuActionHandlerBase<IntelligentCodeCompletionAction>
    {
        public const string Url = "http://www.kave.cc/intelligent-code-completion";

        public IntelligentCodeCompletionActionHandler(IActionManager am) : base(am) { }

        public override void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            System.Diagnostics.Process.Start(Url);
        }
    }
}