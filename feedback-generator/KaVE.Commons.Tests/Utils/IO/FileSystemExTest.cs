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
 * 
 * Contributors:
 *    - Sven Amann
 */

using System.Globalization;
using KaVE.Commons.Utils.IO;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Utils.IO
{
    [TestFixture]
    internal class FileSystemExTest
    {
        [TestCase(0, "0 B"),
         TestCase(1, "1 B"),
         TestCase(1024, "1 KB"),
         TestCase(1025, "1 KB"),
         TestCase(1127, "1,1 KB"),
         TestCase(1048576, "1 MB"),
         TestCase(1073741824, "1.024 MB")]
        public void Formats(long sizeInBytes, string expectedFormattedSize)
        {
            Assert.AreEqual(expectedFormattedSize, sizeInBytes.FormatSizeInBytes(new CultureInfo("de-DE")));
        }
    }
}