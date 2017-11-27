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
using KaVE.Commons.Model.Events.Enums;
using KaVE.Commons.Model.Events.UserProfiles;

namespace KaVE.RS.Commons.Settings
{
    [SettingsKey(typeof(KaVESettings), "KaVE UserProfile Settings")]
    // WARNING: Do not change classname, as it is used to identify settings
    public class UserProfileSettings
    {
        [SettingsEntry(false, "UserProfile: HasBeenAskedToFillProfile")]
        public bool HasBeenAskedToFillProfile;

        [SettingsEntry("", "UserProfile: ProfileId")]
        public string ProfileId;

        [SettingsEntry(Educations.Unknown, "UserProfile: Education")]
        public Educations Education;

        [SettingsEntry(Positions.Unknown, "UserProfile: Position")]
        public Positions Position;

        [SettingsEntry(false, "UserProfile: ProjectsCourses")]
        public bool ProjectsCourses;

        [SettingsEntry(false, "UserProfile: ProjectsPersonal")]
        public bool ProjectsPersonal;

        [SettingsEntry(false, "UserProfile: ProjectsSharedSmall")]
        public bool ProjectsSharedSmall;

        [SettingsEntry(false, "UserProfile: ProjectsSharedMedium")]
        public bool ProjectsSharedMedium;

        [SettingsEntry(false, "UserProfile: ProjectsSharedLarge")]
        public bool ProjectsSharedLarge;

        [SettingsEntry(false, "UserProfile: TeamSolo")]
        public bool TeamsSolo;

        [SettingsEntry(false, "UserProfile: TeamSmall")]
        public bool TeamsSmall;

        [SettingsEntry(false, "UserProfile: TeamMedium")]
        public bool TeamsMedium;

        [SettingsEntry(false, "UserProfile: TeamLarge")]
        public bool TeamsLarge;

        [SettingsEntry(YesNoUnknown.Unknown, "UserProfile: CodeReviews")]
        public YesNoUnknown CodeReviews;

        [SettingsEntry(Likert7Point.Unknown, "UserProfile: ProgrammingGeneral")]
        public Likert7Point ProgrammingGeneral;

        [SettingsEntry(Likert7Point.Unknown, "UserProfile: ProgrammingCSharp")]
        public Likert7Point ProgrammingCSharp;
    }
}