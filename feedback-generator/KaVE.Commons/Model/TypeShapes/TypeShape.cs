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

using System.Runtime.Serialization;
using KaVE.Commons.Model.Naming.CodeElements;
using KaVE.Commons.Model.Naming.Types;
using KaVE.Commons.Utils;
using KaVE.Commons.Utils.Collections;

namespace KaVE.Commons.Model.TypeShapes
{
    [DataContract]
    public class TypeShape : ITypeShape
    {
        [DataMember]
        public ITypeHierarchy TypeHierarchy { get; set; }

        [DataMember]
        public IKaVESet<ITypeHierarchy> NestedTypes { get; set; }

        [DataMember]
        public IKaVESet<IDelegateTypeName> Delegates { get; set; }

        [DataMember]
        public IKaVESet<IHierarchy<IEventName>> EventHierarchies { get; set; }

        [DataMember]
        public IKaVESet<IFieldName> Fields { get; set; }

        [DataMember]
        public IKaVESet<IHierarchy<IMethodName>> MethodHierarchies { get; set; }

        [DataMember]
        public IKaVESet<IHierarchy<IPropertyName>> PropertyHierarchies { get; set; }

        public TypeShape()
        {
            TypeHierarchy = new TypeHierarchy();
            NestedTypes = Sets.NewHashSet<ITypeHierarchy>();
            Delegates = Sets.NewHashSet<IDelegateTypeName>();
            EventHierarchies = Sets.NewHashSet<IHierarchy<IEventName>>();
            Fields = Sets.NewHashSet<IFieldName>();
            MethodHierarchies = Sets.NewHashSet<IHierarchy<IMethodName>>();
            PropertyHierarchies = Sets.NewHashSet<IHierarchy<IPropertyName>>();
        }

        private bool Equals(TypeShape other)
        {
            return Equals(TypeHierarchy, other.TypeHierarchy) &&
                   Equals(NestedTypes, other.NestedTypes) &&
                   Equals(Delegates, other.Delegates) &&
                   Equals(EventHierarchies, other.EventHierarchies) &&
                   Equals(Fields, other.Fields) &&
                   Equals(MethodHierarchies, other.MethodHierarchies) &&
                   Equals(PropertyHierarchies, other.PropertyHierarchies);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj, Equals);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (TypeHierarchy.GetHashCode()*397) ^
                       NestedTypes.GetSetHashCode() ^
                       Delegates.GetSetHashCode() ^
                       EventHierarchies.GetSetHashCode() ^
                       Fields.GetSetHashCode() ^
                       MethodHierarchies.GetSetHashCode() ^
                       PropertyHierarchies.GetSetHashCode();
            }
        }

        public override string ToString()
        {
            return this.ToStringReflection();
        }
    }
}