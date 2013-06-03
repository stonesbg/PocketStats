using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PocketStatsHandler;

namespace PocketStatsHandlerTests
{
    [TestClass]
    public class PocketStatsHandlerTests
    {       
        private const string ConsumerToken = "";
        private const string RedirectUri = "";
        private const string TestAccessToken = "";
        private const string UserName = "";
        private const string Password = "";

        [TestMethod]
        [TestCategory("Integration")]
        public void RequestAccessTokenTest()
        {
            var handler = new PocketStatsHandler.PocketStatsHandler();
            string response = handler.RequestAccessToken(ConsumerToken, RedirectUri, ContentType.UrlEncoded);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void AuthorizePocketTest()
        {
            var handler = new PocketStatsHandler.PocketStatsHandler();

            //Step 1
            string responseCode = handler.RequestAccessToken(ConsumerToken, RedirectUri, ContentType.UrlEncoded);

            //Step 2
            handler.AuthorizePocket(responseCode, RedirectUri, UserName, Password, ContentType.UrlEncoded);

            //Step 3
            string accessCode = handler.RetreiveAccessToken(ConsumerToken, responseCode, ContentType.UrlEncoded);

            Assert.IsNotNull(accessCode);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void GetArticlesTest()
        {
            var handler = new PocketStatsHandler.PocketStatsHandler();
            var articles = handler.GetPocketArticles(ConsumerToken, TestAccessToken, ContentType.UrlEncoded);

            Assert.IsTrue(articles.Articles.Count() > 0);
        }
    }
}
