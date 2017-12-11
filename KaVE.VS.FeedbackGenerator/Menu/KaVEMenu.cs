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

using JetBrains.Application.UI.Actions.MenuGroups;
using JetBrains.Application.UI.ActionSystem.ActionsRevised.Menu;
using KaVE.VS.FeedbackGenerator.SessionManager.Presentation;

namespace KaVE.VS.FeedbackGenerator.Menu
{
    [ActionGroup(Id, ActionGroupInsertStyles.Submenu, Id = 12345678, Text = "&KaVE")]
    public class KaVEMenu : IAction, IInsertLast<VsMainMenuGroup>
    {
        public const string Id = "KaVE.Menu";

        public KaVEMenu(SessionManagerWindowAction b,
            UploadWizardAction d,
            OptionPageAction c,
            IntelligentCodeCompletionAction e,
            AboutAction a) { }
    }
}