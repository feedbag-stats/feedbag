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

using System.ComponentModel;

namespace KaVE.VS.Achievements.LogCollector
{
    /// <summary>
    ///     Interaction logic for LogReplayWindow.xaml
    /// </summary>
    public partial class LogReplayWindow
    {
        public LogReplayWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // cancels closing until Progress Bar is 100%
            base.OnClosing(e);
            if (!Progress.Value.Equals(100d))
            {
                e.Cancel = true;
            }
        }
    }
}