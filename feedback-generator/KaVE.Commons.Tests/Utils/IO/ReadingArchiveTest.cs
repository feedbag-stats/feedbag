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

using System.Collections.Generic;
using System.IO;
using Ionic.Zip;
using KaVE.Commons.Utils.Assertion;
using KaVE.Commons.Utils.IO;
using KaVE.Commons.Utils.Json;
using NUnit.Framework;

namespace KaVE.Commons.Tests.Utils.IO
{
    internal class ReadingArchiveTest
    {
        private string _zipPath;

        private ReadingArchive _sut;

        [SetUp]
        public void SetUp()
        {
            _zipPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        }

        [TearDown]
        public void TearDown()
        {
            if (_sut != null)
            {
                _sut.Dispose();
            }
            if (File.Exists(_zipPath))
            {
                File.Delete(_zipPath);
            }
        }

        [Test, ExpectedException(typeof (AssertException))]
        public void NonExistingZip()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new ReadingArchive(@"C:\does\not\exist.zip");
        }

        [Test]
        public void EmptyZip()
        {
            PrepareZip();

            Assert.False(_sut.HasNext());
        }

        [Test]
        public void NonEmptyZip()
        {
            PrepareZip("a", "b");

            Assert.True(_sut.HasNext());
            Assert.AreEqual("a", _sut.GetNext<string>());
            Assert.True(_sut.HasNext());
            Assert.AreEqual("b", _sut.GetNext<string>());
            Assert.False(_sut.HasNext());
        }

        [Test]
        public void GetAll()
        {
            PrepareZip("a", "b", "c");

            var actual = _sut.GetAll<string>();
            var expected = new List<string> {"a", "b", "c"};
            Assert.AreEqual(expected, actual);
        }

        private void PrepareZip(params string[] entries)
        {
            using (var zipFile = new ZipFile())
            {
                var i = 0;
                foreach (var entry in entries)
                {
                    var fileName = (i++) + ".json";
                    zipFile.AddEntry(fileName, entry.ToCompactJson());
                }
                zipFile.Save(_zipPath);
            }
            _sut = new ReadingArchive(_zipPath);
        }
    }
}