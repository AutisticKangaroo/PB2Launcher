using System;

namespace PB2Launcher {
    internal static class Logger {
        internal static void Log(string message, bool newline = true) {
            var dateTime = DateTime.Now;

            message = $"[*] [{dateTime.ToShortDateString()}][{dateTime.ToLongTimeString()}] {message}";
            
            if (newline) {
                Console.WriteLine(message);
            }
            else {
                Console.Write(message);
            }
        }

        internal static void Error(string message, bool newline = true) {
            var dateTime = DateTime.Now;

            message = $"[-] [{dateTime.ToShortDateString()}][{dateTime.ToLongTimeString()}] {message}";

            if (newline) {
                Console.WriteLine(message);
            }
            else {
                Console.Write(message);
            }
        }
    }
}