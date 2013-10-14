﻿using System.Collections.Generic;
using System.IO;
using CodeCompletion.Utils.Assertion;
using Newtonsoft.Json;

namespace KAVE.KAVE_MessageBus.Json
{
    internal class JsonLogReader
    {
        private readonly StreamReader _logStreamReader;

        public JsonLogReader(Stream logStream)
        {
            Asserts.NotNull(logStream, "log stream");
            Asserts.That(logStream.CanRead, "log stream not readable");
            _logStreamReader = new StreamReader(logStream, JsonLogSerialization.Encoding);
        }

        public TMessage Read<TMessage>()
        {
            var json = _logStreamReader.ReadLine();
            return json == null
                ? default(TMessage)
                : JsonConvert.DeserializeObject<TMessage>(json, JsonLogSerialization.Settings);
        }

        /// <summary>
        /// Returns an Enumarable that lazily reads all log entries from the underlying stream.
        /// </summary>
        public IEnumerable<TEvent> GetEnumeration<TEvent>() where TEvent : class
        {
            TEvent evt;
            while ((evt = Read<TEvent>()) != null)
            {
                yield return evt;
            }
        }
    }
}