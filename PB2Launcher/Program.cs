using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Collections.Generic;

namespace PB2Launcher {
    internal static class Program {
        internal static void Main(string[] args) {
            var platform = GetPlatform();

            var defaultTitle = $"PB2Launcher - {Enum.GetName(typeof(Platform), platform)}";

            Console.Title = defaultTitle;

            if (platform == Platform.Unknown) {
                Console.WriteLine("[-] unknown platform, cannot proceed!");

                return;
            }

            var download = _flashDownloads[platform];

            var dataDirectory = Path.Join(AppContext.BaseDirectory, "data");

            if (!Directory.Exists(dataDirectory)) {
                Directory.CreateDirectory(dataDirectory);
            }

            string filename = Path.Join(dataDirectory, GetFileName(platform));

            string flash = filename;

            if (!File.Exists(filename)) {
                Logger.Log("downloading FlashPlayer!");

                DownloadFile(defaultTitle, download, filename);
            }

            filename = Path.Join(dataDirectory, "pb2_re34_alt.swf");

            string swf = filename;
            
            if (!File.Exists(filename)) {
                Logger.Log("downloading PB2 Game File!");

                DownloadFile(defaultTitle, _swfDownload, filename);
            }

            Logger.Log("starting PB2 :D");

            try {
                if (platform == Platform.Linux) {
                    Process.Start("chmod", $"+x {flash}");
                }
                
                Process.Start(flash, swf);
            }
            catch (Exception exception) {
                Logger.Error(exception.ToString());
                
                Logger.Error("please delete the data folder and re run this program!");
            }
            
            Logger.Log("press any key to close this window!");

            Console.ReadKey();
        }

        private static void DownloadFile(string defaultTitle, string url, string path) {
            using var client = new WebClient();

            var fileName = Path.GetFileName(path);

            client.DownloadProgressChanged += (sender, args) => {
                Console.Title = $"{defaultTitle} - downloading {fileName} : {args.ProgressPercentage}% complete!";
                    
                Logger.Log($"downloading {args.BytesReceived}/{args.TotalBytesToReceive} / {args.ProgressPercentage}% completed!");
            };

            client.DownloadFileCompleted += (sender, args) => {
                Console.Title = defaultTitle;

                if (args.Error is not null) {
                    Logger.Error(args.Error.ToString());
                    
                    File.Delete(path);

                    Logger.Error("download failed!");
                    
                    Logger.Log("press any key to close this window!");
                    
                    Console.ReadKey();

                    Environment.Exit(1);
                }
                else {
                    Logger.Log("download complete!");
                }
            };

            // callbacks are not invoked when not calling async so we setup
            // an awaiter and run synchronously
            
            client.DownloadFileTaskAsync(new Uri(url), path).GetAwaiter().GetResult();
        }

        private static string GetFileName(Platform platform) {
            switch (platform) {
                case Platform.Windows:
                    return "FlashPlayer.exe";
                
                case Platform.Linux:
                    return "FlashPlayer";
                
                case Platform.Macos:
                    return "FlashPlayer.dmg";
                
                default:
                    return string.Empty;
            }
        }

        private static Platform GetPlatform() {
            if (OperatingSystem.IsWindows())
                return Platform.Windows;

            if (OperatingSystem.IsLinux())
                return Platform.Linux;

            if (OperatingSystem.IsMacOS())
                return Platform.Macos;

            return Platform.Unknown;
        }

        private static Dictionary<Platform, string> _flashDownloads = new() {
            [Platform.Windows] = "https://fpdownload.macromedia.com/pub/flashplayer/updaters/32/flashplayer_32_sa.exe",
            [Platform.Linux]   = "https://fpdownload.macromedia.com/pub/flashplayer/updaters/32/flashplayer_32_sa.dmg",
            [Platform.Macos]   = "https://fpdownload.macromedia.com/pub/flashplayer/updaters/32/flash_player_sa_linux_debug.x86_64.tar.gz",
            [Platform.Unknown] = string.Empty
        };

        private const string _swfDownload = "https://www.plazmaburst2.com/pb2/pb2_re34.swf";
    }
}