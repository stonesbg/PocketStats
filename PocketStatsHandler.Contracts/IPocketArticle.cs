namespace PocketStatsHandler.Contracts
{
    public interface IPocketArticle
    {
        string ItemId { get; set; }

        string ResolvedId { get; set; }

        string GivenUrl { get; set; }

        string GivenTitle { get; set; }

        string Favorite { get; set; }

        string Status { get; set; }

        string TimeAdded { get; set; }

        string TimeUpdated { get; set; }

        string TimeRead { get; set; }

        string TimeFavorited { get; set; }

        int SortId { get; set; }

        string ResolvedTitle { get; set; }

        string ResolvedUrl { get; set; }

        string Excerpt { get; set; }

        string IsArticle { get; set; }

        string IsIndex { get; set; }

        string HasVideo { get; set; }

        string HasImage { get; set; }

        string WordCount { get; set; }
    }
}