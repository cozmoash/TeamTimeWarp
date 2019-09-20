using System;
using EnvDTE;

namespace TeamTimeWarp.TeamTimeWarp_VsPackage
{
    public class DteTrigger : IDteTrigger
    {
        private readonly DTE _dte;

        private readonly SolutionEvents _solutionEvents;
        private readonly SelectionEvents _selectionEvents;
        private readonly TextEditorEvents _textEditorEvents;

        public event EventHandler<EventArgs> OnDteTrigger;

        public DteTrigger(DTE dte)
        {
            _dte = dte;

            _textEditorEvents = _dte.Events.TextEditorEvents;
            _solutionEvents = _dte.Events.SolutionEvents;
            _selectionEvents = _dte.Events.SelectionEvents;

            _textEditorEvents.LineChanged += HandleTextEditorLineChanged;
            _solutionEvents.Opened += HandleSolutionEventsOnOpened;
            _selectionEvents.OnChange += HandleSelectionEventsOnChange;
        }

        private void HandleSelectionEventsOnChange()
        {
            OnOnDteTrigger();
        }

        private void HandleSolutionEventsOnOpened()
        {
            OnOnDteTrigger();
        }

        private void HandleTextEditorLineChanged(TextPoint startPoint, TextPoint endPoint, int hint)
        {
            OnOnDteTrigger();
        }


        protected virtual void OnOnDteTrigger()
        {
            EventHandler<EventArgs> handler = OnDteTrigger;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            _textEditorEvents.LineChanged -= HandleTextEditorLineChanged;
            _solutionEvents.Opened -= HandleSolutionEventsOnOpened;
            _selectionEvents.OnChange -= HandleSelectionEventsOnChange;
        }
    }
}