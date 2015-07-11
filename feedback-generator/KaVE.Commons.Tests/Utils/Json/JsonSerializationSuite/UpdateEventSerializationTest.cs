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

using KaVE.Commons.Model.Events.VisualStudio;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Utils.Json.JsonSerializationSuite
{
    [TestFixture]
    internal class UpdateEventSerializationTest
    {
        [Test]
        public void ShouldSerializeToString()
        {
            var installEvent = new UpdateEvent
            {
                OldPluginVersion = "SomeOldVersion",
                NewPluginVersion = "SomeNewVersion",
            };
            const string expected =
                "{\"$type\":\"KaVE.Commons.Model.Events.VisualStudio.UpdateEvent, KaVE.Commons\",\"OldPluginVersion\":\"SomeOldVersion\",\"NewPluginVersion\":\"SomeNewVersion\",\"TriggeredBy\":0}";

            JsonAssert.SerializesTo(installEvent, expected);
        }
    }
}
