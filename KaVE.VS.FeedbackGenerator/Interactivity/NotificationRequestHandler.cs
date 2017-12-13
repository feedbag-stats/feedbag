/*
 * Copyright 2014 Technische Universit�t Darmstadt
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

using System.Windows;

namespace KaVE.VS.FeedbackGenerator.Interactivity
{
    public class NotificationRequestHandler
    {
        private readonly Window _window;

        public NotificationRequestHandler(DependencyObject parent)
        {
            _window = Window.GetWindow(parent);
        }

        public void Handle(object sender, InteractionRequestedEventArgs<Notification> args)
        {
            var notification = args.Notification;

            if (_window == null)
            {
                MessageBox.Show(
                    notification.Message,
                    notification.Caption,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information,
                    MessageBoxResult.OK);
            }
            else
            {
                MessageBox.Show(
                    _window,
                    notification.Message,
                    notification.Caption,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information,
                    MessageBoxResult.OK);
            }
        }
    }
}