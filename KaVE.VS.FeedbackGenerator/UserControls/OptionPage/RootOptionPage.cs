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

using JetBrains.Application.UI.Options;
using JetBrains.Application.UI.Options.OptionPages;
using JetBrains.Application.UI.Options.Options.ThemedIcons;
using JetBrains.Application.UI.Options.OptionsDialog;

namespace KaVE.VS.FeedbackGenerator.UserControls.OptionPage
{
    [OptionsPage(
        PID,
        "KaVE Project",
        typeof(OptionsThemedIcons.Options),
        ParentId = ToolsPage.PID)]
    public class RootOptionPage : AEmptyOptionsPage
    {
        public const string PID = "KaVE.VS.FeedbackGenerator.UserControls.OptionPage.RootOptionPage";
    }
}