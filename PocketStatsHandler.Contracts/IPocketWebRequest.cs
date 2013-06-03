namespace PocketStatsHandler.Contracts
{
    public interface IPocketWebRequest
    {
        string Url { get; set; }

        string ContentyType { get; set; }

        string RequestMethod { get; set; }

        string Request { get; set; }
    }
}