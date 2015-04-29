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
 *    - Mattis Manfred Kämmerer
 *    - Markus Zimmermann
 */

using KaVE.Commons.Model.Events;
using KaVE.Commons.Model.Events.VisualStudio;

namespace KaVE.FeedbackProcessor.Cleanup.Processors
{
    internal class AddFileProcessor : BaseProcessor
    {
        private bool _addedNewItem;

        public AddFileProcessor()
        {
            RegisterFor<CommandEvent>(ProcessCommandEvent);
            RegisterFor<DocumentEvent>(ProcessDocumentEvent);
        }

        private void ProcessCommandEvent(CommandEvent commandEvent)
        {
            if (commandEvent.CommandId != null)
            {
                _addedNewItem = IsAddNewItemCommand(commandEvent.CommandId);
            }
        }

        private static bool IsAddNewItemCommand(string commandId)
        {
            return commandId.Contains("AddNewItem") ||
                   commandId.Contains("Project.AddClass") ||
                   commandId.Equals("Class");
        }

        private void ProcessDocumentEvent(DocumentEvent documentEvent)
        {
            if (_addedNewItem)
            {
                _addedNewItem = false;

                var solutionEvent = new SolutionEvent
                {
                    Action = SolutionEvent.SolutionAction.AddProjectItem,
                    Target = documentEvent.Document
                };
                solutionEvent.CopyIDEEventPropertiesFrom(documentEvent);
                Insert(solutionEvent);
            }
        }
    }
}