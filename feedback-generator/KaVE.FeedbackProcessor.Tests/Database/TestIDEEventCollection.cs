/*
 * Copyright 2014 Technische Universitšt Darmstadt
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

using System;
using System.Collections.Generic;
using System.Linq;
using KaVE.Commons.Model.Events;
using KaVE.FeedbackProcessor.Database;
using KaVE.FeedbackProcessor.Model;

namespace KaVE.FeedbackProcessor.Tests.Database
{
    internal class TestIDEEventCollection : IIDEEventCollection
    {
        private readonly ICollection<IDEEvent> _ideEvents = new List<IDEEvent>();

        public IEnumerable<IDEEvent> FindAll()
        {
            return _ideEvents;
        }

        public void Insert(IDEEvent instance)
        {
            _ideEvents.Add(instance);
        }

        public void Save(IDEEvent instance)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IDEEvent> GetEventStream(Developer developer)
        {
            return _ideEvents.Where(evt => developer.SessionIds.Contains(evt.IDESessionUUID));
        }

        public bool Contains(IDEEvent @event)
        {
            throw new NotImplementedException();
        }

        public IDEEvent GetFirstEvent(Developer developer)
        {
            return _ideEvents.OrderBy(ideEvent => ideEvent.TriggeredAt).First();
        }

        public IDEEvent GetLastEvent(Developer developer)
        {
            return _ideEvents.OrderByDescending(ideEvent => ideEvent.TriggeredAt).First();
        }
    }
}