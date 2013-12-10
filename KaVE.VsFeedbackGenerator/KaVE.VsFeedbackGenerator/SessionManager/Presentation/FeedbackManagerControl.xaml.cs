﻿using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KaVE.VsFeedbackGenerator.SessionManager.Presentation
{
    /// <summary>
    /// Interaktionslogik für SessionManagerControl.xaml
    /// </summary>
    public partial class SessionManagerControl
    {
        private readonly FeedbackView _feedbackView;

        public SessionManagerControl(FeedbackView holder)
        {
            _feedbackView = holder;
            DataContext = holder;
            InitializeComponent();
            
            // TODO: Move this to XAML if someone should find out how to do this
            SessionListView.Items.SortDescriptions.Add(new SortDescription("StartDate", ListSortDirection.Descending));
            EventListView.Items.SortDescriptions.Add(new SortDescription("StartTime", ListSortDirection.Descending));
        }

        private void FeedbackWindowControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            _feedbackView.RefreshSessions();
        }

        /// <summary>
        /// Makes the overflow dropdown button invisible.
        /// </summary>
        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            var toolBar = sender as ToolBar;
            if (toolBar == null)
            {
                return;
            }
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void SessionListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _feedbackView.SelectedSessions = SessionListView.SelectedItems.Cast<SessionView>();
        }

        private void EventListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_feedbackView.SingleSelectedSession != null)
            {
                _feedbackView.SingleSelectedSession.SelectedEvents = EventListView.SelectedItems.Cast<EventView>();
            }
        }

        private void RefreshButton_OnClick(object sender, RoutedEventArgs e)
        {
            _feedbackView.RefreshSessions();
        }

        private void VisitHomepageButton_OnClick(object sender, RoutedEventArgs e)
        {
            // TODO insert correct url here
            System.Diagnostics.Process.Start("http://kave-project.de");
        }
    }
}