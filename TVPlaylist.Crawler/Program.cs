using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

using iTeco.Lib.Base;
using System.Net;
using System.Runtime.InteropServices;

namespace TVPlaylist.Crawler
{
    class Program
    {
        [DllImport("user32")]
        private static extern bool SetForegroundWindow(IntPtr hwnd);

        static void Main(string[] args)
        {
            try
            {
                TVConfig config = LoadConfig(args);
                GrabChannels(config);
                StartApp(config);
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Error:");
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
        }

        private static void StartApp(TVConfig config)
        {
            Console.WriteLine("StartApp ...");
            if (!string.IsNullOrEmpty(config.StartApp))
            { 
                var proc = System.Diagnostics.Process.Start(config.StartApp.Replace("{output}", config.Output), config.StartAppArgs?.Replace("{output}", config.Output));
                SetForegroundWindow(proc.MainWindowHandle);
            }

            Console.WriteLine("StartApp done");
        }

        private static void GrabChannels(TVConfig config)
        {
            Console.WriteLine("GrabChannels ...");

            var extractor = new ChannelExtractor();

            List<Channel> ids = new List<Channel>();

            foreach (var url in config.Channels)
            {
                Console.WriteLine("[extract] " + url);
                try
                {
                    var id = extractor.ExtractAceStreamId(url);
                    if (id != null)
                        ids.Add(id);
                    Console.WriteLine("[find] " + id);
                }
                catch(System.Net.WebException ex)
                {
                    if (ex.Response != null && (ex.Response as HttpWebResponse).StatusCode == HttpStatusCode.NotFound)
                    {
                        Console.WriteLine("[error] Page Not Found");
                        Console.ReadKey();
                    }
                    else
                        throw;
                }
            }

            string playList = new M3UGenerator().Create(ids);
            File.WriteAllText(config.Output, playList, Encoding.UTF8);

            Console.WriteLine("GrabChannels done");
        }

        private static TVConfig LoadConfig(string[] args)
        {
            string configFile = args != null && args.Length > 0 ? args[0] : "Config.Xml";

            Console.WriteLine("Config file: " + configFile);

            return File.ReadAllText("Config.Xml").ToXmlReader().Deserialize<TVConfig>();
        }
    }
}
