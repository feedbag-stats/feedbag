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
using KaVE.Commons.Utils.Assertion;

namespace KaVE.RS.SolutionAnalysis
{
    public class MergingHistogram : Histogram
    {
        private readonly int _numSlots;

        public MergingHistogram(int numSlots) : base(numSlots)
        {
            _numSlots = numSlots;
        }

        public void AddRatio(int enumerator, int denominator)
        {
            Asserts.That(denominator >= _numSlots);

            var ratio = enumerator/(1.0*denominator);
            var slot = (int) Math.Ceiling(_numSlots*ratio);
            Add(slot);
        }
    }
}