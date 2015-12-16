using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TVPlaylist.Crawler
{
    class ChannelExtractor
    {
        private const string ExtractTorFrameRegEx = @"<iframe.*?src=\\""(?<url>.*?)\\""";
        private const string AceStreamIdRegEx = @"this\.loadPlayer\(""(?<id>.*?)""";
        private const string ChannelNameRegEx = @"<h1>(?<title>.*?)<\/h1>";

        public Channel ExtractAceStreamId(string channelUrl)
        {
            WebClient client = new WebClient();
            Channel result = new Channel();

            byte[] channelData = client.DownloadData(channelUrl);
            string channelContent = System.Text.Encoding.UTF8.GetString(channelData);

            Regex findTitle = new Regex(ChannelNameRegEx);
            var mTitle = findTitle.Match(channelContent);
            if (mTitle != null && mTitle.Groups["title"] != null)
                result.Name = mTitle.Groups["title"].Value;

            Regex findUrl = new Regex(ExtractTorFrameRegEx);

            var m = findUrl.Match(channelContent);
            if (m != null && m.Groups["url"] != null)
            {
                var url = m.Groups["url"].Value;
                var frameContent = client.DownloadString(url);
                Regex findId = new Regex(AceStreamIdRegEx);
                var idm = findId.Match(frameContent);

                if (idm != null && idm.Groups["id"] != null)
                {
                    result.ContentId = idm.Groups["id"].Value;
                    return result; ;
                }
            }

            return null;
        }
    }
}
