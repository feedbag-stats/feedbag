﻿using System.ComponentModel.Composition;
using CodeCompletion.Model.Events.VisualStudio;
using EnvDTE;
using EventGenerator.Commons;
using KAVE.KAVE_MessageBus.MessageBus;

namespace KAVE.EventGenerator_VisualStudio10.Generators
{
    [Export(typeof (VisualStudioEventGenerator))]
    internal class IDEStartupStateEventGenerator : VisualStudioEventGenerator
    {
        public IDEStartupStateEventGenerator(DTE dte, SMessageBus messageBus) : base(dte, messageBus) {}

        public override void Initialize()
        {
            // TODO defer this until IDE is actually loaded!
            // TODO add IDE shutdown event
            var ideStateEvent = Create<IDEStartupStateEvent>();
            ideStateEvent.OpenWindows = VsComponentNameFactory.GetNamesOf(DTE.Windows);
            ideStateEvent.OpenDocuments = VsComponentNameFactory.GetNamesOf(DTE.Documents);
            Fire(ideStateEvent);
        }
    }
}
