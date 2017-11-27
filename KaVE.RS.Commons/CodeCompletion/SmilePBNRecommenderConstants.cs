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
using KaVE.Commons.Model.ObjectUsage;
using KaVE.JetBrains.Annotations;

namespace KaVE.RS.Commons.CodeCompletion
{
    public class SmilePBNRecommenderConstants
    {
        public const string PatternTitle = "patterns";
        public const string ClassContextTitle = "inClass";
        public const string MethodContextTitle = "inMethod";
        public const string DefinitionTitle = "definition";

        public const string StateTrue = "t";
        public const string StateFalse = "f";

        public const string CallPrefix = "C_";
        public const string ParameterPrefix = "P_";

        public static string NewClassContext([NotNull] CoReTypeName typeName)
        {
            return typeName.Name;
        }

        public static string NewMethodContext([NotNull] CoReMethodName methodName)
        {
            return methodName.Name;
        }

        public static string NewDefinition([NotNull] DefinitionSite definition)
        {
            switch (definition.kind)
            {
                case DefinitionSiteKind.RETURN:
                    return string.Format("{0}:{1}", definition.kind, definition.method.Name);
                case DefinitionSiteKind.NEW:
                    return string.Format("INIT:{0}", definition.method.Name);
                case DefinitionSiteKind.PARAM:
                    return string.Format("{0}({1}):{2}", definition.kind, definition.argIndex, definition.method.Name);
                case DefinitionSiteKind.FIELD:
                    return string.Format("{0}:{1}", definition.kind, definition.field.Name);
                case DefinitionSiteKind.THIS:
                case DefinitionSiteKind.CONSTANT:
                case DefinitionSiteKind.UNKNOWN:
                    return definition.kind.ToString();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static string NewParameterSite([NotNull] CallSite site)
        {
            return string.Format("{0}{1}#{2}", ParameterPrefix, site.method.Name, site.argIndex);
        }

        public static string NewReceiverCallSite([NotNull] CallSite site)
        {
            return string.Format("{0}{1}", CallPrefix, site.method.Name);
        }
    }
}