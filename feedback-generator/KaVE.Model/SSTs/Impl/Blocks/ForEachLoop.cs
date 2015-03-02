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
 *    - Sebastian Proksch
 */

using System.Collections.Generic;
using KaVE.Model.Collections;
using KaVE.Model.SSTs.Blocks;
using KaVE.Model.SSTs.Declarations;
using KaVE.Model.SSTs.References;
using KaVE.Model.SSTs.Visitor;
using KaVE.Utils;

namespace KaVE.Model.SSTs.Impl.Blocks
{
    public class ForEachLoop : IForEachLoop
    {
        public IVariableDeclaration Declaration { get; set; }
        public IVariableReference LoopedReference { get; set; }
        public IList<IStatement> Body { get; set; }

        public ForEachLoop()
        {
            Body = Lists.NewList<IStatement>();
        }

        private bool Equals(ForEachLoop other)
        {
            return Body.Equals(other.Body) && Equals(Declaration, other.Declaration) &&
                   Equals(LoopedReference, other.LoopedReference);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj, Equals);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 33 + Body.GetHashCode();
                hashCode = (hashCode*397) ^ (Declaration != null ? Declaration.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (LoopedReference != null ? LoopedReference.GetHashCode() : 0);
                return hashCode;
            }
        }

        public void Accept<TContext>(ISSTNodeVisitor<TContext> visitor, TContext context)
        {
            visitor.Visit(this, context);
        }
    }
}