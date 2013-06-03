using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PocketStatsHandler.Contracts;

namespace PocketStatsHandler.Model
{
    public class PocketArticles: IPocketArticles
    {
        public int Status { get; set; }
        public int Complete { get; set; }
        public IEnumerable<IPocketArticle> Articles { get; set; }
        public int Since { get; set; }
    }
}
