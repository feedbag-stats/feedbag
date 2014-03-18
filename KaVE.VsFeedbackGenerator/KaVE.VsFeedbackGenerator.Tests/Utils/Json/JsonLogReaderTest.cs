﻿using System.IO;
using System.Linq;
using KaVE.VsFeedbackGenerator.Utils.Json;
using NUnit.Framework;

namespace KaVE.VsFeedbackGenerator.Tests.Utils.Json
{
    [TestFixture]
    public class JsonLogReaderTest
    {
        [Test]
        public void ShouldDeserializeInstance()
        {
            var reader = CreateReader("{\"Id\":\"0xDEADBEEF\"}");

            var instance = reader.ReadNext();

            Assert.AreEqual("0xDEADBEEF", instance.Id);
        }

        [Test]
        public void ShouldDeserializeMultipleInstancesOneAtATime()
        {
            var reader = CreateReader("{\"Id\":\"A\"}\r\n{\"Id\":\"B\"}\r\n");

            var instance1 = reader.ReadNext();
            var instance2 = reader.ReadNext();

            Assert.AreEqual("A", instance1.Id);
            Assert.AreEqual("B", instance2.Id);
        }

        [Test]
        public void ShouldReadNullFromEmptyStream()
        {
            var reader = CreateReader("");

            var instance = reader.ReadNext();

            Assert.IsNull(instance);
        }

        [Test]
        public void ShouldIgnoreLeadingWhitespaces()
        {
            var reader = CreateReader("      {\"Id\":\"Later\"}");

            var instance = reader.ReadNext();

            Assert.AreEqual("Later", instance.Id);
        }

        [Test]
        public void ShouldIgnoreTrailingWhitespaces()
        {
            var reader = CreateReader("{\"Id\":\"Later\"}   \r\n       ");

            reader.ReadNext();
            var instance = reader.ReadNext();

            Assert.IsNull(instance);
        }

        [Test]
        public void ShouldEnumerateLogContent()
        {
            var reader = CreateReader("{\"Id\":\"0\"}\r\n{\"Id\":\"1\"}\r\n{\"Id\":\"2\"}");

            var instances = reader.ReadAll().ToArray();

            Assert.AreEqual("0", instances[0].Id);
            Assert.AreEqual("1", instances[1].Id);
            Assert.AreEqual("2", instances[2].Id);
        }

        private static JsonLogReader<SerializationTestTarget> CreateReader(string input)
        {
            return new JsonLogReader<SerializationTestTarget>(new MemoryStream(input.AsBytes()));
        }
    }
}