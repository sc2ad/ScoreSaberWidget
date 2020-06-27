using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HtmlAgilityPack;

namespace ScoreSaber
{
    public struct UserInfo
    {
        public string FlagPath { get; }
        public string Username { get; }
        public uint GlobalRank { get; }
        public string CountryTag { get; }
        public uint CountryRank { get; }
        public double PerformancePoints { get; }
        public int PlayCount { get; }
        public int TotalScore { get; }
        public int ReplaysWatched { get; }

        private const int PP_Prefix = 20;
        private const int PlayCount_Prefix = 12;
        private const int TotalScore_Prefix = 13;
        private const int Replays_Prefix = 27;

        public UserInfo(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var parent = doc.DocumentNode.Descendants("section").FirstOrDefault()?.ParentNode;
            var h5 = parent.Descendants("h5").FirstOrDefault();
            FlagPath = h5?.Descendants("img").FirstOrDefault()?.GetAttributeValue("src", "");
            Username = h5?.Descendants("a").FirstOrDefault()?.InnerText.Trim();

            var ul = parent.Descendants("ul").FirstOrDefault();
            var lis = ul.Descendants("li").ToList();
            var liAs = lis.FirstOrDefault()?.Descendants("a").ToList();
            if (uint.TryParse(liAs.FirstOrDefault()?.InnerText.Trim().Substring(1), System.Globalization.NumberStyles.Any, null, out var gr))
                GlobalRank = gr;
            else
                GlobalRank = 0;
            var hrefA = liAs[1].GetAttributeValue("href", "").Trim();
            CountryTag = hrefA.Substring(hrefA.LastIndexOf('=') + 1);
            if (uint.TryParse(liAs[1].InnerText.Trim().Substring(1), System.Globalization.NumberStyles.Any, null, out var cr))
                CountryRank = cr;
            else
                CountryRank = 0;

            if (double.TryParse(lis[1].InnerText.Trim().Substring(PP_Prefix).TrimEnd('p'), System.Globalization.NumberStyles.Any, null, out var pp))
                PerformancePoints = pp;
            else
                PerformancePoints = 0;
            if (int.TryParse(lis[2].InnerText.Trim().Substring(PlayCount_Prefix), System.Globalization.NumberStyles.Any, null, out var pc))
                PlayCount = pc;
            else
                PlayCount = 0;
            if (int.TryParse(lis[3].InnerText.Trim().Substring(TotalScore_Prefix), System.Globalization.NumberStyles.Any, null, out var ts))
                TotalScore = ts;
            else
                TotalScore = 0;
            if (int.TryParse(lis[4].InnerText.Trim().Substring(Replays_Prefix), System.Globalization.NumberStyles.Any, null, out var rw))
                ReplaysWatched = rw;
            else
                ReplaysWatched = 0;
        }
    }
}