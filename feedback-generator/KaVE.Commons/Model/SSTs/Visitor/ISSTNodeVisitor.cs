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

using KaVE.Model.SSTs.Blocks;
using KaVE.Model.SSTs.Declarations;
using KaVE.Model.SSTs.Expressions.Assignable;
using KaVE.Model.SSTs.Expressions.LoopHeader;
using KaVE.Model.SSTs.Expressions.Simple;
using KaVE.Model.SSTs.References;
using KaVE.Model.SSTs.Statements;

namespace KaVE.Model.SSTs.Visitor
{
    public interface ISSTNodeVisitor<in TContext>
    {
        void Visit(ISST sst, TContext context);

        // declarations
        void Visit(IDelegateDeclaration stmt, TContext context);
        void Visit(IEventDeclaration stmt, TContext context);
        void Visit(IFieldDeclaration stmt, TContext context);
        void Visit(IMethodDeclaration stmt, TContext context);
        void Visit(IPropertyDeclaration stmt, TContext context);
        void Visit(IVariableDeclaration stmt, TContext context);

        // statements
        void Visit(IAssignment stmt, TContext context);
        void Visit(IBreakStatement stmt, TContext context);
        void Visit(IContinueStatement stmt, TContext context);
        void Visit(IExpressionStatement stmt, TContext context);
        void Visit(IGotoStatement stmt, TContext context);
        void Visit(ILabelledStatement stmt, TContext context);
        void Visit(IReturnStatement stmt, TContext context);
        void Visit(IThrowStatement stmt, TContext context);

        // blocks
        void Visit(IDoLoop block, TContext context);
        void Visit(IForEachLoop block, TContext context);
        void Visit(IForLoop block, TContext context);
        void Visit(IIfElseBlock block, TContext context);
        void Visit(ILockBlock stmt, TContext context);
        void Visit(ISwitchBlock block, TContext context);
        void Visit(ITryBlock block, TContext context);
        void Visit(IUncheckedBlock block, TContext context);
        void Visit(IUnsafeBlock block, TContext context);
        void Visit(IUsingBlock block, TContext context);
        void Visit(IWhileLoop block, TContext context);

        // Expressions
        void Visit(ICompletionExpression entity, TContext context);
        void Visit(IComposedExpression expr, TContext context);
        void Visit(IIfElseExpression expr, TContext context);
        void Visit(IInvocationExpression entity, TContext context);
        void Visit(ILambdaExpression expr, TContext context);
        void Visit(ILoopHeaderBlockExpression expr, TContext context);
        void Visit(IConstantValueExpression expr, TContext context);
        void Visit(INullExpression expr, TContext context);
        void Visit(IReferenceExpression expr, TContext context);

        // References
        void Visit(IEventReference eventRef, TContext context);
        void Visit(IFieldReference fieldRef, TContext context);
        void Visit(IMethodReference methodRef, TContext context);
        void Visit(IPropertyReference methodRef, TContext context);
        void Visit(IVariableReference varRef, TContext context);

        // unknowns
        void Visit(IUnknownReference unknownRef, TContext context);
        void Visit(IUnknownExpression unknownExpr, TContext context);
        void Visit(IUnknownStatement unknownStmt, TContext context);
    }

    public interface ISSTNodeVisitor<in TContext, out TReturn>
    {
        TReturn Visit(ISST sst, TContext context);

        // declarations
        TReturn Visit(IDelegateDeclaration stmt, TContext context);
        TReturn Visit(IEventDeclaration stmt, TContext context);
        TReturn Visit(IFieldDeclaration stmt, TContext context);
        TReturn Visit(IMethodDeclaration stmt, TContext context);
        TReturn Visit(IPropertyDeclaration stmt, TContext context);
        TReturn Visit(IVariableDeclaration stmt, TContext context);

        // statements
        TReturn Visit(IAssignment stmt, TContext context);
        TReturn Visit(IBreakStatement stmt, TContext context);
        TReturn Visit(IContinueStatement stmt, TContext context);
        TReturn Visit(IExpressionStatement stmt, TContext context);
        TReturn Visit(IGotoStatement stmt, TContext context);
        TReturn Visit(ILabelledStatement stmt, TContext context);
        TReturn Visit(IReturnStatement stmt, TContext context);
        TReturn Visit(IThrowStatement stmt, TContext context);

        // blocks
        TReturn Visit(IDoLoop block, TContext context);
        TReturn Visit(IForEachLoop block, TContext context);
        TReturn Visit(IForLoop block, TContext context);
        TReturn Visit(IIfElseBlock block, TContext context);
        TReturn Visit(ILockBlock stmt, TContext context);
        TReturn Visit(ISwitchBlock block, TContext context);
        TReturn Visit(ITryBlock block, TContext context);
        TReturn Visit(IUncheckedBlock block, TContext context);
        TReturn Visit(IUnsafeBlock block, TContext context);
        TReturn Visit(IUsingBlock block, TContext context);
        TReturn Visit(IWhileLoop block, TContext context);

        // Expressions
        TReturn Visit(ICompletionExpression entity, TContext context);
        TReturn Visit(IComposedExpression expr, TContext context);
        TReturn Visit(IIfElseExpression expr, TContext context);
        TReturn Visit(IInvocationExpression entity, TContext context);
        TReturn Visit(ILambdaExpression expr, TContext context);
        TReturn Visit(ILoopHeaderBlockExpression expr, TContext context);
        TReturn Visit(IConstantValueExpression expr, TContext context);
        TReturn Visit(INullExpression expr, TContext context);
        TReturn Visit(IReferenceExpression expr, TContext context);

        // References
        TReturn Visit(IEventReference eventRef, TContext context);
        TReturn Visit(IFieldReference fieldRef, TContext context);
        TReturn Visit(IMethodReference methodRef, TContext context);
        TReturn Visit(IPropertyReference methodRef, TContext context);
        TReturn Visit(IVariableReference varRef, TContext context);

        // unknowns
        TReturn Visit(IUnknownReference unknownRef, TContext context);
        TReturn Visit(IUnknownExpression unknownExpr, TContext context);
        TReturn Visit(IUnknownStatement unknownStmt, TContext context);
    }
}