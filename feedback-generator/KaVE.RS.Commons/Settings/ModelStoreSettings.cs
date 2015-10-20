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

using JetBrains.Application.Settings;
using JetBrains.UsageStatistics;

namespace KaVE.RS.Commons.Settings
{
    namespace KaVE.RS.Commons.Settings
    {
        [SettingsKey(typeof (FeedbackSettings), "KaVE model store settings")]
        public class ModelStoreSettings
        {
            [SettingsEntry(@"c:\kave-models\", "Path to KaVE models")]
            public string ModelStorePath;

            [SettingsEntry(@"file://C:/", "Remote KaVE models")]
            public string ModelStoreUri;
        }
    }
}