using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TVPlaylist.Crawler
{
    [XmlType("config")]
    public class TVConfig
    {
        [XmlArray("channels"), XmlArrayItem("channel")]
        public string[] Channels;
    }
}
