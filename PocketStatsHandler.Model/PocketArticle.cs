using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PocketStatsHandler.Contracts;

namespace PocketStatsHandler.Model
{
    public class PocketArticle : IPocketArticle
    {
        public string ItemId { get; set; }
        public string ResolvedId { get; set; }
        public string GivenUrl { get; set; }
        public string GivenTitle { get; set; }
        public string Favorite { get; set; }
        public string Status { get; set; }
        public string TimeAdded { get; set; }
        public string TimeUpdated { get; set; }
        public string TimeRead { get; set; }
        public string TimeFavorited { get; set; }
        public int SortId { get; set; }
        public string ResolvedTitle { get; set; }
        public string ResolvedUrl { get; set; }
        public string Excerpt { get; set; }
        public string IsArticle { get; set; }
        public string IsIndex { get; set; }
        public string HasVideo { get; set; }
        public string HasImage { get; set; }
        public string WordCount { get; set; }
    }
}
