using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace AxisEngine.Debug
{
    public static class Log
    {
        private static int _maxMessageCount = 100;

        private static Queue<string> _logQueue = new Queue<string>(_maxMessageCount);

        private static bool ShowingWindow = false;

        private static System.Windows.Forms.RichTextBox LogTextSource;

        private static RichTextBox LogText;

        private static Window LogWindow;

        public static int MaxMessageCount
        {
            get
            {
                return _maxMessageCount;
            }
            set
            {
                _maxMessageCount = value;
            }
        }

        public static void CloseWindow()
        {
            if (ShowingWindow)
            {
                try
                {
                    LogWindow.Close();
                    LogTextSource = null;
                    LogWindow = null;
                    ShowingWindow = false;
                    LogWindow.Activated -= LogWindow_Activated;
                    LogWindow.Deactivated -= LogWindow_Deactivated;
                }
                catch (Exception) { }
            }
        }

        public static void ShowWindow()
        {
            if (!ShowingWindow)
            {
                try
                {
                    BuildWindow();
                    LogWindow.Show();
                    ShowingWindow = true;
                }
                catch (Exception) { }
            }
        }

        private static void BuildWindow()
        {
            LogWindow = new Window();
            LogWindow.Title = "Axis Engine Debug Log";
            LogWindow.Width = 480;
            LogWindow.Height = 860;
            LogWindow.WindowStartupLocation = WindowStartupLocation.Manual;
            LogWindow.Left = 10;
            LogWindow.Top = 20;
            LogWindow.Activated += LogWindow_Activated;
            LogWindow.Deactivated += LogWindow_Deactivated;

            LogTextSource = new System.Windows.Forms.RichTextBox();

            LogText = new RichTextBox();
            LogText.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            LogText.Margin = new Thickness(5);

            StackPanel stackPanel = new StackPanel();
            stackPanel.Children.Add(LogText);
            ScrollViewer scroller = new ScrollViewer();
            scroller.Content = stackPanel;
            scroller.ScrollToBottom();

            LogWindow.Content = scroller;
        }

        private static void LogWindow_Deactivated(object sender, EventArgs e)
        {
            LogText.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
        }

        private static void LogWindow_Activated(object sender, EventArgs e)
        {
            LogText.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        }

        public static void Write(int x)
        {
            Write(x.ToString());
        }

        public static void Write(float x)
        {
            Write(x.ToString());
        }

        public static void Write(string message)
        {
            // check to see if there are too many messages
            if (_logQueue.Count >= MaxMessageCount)
            {
                _logQueue.Dequeue();
            }

            // add the message to the queue
            _logQueue.Enqueue(message);

            // display the message
            if (ShowingWindow)
            {
                // some hackery to get the functionality of the Forms.RichTextBox and the display of the Controls.RichTextBox
                LogTextSource.Lines = _logQueue.ToArray();
                LogText.Document.Blocks.Clear();
                LogText.Document.Blocks.Add(new Paragraph(new Run(LogTextSource.Text)));
            }
        }
    }
}