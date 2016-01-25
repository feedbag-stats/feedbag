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
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using KaVE.FeedbackProcessor.Intervals.Model;

namespace KaVE.FeedbackProcessor.Intervals.Exporter
{
    public static class WatchdogUtils
    {
        public static string GetSerializedIntervalTypeName(Interval t)
        {
            if (t is VisualStudioOpenedInterval)
            {
                return "eo";
            }
            if (t is UserActiveInterval)
            {
                return "ua";
            }
            if (t is PerspectiveInterval)
            {
                return "pe";
            }

            var re = t as FileInteractionInterval;
            if (re != null)
            {
                switch (re.Type)
                {
                    case FileInteractionType.Reading:
                        return "re";
                    case FileInteractionType.Typing:
                        return "ty";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            throw new ArgumentException();
        }

        public static long ToJavaTimestamp(this DateTime date)
        {
            long unixTimestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerSecond;
            return unixTimestamp * 1000;
        }

        public static void WriteToFiles(this WatchdogData data, string outputFolder)
        {
            File.WriteAllLines(Path.Combine(outputFolder, "intervals.json"), data.Intervals.Select(o => o.ToString()));
            File.WriteAllLines(Path.Combine(outputFolder, "projects.json"), data.Projects.Select(o => o.ToString()));
            File.WriteAllLines(Path.Combine(outputFolder, "users.json"), data.Users.Select(o => o.ToString()));
        }

        public static string Sha1Hash(string input)
        {
            using (var sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                return string.Join(string.Empty, hash.Select(b => b.ToString("x2")));
            }
        }
    }
}