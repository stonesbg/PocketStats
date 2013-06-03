using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using PocketStatsHandler.Contracts;
using PocketStatsHandler.Utilities;

namespace PocketStatsHandler
{
    public enum ContentType
    {
        Json,
        UrlEncoded
    }

    public class PocketStatsHandler
    {
        private const string RequestOathAuthorizeUrl = "https://getpocket.com/v3/oauth/authorize";
        private const string RequestAccessTokenUrl = "https://getpocket.com/v3/oauth/request";
        private const string RequestLoginProcessUrl = "https://getpocket.com/login_process";
        private const string RequestGetArticlesUrl = "https://getpocket.com/v3/get";
        private const string RequestAuthorizeUrl = "https://getpocket.com/auth/authorize";

        private const string ValueHtmlMarker = "value=\"";
        private const string ResponseCodeToken = "code";

        // Step 1
        public string RequestAccessToken(string consumerToken, string redirectUri, ContentType type)
        {
            var sb = new StringBuilder();
            if (type == ContentType.Json)
            {
                sb.Append("{");
                sb.Append("\"consumer_key\":\"");
                sb.Append(consumerToken);
                sb.Append("\", ");
                sb.AppendLine();
                sb.Append("\"redirect_uri\":\"");
                sb.Append(redirectUri);
                sb.Append("}");
            }
            else
            {
                sb.Append("consumer_key=");
                sb.Append(consumerToken);
                sb.Append("&redirect_uri=");
                sb.Append(redirectUri);
            }

            var response = SubmitRequest(new PocketWebRequest
                                         {
                                   ContentyType = type == ContentType.Json? ContentTypeEncodeing.JsonUtf8Encoded: ContentTypeEncodeing.FormUrlEncoded,
                                   Request = sb.ToString(),
                                   RequestMethod = WebRequestMethods.Http.Post,
                                   Url = RequestAccessTokenUrl
                               });

            string code = ParseCodeFromRequestAccessToken(response);

            return code;
        }

        private string ParseCodeFromRequestAccessToken(string responseToken)
        {
            string code = string.Empty;

            string[] tokens = responseToken.Split('=');

            bool foundCodeKey = false;
            foreach (string tokenValue in tokens)
            {
                if (foundCodeKey)
                {
                    code = tokenValue;
                    break;
                }

                if (tokenValue.ToLower() == ResponseCodeToken)
                    foundCodeKey = true;
            }

            return code;
        }

        public string AuthorizePocket(string responseCode, string redirectUri, string username, string password, ContentType type)
        {
            var buildUrl = new StringBuilder();
            buildUrl.Append(RequestAuthorizeUrl);
            buildUrl.Append("?request_token=");
            buildUrl.Append(responseCode);
            buildUrl.Append("&redirect_uri=");
            buildUrl.Append(redirectUri);

            var response = SubmitRequest(new PocketWebRequest
                                         {
                                                  ContentyType = type == ContentType.Json?ContentTypeEncodeing.JsonUtf8Encoded:ContentTypeEncodeing.FormUrlEncoded,
                                                  RequestMethod = WebRequestMethods.Http.Get,
                                                  Url = buildUrl.ToString()
                                              });

            LoginProcess loginProcess = ParseAuthorizePage(response);

            string loginProcessUriParameters = BuildLoginProcessUri(loginProcess, username, password);

            AttemptLoginProcess(loginProcessUriParameters, type, buildUrl.ToString());

            return response;
        }

        private void AttemptLoginProcess(string loginProcessUriParameters, ContentType type, string referer)
        {
            SubmitRequest(new PocketWebRequest
                          {
                                   ContentyType =
                                       type == ContentType.Json
                                           ? ContentTypeEncodeing.JsonUtf8Encoded
                                           : ContentTypeEncodeing.FormUrlEncoded,
                                   Request = loginProcessUriParameters,
                                   RequestMethod = WebRequestMethods.Http.Post,
                                   Url = RequestLoginProcessUrl
                               });

            //HttpWebRequest request = WebRequest.Create(RequestLoginProcessUrl) as HttpWebRequest;
            //request.Method = WebRequestMethods.Http.Post;
            //request.KeepAlive = true;
            //request.Referer = referer;
        }

        private string BuildLoginProcessUri(LoginProcess loginProcess, string username, string password)
        {
            var parameterBuilder = new StringBuilder();
            parameterBuilder.Append("feed_id=");
            parameterBuilder.Append(username);
            parameterBuilder.Append("&password=");
            parameterBuilder.Append(password);
            parameterBuilder.Append("&form_check=");
            parameterBuilder.Append(HttpUtility.UrlEncode(loginProcess.FormCheck));
            parameterBuilder.Append("&source=");
            parameterBuilder.Append(HttpUtility.UrlEncode(loginProcess.Source));
            parameterBuilder.Append("&route=");
            parameterBuilder.Append(HttpUtility.UrlEncode(loginProcess.Route));
            return parameterBuilder.ToString();
        }

        public LoginProcess ParseAuthorizePage(string result)
        {
            var loginProcess = new LoginProcess();

            bool foundLoginSection = false;
            var reader = new StringReader(result);
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();

                if (foundLoginSection)
                {
                    if(line != null && line.Contains("type=\"hidden\""))
                    {
                        if (line.Contains("name=\"form_check\""))
                        {
                            loginProcess.FormCheck = ReviewValueFieldFromHtml(line);
                        }
                        else if (line.Contains("name=\"source\""))
                        {
                            loginProcess.Source = ReviewValueFieldFromHtml(line);
                        }
                        else if (line.Contains("name=\"route\""))
                        {
                            loginProcess.Route = ReviewValueFieldFromHtml(line);
                        }
                        else
                            continue;
                    }
                }

                if (line != null && (line.Contains("form") && line.Contains("action=\"/login_process\"")))
                    foundLoginSection = true;
            }

            return loginProcess;
        }

        private string ReviewValueFieldFromHtml(string htmlLine)
        {
            int valueLocation = htmlLine.IndexOf(ValueHtmlMarker, StringComparison.Ordinal) + ValueHtmlMarker.Count();
            string startOfValueString = htmlLine.Substring(valueLocation);
            int indexOfEndValueLocation = startOfValueString.IndexOf('"');

            return startOfValueString.Substring(0, indexOfEndValueLocation);
        }

        public string RetreiveAccessToken(string consumerToken, string responseCode, ContentType type)
        {
            var sb = new StringBuilder();

            if (type == ContentType.Json)
            {
                sb.Append("{");
                sb.Append("\"consumer_key\":\"");
                sb.Append(consumerToken);
                sb.Append("\", ");
                sb.AppendLine();
                sb.Append("\"code\":\"");
                sb.Append(responseCode);
                sb.Append("\"");
                sb.Append("}");
            }
            else
            {
                sb.Append("consumer_key=");
                sb.Append(consumerToken);
                sb.Append("&code=");
                sb.Append(responseCode);
            }

            var response = SubmitRequest(new PocketWebRequest
                                         {
                                   ContentyType =
                                       type == ContentType.Json
                                           ? ContentTypeEncodeing.JsonUtf8Encoded
                                           : ContentTypeEncodeing.FormUrlEncoded,
                                   Request = sb.ToString(),
                                   RequestMethod = WebRequestMethods.Http.Post,
                                   Url = RequestOathAuthorizeUrl
                               });

            return response;
        }

        public class PocketWebRequest
        {
            public string Url { get; set; }

            public string ContentyType { get; set; }

            public string RequestMethod { get; set; }

            public string Request { get; set; }
        }

        public string SubmitRequest(PocketWebRequest request)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(request.Url);
            httpWebRequest.Method = request.RequestMethod;
            httpWebRequest.ContentType = request.ContentyType;

            if (request.RequestMethod != WebRequestMethods.Http.Get)
            {
                using (var requestStream = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    requestStream.Write(request.Request);
                }
            }

            var response = (HttpWebResponse)httpWebRequest.GetResponse();
            string result;
            using ( var rdr = new StreamReader(response.GetResponseStream()))
            {
                result = rdr.ReadToEnd();
            }

            return result;
        }

        public IPocketArticles GetPocketArticles(string consumerToken, string userAccessToken, ContentType type)
        {
            var sb = new StringBuilder();
            sb.Append("consumer_key=");
            sb.Append(consumerToken);
            sb.Append("&access_token=");
            sb.Append(userAccessToken);

            string response = SubmitRequest(new PocketWebRequest
                                            {
                                   ContentyType = type == ContentType.Json?ContentTypeEncodeing.JsonUtf8Encoded:ContentTypeEncodeing.FormUrlEncoded,
                                   RequestMethod = WebRequestMethods.Http.Post,
                                   Request = sb.ToString(),
                                   Url = RequestGetArticlesUrl
                               });

            var articles = JsonConvert.DeserializeObject<IPocketArticles>(response, new PocketArticlesJsonConverter(), new PocketArticleJsonConverter());

            return articles;
        }

    }
}