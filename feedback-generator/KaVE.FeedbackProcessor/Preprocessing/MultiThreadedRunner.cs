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
using System.Threading.Tasks;
using KaVE.Commons.Utils.Collections;
using KaVE.FeedbackProcessor.Preprocessing.Logging;
using KaVE.FeedbackProcessor.Preprocessing.Model;

namespace KaVE.FeedbackProcessor.Preprocessing
{
    public class MultiThreadedRunner
    {
        private readonly IPreprocessingIo _io;
        private readonly int _numProcs;
        private readonly Func<int, IdReader> _idReaderFactory;
        private readonly Grouper _grouper;
        private readonly Func<int, GroupMerger> _groupMergerFactory;
        private readonly Func<int, Cleaner> _cleanerFactory;
        private readonly IMultiThreadedRunnerLogger _log;

        private PreprocessingData _data;

        public MultiThreadedRunner(IPreprocessingIo io,
            IMultiThreadedRunnerLogger log,
            int numProcs,
            Func<int, IdReader> idReaderFactory,
            Grouper grouper,
            Func<int, GroupMerger> groupMergerFactory,
            Func<int, Cleaner> cleanerFactory)
        {
            _io = io;
            _log = log;
            _numProcs = numProcs;

            _idReaderFactory = idReaderFactory;
            _grouper = grouper;
            _groupMergerFactory = groupMergerFactory;
            _cleanerFactory = cleanerFactory;
        }

        public void Run()
        {
            Initialize();

            InParallel(ReadIds);
            GroupZipsByIds();
            InParallel(MergeZipGroups);
            InParallel(CleanZips);
        }

        private void Initialize()
        {
            var zips = _io.FindRelativeZipPaths();
            _data = new PreprocessingData(zips);
        }

        private void InParallel(Action<int> task)
        {
            var tasks = new Task[_numProcs];
            for (var i = 0; i < _numProcs; i++)
            {
                var taskId = i;
                tasks[i] = Task.Factory.StartNew(() => { task(taskId); });
            }
            Task.WaitAll(tasks);
        }

        private void ReadIds(int taskId)
        {
            var idReader = _idReaderFactory(taskId);

            string zip;
            while (_data.AcquireNextUnindexedZip(out zip))
            {
                var ids = idReader.Read(zip);
                _data.StoreIds(zip, ids);
            }
        }

        private void GroupZipsByIds()
        {
            var zipGroups = _grouper.GroupRelatedZips(_data.GetIdsByZip());
            _data.StoreZipGroups(zipGroups);
        }

        private void MergeZipGroups(int taskId)
        {
            var zipMerger = _groupMergerFactory(taskId);

            IKaVESet<string> zips;
            while (_data.AcquireNextUnmergedZipGroup(out zips))
            {
                zipMerger.Merge(zips);
                //_data.StoreMergedZip(zips.First());
            }
        }

        private void CleanZips(int taskId)
        {
            using (var cleaner = _cleanerFactory(taskId))
            {
                string zip;
                while (_data.AcquireNextUncleansedZip(out zip))
                {
                    cleaner.Clean(zip);
                }
            }
        }
    }
}