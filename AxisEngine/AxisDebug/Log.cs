using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;

namespace AxisEngine.Debug
{
    /// <summary>
    /// provides a mechanism to log to an output window
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// The capacity of the log container used
        /// </summary>
        private static int _maxMessageCount = 100;

        /// <summary>
        /// holds the logs
        /// </summary>
        private static Queue<string> _logQueue = new Queue<string>(_maxMessageCount);

        /// <summary>
        /// whether or not the log window is showing
        /// </summary>
        private static bool ShowingWindow = false;

        /// <summary>
        /// the text source
        /// </summary>
        private static System.Windows.Forms.RichTextBox LogTextSource;

        /// <summary>
        /// the log text
        /// </summary>
        private static RichTextBox LogText;

        /// <summary>
        /// the log window
        /// </summary>
        private static Window LogWindow;

        /// <summary>
        /// The capacity of the log container used
        /// </summary>
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

        /// <summary>
        /// Closes the Log Window
        /// </summary>
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

        /// <summary>
        /// Launches the Log Window
        /// </summary>
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

        /// <summary>
        /// builds up the GUI for the window
        /// </summary>
        private static void BuildWindow()
        {
            LogWindow = new Window();
            LogWindow.Title = "Axis Engine Debug Log";
            LogWindow.Width = 480;
            LogWindow.Height = 860;
            LogWindow.WindowStartupLocation = WindowStartupLocation.Manual;
            LogWindow.Left = 20;
            LogWindow.Top = 200;
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

        /// <summary>
        /// handles the deactivation of the log window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void LogWindow_Deactivated(object sender, EventArgs e)
        {
            LogText.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
        }

        /// <summary>
        /// handles the activation of the log window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void LogWindow_Activated(object sender, EventArgs e)
        {
            LogText.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        }

        /// <summary>
        /// writes an integer to the log window
        /// </summary>
        /// <param name="x">the integer to write</param>
        public static void Write(int x)
        {
            Write(x.ToString());
        }

        /// <summary>
        /// writes a float to the log window
        /// </summary>
        /// <param name="x">the float to write</param>
        public static void Write(float x)
        {
            Write(x.ToString());
        }

        /// <summary>
        /// writes a string message to the log window
        /// </summary>
        /// <param name="message">the message to write</param>
        public static void Write(string message)
        {
            // check to see if there are too many messages
            if(_logQueue.Count >= MaxMessageCount)
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