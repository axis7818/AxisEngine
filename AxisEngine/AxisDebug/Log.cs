using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace AxisEngine.AxisDebug
{
    public static class Log
    {
        public static void Write(string message, ConsoleColor? color = null)
        {
            if (color != null)
            {
                Console.ForegroundColor = color ?? Console.ForegroundColor;
                Console.Write(message);
                Console.ResetColor();
            }
            else
            {
                Console.Write(message);
            }
        }

        public static void WriteLine(string message, ConsoleColor? color = null)
        {
            Write(message + "\n", color);
        }
    }
}