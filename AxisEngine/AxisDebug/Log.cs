using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace AxisEngine.Debug
{
    //public static class Log
    //{
    //    private static int MAX_MESSAGE_COUNT = 100;

    //    private static Queue<string> _logQueue = new Queue<string>(MAX_MESSAGE_COUNT);
    //    private static bool _showingWindow = false;
    //    private static System.Windows.Forms.RichTextBox _logTextSource;
    //    private static RichTextBox _logText;
    //    private static Window _logWindow;

    //    public static int MaxMessageCount
    //    {
    //        get
    //        {
    //            return MAX_MESSAGE_COUNT;
    //        }
    //        set
    //        {
    //            MAX_MESSAGE_COUNT = value;
    //        }
    //    }

    //    public static void CloseWindow()
    //    {
    //        if (_showingWindow)
    //        {
    //            try
    //            {
    //                _logWindow.Close();
    //                _logTextSource = null;
    //                _logWindow = null;
    //                _showingWindow = false;
    //                _logWindow.Activated -= _logWindow_Activated;
    //                _logWindow.Deactivated -= _logWindow_Deactivated;
    //            }
    //            catch (Exception) { }
    //        }
    //    }

    //    public static void ShowWindow()
    //    {
    //        if (!_showingWindow)
    //        {
    //            try
    //            {
    //                BuildWindow();
    //                _logWindow.Show();
    //                _showingWindow = true;
    //            }
    //            catch (Exception) { }
    //        }
    //    }

    //    private static void BuildWindow()
    //    {
    //        _logWindow = new Window();
    //        _logWindow.Title = "Axis Engine Debug Log";
    //        _logWindow.Width = 480;
    //        _logWindow.Height = 860;
    //        _logWindow.WindowStartupLocation = WindowStartupLocation.Manual;
    //        _logWindow.Left = 10;
    //        _logWindow.Top = 20;
    //        _logWindow.Activated += _logWindow_Activated;
    //        _logWindow.Deactivated += _logWindow_Deactivated;

    //        _logTextSource = new System.Windows.Forms.RichTextBox();

    //        _logText = new RichTextBox();
    //        _logText.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
    //        _logText.Margin = new Thickness(5);

    //        StackPanel stackPanel = new StackPanel();
    //        stackPanel.Children.Add(_logText);
    //        ScrollViewer scroller = new ScrollViewer();
    //        scroller.Content = stackPanel;
    //        scroller.ScrollToBottom();

    //        _logWindow.Content = scroller;
    //    }

    //    private static void _logWindow_Deactivated(object sender, EventArgs e)
    //    {
    //        _logText.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
    //    }

    //    private static void _logWindow_Activated(object sender, EventArgs e)
    //    {
    //        _logText.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
    //    }

    //    public static void Write(int x)
    //    {
    //        Write(x.ToString());
    //    }

    //    public static void Write(float x)
    //    {
    //        Write(x.ToString());
    //    }

    //    public static void Write(string message)
    //    {
    //        // check to see if there are too many messages
    //        if (_logQueue.Count >= MaxMessageCount)
    //        {
    //            _logQueue.Dequeue();
    //        }

    //        // add the message to the queue
    //        _logQueue.Enqueue(message);

    //        // display the message
    //        if (_showingWindow)
    //        {
    //            // some hackery to get the functionality of the Forms.RichTextBox and the display of the Controls.RichTextBox
    //            _logTextSource.Lines = _logQueue.ToArray();
    //            _logText.Document.Blocks.Clear();
    //            _logText.Document.Blocks.Add(new Paragraph(new Run(_logTextSource.Text)));
    //        }
    //    }
    //}
}