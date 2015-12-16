using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVPlaylist.Crawler
{
    class M3UGenerator
    {
        private const string ItemTemplate = "#EXTINF:0,{1}\r\n#EXTVLCOPT:network-caching=1000\r\n{0}\r\n";
        public string Create(IEnumerable<Channel> items)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("#EXTM3U\r\n");
            foreach (var s in items)
                sb.Append(string.Format(ItemTemplate, s.ContentId, s.Name));

            return sb.ToString();
        }
    }
}
