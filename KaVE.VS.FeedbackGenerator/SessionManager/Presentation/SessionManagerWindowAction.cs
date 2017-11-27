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
using JetBrains.Application.DataContext;
using JetBrains.UI.ActionsRevised;
using JetBrains.UI.ToolWindowManagement;
using KaVE.VS.Commons;
using KaVE.VS.FeedbackGenerator.Generators;

namespace KaVE.VS.FeedbackGenerator.SessionManager.Presentation
{
    [Action(Id, "Event Manager", Id = 21340987)]
    public class SessionManagerWindowAction : ActivateToolWindowActionHandler<SessionManagerWindowDescriptor>
    {
        public const string Id = "KaVE.SessionManager";

        public override void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            Registry.GetComponent<IKaVECommandGenerator>().FireOpenEventManager();
            base.Execute(context, nextExecute);
        }
    }
}