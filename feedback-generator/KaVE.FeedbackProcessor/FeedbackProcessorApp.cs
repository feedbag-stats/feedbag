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
using System.Linq;
using KaVE.Commons.Utils.Exceptions;

namespace KaVE.FeedbackProcessor
{
    internal class FeedbackProcessorApp
    {
        private static readonly ILogger Logger = new ConsoleLogger();

        public static void Main()
        {
            //new SanityCheckApp().Run();

            //new TimeBudgetEvaluationApp(Logger).Run();
            //new SSTSequenceExtractor(Logger).Run();

            //var documentEventFilter =
            //    EventStreamFilter.DocumentEventFilter(@"\KaVE.Commons\Utils\ObjectUsageExport\ScopedEnclosings.cs");
            //var activeDocumentFilter =
            //    EventStreamFilter.ActiveDocumentFilter(@"\KaVE.Commons\Utils\ObjectUsageExport\ScopedEnclosings.cs");

            //var events = new EventStreamFilter(e => documentEventFilter(e) || activeDocumentFilter(e))
            //    .Filter("C:/Users/Andreas/Desktop/OSS-Events/target/be8f9fdb-d75e-4ec1-8b54-7b57bd47706a.zip").ToList();

            //var events = new EventStreamFilter(EventStreamFilter.TimeBoxFilter("02.11.2015 11:03:31", "02.11.2015 11:03:59"))
            //    .Filter("C:/Users/Andreas/Desktop/OSS-Events/target/be8f9fdb-d75e-4ec1-8b54-7b57bd47706a.zip").ToList();

            new IntervalTransformerApp(Logger).Run(
                "C:/Users/Andreas/Desktop/OSS-Events/target/be8f9fdb-d75e-4ec1-8b54-7b57bd47706a.zip");

            Console.ReadKey();
        }
    }
}