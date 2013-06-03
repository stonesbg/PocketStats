using System.Collections.Generic;

namespace PocketStatsHandler.Contracts
{
    public interface IPocketArticles
    {
        int Status { get; set; }

        int Complete { get; set; }

        IEnumerable<IPocketArticle> Articles { get; set; }

        int Since { get; set; }
    }
}