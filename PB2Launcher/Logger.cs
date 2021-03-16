using System;

namespace PB2Launcher {
    internal static class Logger {
        internal static void Log(string message) {
            var dateTime = DateTime.Now;
            
            Console.WriteLine($"[*] [{dateTime.ToShortDateString()}][{dateTime.ToLongTimeString()}] {message}");
        }

        internal static void Error(string message) {
            var dateTime = DateTime.Now;
            
            Console.WriteLine($"[-] [{dateTime.ToShortDateString()}][{dateTime.ToLongTimeString()}] {message}");
        }
    }
}