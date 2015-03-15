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
 *    - Sebastian Proksch
 */

using System.Linq;
using KaVE.Model.Collections;
using KaVE.Model.TypeShapes;

namespace KaVE.VsFeedbackGenerator.SessionManager.Anonymize.CompletionEvents
{
    public class TypeShapeAnonymizer
    {
        public ITypeShape Anonymize(ITypeShape typeShape)
        {
            return new TypeShape
            {
                TypeHierarchy = AnonymizeCodeNames(typeShape.TypeHierarchy),
                MethodHierarchies = Sets.NewHashSetFrom(typeShape.MethodHierarchies.Select(AnonymizeCodeNames))
            };
        }

        private static ITypeHierarchy AnonymizeCodeNames(ITypeHierarchy raw)
        {
            if (raw == null)
            {
                return null;
            }

            return new TypeHierarchy
            {
                Element = raw.Element.ToAnonymousName(),
                Extends = AnonymizeCodeNames(raw.Extends),
                Implements = Sets.NewHashSetFrom(raw.Implements.Select(AnonymizeCodeNames))
            };
        }

        private static IMethodHierarchy AnonymizeCodeNames(IMethodHierarchy raw)
        {
            return new MethodHierarchy
            {
                Element = raw.Element.ToAnonymousName(),
                Super = raw.Super.ToAnonymousName(),
                First = raw.First.ToAnonymousName()
            };
        }
    }
}