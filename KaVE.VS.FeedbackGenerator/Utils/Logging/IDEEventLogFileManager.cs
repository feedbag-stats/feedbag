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

using System;
using System.IO;
using JetBrains.Application;
using KaVE.RS.Commons;

namespace KaVE.VS.FeedbackGenerator.Utils.Logging
{
    [ShellComponent]
    public class IDEEventLogFileManager : LogFileManager
    {
        public const string ProjectName = "KaVE";

        /// <summary>
        ///     Usually something like "C:\Users\%USERNAME%\AppData\Roaming\"
        /// </summary>
        public static readonly string AppDataPath = Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData);

        /// <summary>
        ///     Usually something like "C:\Users\%USERNAME%\AppData\Local\"
        /// </summary>
        public static readonly string LocalAppDataPath = Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData);

        public static readonly string EventLogsScope = typeof(IDEEventLogFileManager).Assembly.GetName().Name;

        /// <summary>
        ///     E.g., "C:\Users\%USERNAME%\AppData\Local\KaVE\KaVE.VS.FeedbackGenerator\%VARIANT%\"
        /// </summary>
        public static readonly string EventLogsPath = Path.Combine(LocalAppDataPath, ProjectName, EventLogsScope);

        public IDEEventLogFileManager(FeedBaGVersionUtil versionUtil)
            : base(Path.Combine(EventLogsPath, versionUtil.GetVariant().ToString())) { }
    }
}