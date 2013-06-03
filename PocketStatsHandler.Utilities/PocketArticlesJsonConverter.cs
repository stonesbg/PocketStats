using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PocketStatsHandler.Contracts;
using PocketStatsHandler.Model;

namespace PocketStatsHandler.Utilities
{
    public class PocketArticlesJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Name == "IPocketArticles";
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            PocketArticles pocketArticles1 = new PocketArticles();
            int? nullable;
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    switch (reader.Path)
                    {
                        case "status":
                            PocketArticles pocketArticles2 = pocketArticles1;
                            nullable = reader.ReadAsInt32();
                            int valueOrDefault1 = nullable.GetValueOrDefault();
                            pocketArticles2.Status = valueOrDefault1;
                            break;
                        case "complete":
                            PocketArticles pocketArticles3 = pocketArticles1;
                            nullable = reader.ReadAsInt32();
                            int valueOrDefault2 = nullable.GetValueOrDefault();
                            pocketArticles3.Complete = valueOrDefault2;
                            break;
                        case "list":
                            pocketArticles1.Articles = serializer.Deserialize<IEnumerable<IPocketArticle>>(reader);
                            break;
                        case "since":
                            PocketArticles pocketArticles4 = pocketArticles1;
                            nullable = reader.ReadAsInt32();
                            int valueOrDefault3 = nullable.GetValueOrDefault();
                            pocketArticles4.Since = valueOrDefault3;
                            break;
                    }
                }
            }
            reader.Read();
            return (object) pocketArticles1;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}