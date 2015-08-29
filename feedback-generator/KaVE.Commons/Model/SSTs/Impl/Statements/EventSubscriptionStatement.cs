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

using KaVE.Commons.Model.SSTs.Expressions;
using KaVE.Commons.Model.SSTs.Impl.Expressions.Simple;
using KaVE.Commons.Model.SSTs.Impl.References;
using KaVE.Commons.Model.SSTs.References;
using KaVE.Commons.Model.SSTs.Statements;
using KaVE.Commons.Model.SSTs.Visitor;
using KaVE.Commons.Utils;

namespace KaVE.Commons.Model.SSTs.Impl.Statements
{
    public class EventSubscriptionStatement : IEventSubscriptionStatement
    {
        public IAssignableReference Reference { get; set; }
        public EventSubscriptionOperation Operation { get; set; }
        public IAssignableExpression Expression { get; set; }

        public EventSubscriptionStatement()
        {
            Reference = new UnknownReference();
            Expression = new UnknownExpression();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj, Equals);
        }

        private bool Equals(EventSubscriptionStatement other)
        {
            return Reference.Equals(other.Reference) && Operation == other.Operation &&
                   Expression.Equals(other.Expression);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Reference.GetHashCode();
                hashCode = (hashCode*397) ^ (int) Operation;
                hashCode = (hashCode*397) ^ Expression.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return this.ToStringReflection();
        }

        public void Accept<TContext>(ISSTNodeVisitor<TContext> visitor, TContext context)
        {
            visitor.Visit(this, context);
        }

        public TReturn Accept<TContext, TReturn>(ISSTNodeVisitor<TContext, TReturn> visitor, TContext context)
        {
            return visitor.Visit(this, context);
        }
    }
}