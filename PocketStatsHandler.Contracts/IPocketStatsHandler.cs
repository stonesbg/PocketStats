namespace PocketStatsHandler.Contracts
{
    public interface IPocketStatsHandler
    {
        string RequestAccessToken(string consumerToken, string redirectUri, ContentType type);

        string AuthorizePocket(string responseCode, string redirectUri, string username, string password, ContentType type);

        string RetreiveAccessToken(string consumerToken, string responseCode, ContentType type);

        IPocketArticles GetPocketArticles(string consumerToken, string userAccessToken, ContentType type);

        string SubmitRequest(IPocketWebRequest request);
    }
}