using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

using iTeco.Lib.Base;

namespace TVPlaylist.Crawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[export begin]");
            var config = File.ReadAllText("Config.Xml").ToXmlReader().Deserialize<TVConfig>();
            var extractor = new ChannelExtractor();

            List<Channel> ids = new List<Channel>();

            foreach(var url in config.Channels)
            {
                Console.WriteLine("[extract] " + url);
                var id = extractor.ExtractAceStreamId(url);
                if (id!=null)
                    ids.Add(id);

                Console.WriteLine("[find] " + id);
            }

            string playList = new M3UGenerator().Create(ids);
            File.WriteAllText(@"C:\Users\ai\Documents\TV.m3u", playList,Encoding.UTF8);

            Console.WriteLine("[export done]");
            Console.ReadKey();
        }


    }
}
