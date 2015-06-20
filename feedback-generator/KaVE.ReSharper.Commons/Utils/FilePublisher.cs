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
 */

using System;
using System.IO;
using JetBrains.Annotations;
using JetBrains.Util;
using KaVE.Commons.Utils.Assertion;
using KaVE.Commons.Utils.IO;
using KaVE.RS.Commons.Properties;

namespace KaVE.RS.Commons.Utils
{
    public class FilePublisher : IPublisher
    {
        private readonly Func<string> _requestFileLocation;
        private readonly IIoUtils _ioUtils;

        public FilePublisher([NotNull] Func<string> requestFileLocation)
        {
            _requestFileLocation = requestFileLocation;
            _ioUtils = Registry.GetComponent<IIoUtils>();
        }

        public void Publish(MemoryStream stream)
        {
            var filename = _requestFileLocation();
            Asserts.Not(filename.IsNullOrEmpty(), Messages.NoFileGiven);

            try
            {
                _ioUtils.WriteAllByte(stream.ToArray(), filename);
            }
            catch (Exception e)
            {
                Asserts.Fail(Messages.PublishingFileFailed, e.Message);
            }
        }
    }
}