﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Util;
using KaVE.Utils;
using KaVE.Utils.Assertion;
using KaVE.VsFeedbackGenerator.Utils.Json;

namespace KaVE.VsFeedbackGenerator.Utils.Logging
{
    public class LogFile<TLogEntry> : ILog<TLogEntry> where TLogEntry : class
    {
        private readonly IIoUtils _ioUtils;

        public LogFile(string path)
        {
            _ioUtils = Registry.GetComponent<IIoUtils>();
            Path = path;
        }

        public string Path { get; private set; }

        public DateTime Date
        {
            get
            {
                var fileName = System.IO.Path.GetFileName(Path);
                Asserts.NotNull(fileName, "illegal log path: '{0}'", Path);
                var dateString = fileName.Substring(LogFileManager<TLogEntry>.LogDirectoryPrefix.Length);
                var date = DateTime.Parse(dateString);
                return date;
            }
        }

        public ILogReader<TLogEntry> NewLogReader()
        {
            var logStream = _ioUtils.OpenFile(Path, FileMode.OpenOrCreate, FileAccess.Read);
            return new JsonLogReader<TLogEntry>(logStream);
        }

        public ILogWriter<TLogEntry> NewLogWriter()
        {
            var logStream = _ioUtils.OpenFile(Path, FileMode.Append, FileAccess.Write);
            return new JsonLogWriter<TLogEntry>(logStream);
        }

        public void Remove(TLogEntry entry)
        {
            RemoveRange(new List<TLogEntry> {entry});
        }

        public void RemoveRange(IEnumerable<TLogEntry> entries)
        {
            var tempFileName = _ioUtils.GetTempFileName();
            using (var stream = _ioUtils.OpenFile(tempFileName, FileMode.Append, FileAccess.Write))
            {
                using (var writer = new JsonLogWriter<TLogEntry>(stream))
                {
                    using (var reader = NewLogReader())
                    {
                        reader.ReadAll().ForEach(
                            entry =>
                            {
                                if (!entries.Contains(entry))
                                {
                                    writer.Write(entry);
                                }
                            });
                    }
                }
            }
            _ioUtils.DeleteFile(Path);
            _ioUtils.MoveFile(tempFileName, Path);
        }

        public void Delete()
        {
            _ioUtils.DeleteFile(Path);
        }

        protected bool Equals(LogFile<TLogEntry> other)
        {
            return string.Equals(Path, other.Path);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj, Equals);
        }

        public override int GetHashCode()
        {
            return (Path != null ? Path.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return "[LogFile " + Path + "]";
        }
    }
}